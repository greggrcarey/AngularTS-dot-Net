using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<StoreUser> _userManager;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper, UserManager<StoreUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var userName = User?.Identity?.Name;

                if (userName is not null)
                {
                    var result = _repository.GetAllOrdersByUser(userName, includeItems);

                    if (result is null) return NotFound();

                    return Ok(_mapper.Map<IEnumerable<OrderViewModel>>(result));
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return NotFound("Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var user = User?.Identity?.Name;
                if (user is null) return BadRequest();

                var order = _repository.GetOrderById(user, id);
                if (order is null) return NotFound();

                return Ok(_mapper.Map<Order, OrderViewModel>(order));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return NotFound("Failed to get order");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderViewModel order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<OrderViewModel, Order>(order);

                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.UtcNow;
                    }

                    if (User?.Identity?.Name is null) return BadRequest("User.Identity.Name was not found");

                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

                    newOrder.User = currentUser;

                    _repository.AddEntity(newOrder);

                    if (_repository.SaveAll())
                    {

                        return Created($"/api/order/{newOrder.Id}", _mapper.Map<Order, OrderViewModel>(newOrder));
                    }

                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                return NotFound(ex);
            }
        }



    }
}
