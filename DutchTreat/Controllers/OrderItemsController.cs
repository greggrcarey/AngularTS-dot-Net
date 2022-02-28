using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    [Route("/api/orders/{orderId:int}/items")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController : ControllerBase
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<OrderItemsController> logger;
        private readonly IMapper mapper;

        public OrderItemsController(IDutchRepository repository,
                                    ILogger<OrderItemsController> logger,
                                    IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            var userName = User?.Identity?.Name;
            if (userName is null) return BadRequest();

            var order = repository.GetOrderById(userName, orderId);
            if (order is not null && order.Items is not null) return Ok(mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
            return BadRequest();
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int orderId, int id)
        {
            var userName = User?.Identity?.Name;
            if (userName is null) return BadRequest();

            var order = repository.GetOrderById(userName, orderId);
            if (order is not null && order.Items is not null)
            {
                var orderItems = order.Items.Where(i => i.Id == id).FirstOrDefault();
                if (orderItems is not null)
                    return Ok(mapper.Map<OrderItem, OrderItemViewModel>(orderItems));
            }
            return NotFound();
        }
    }
}
