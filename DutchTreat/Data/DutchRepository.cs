using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _context;
        private readonly ILogger<DutchRepository> _logger;
        public DutchRepository(DutchContext context,
                               ILogger<DutchRepository> logger)
        {
            _context = context;
            _logger = logger;

        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("DutchRepository.GetAllProducts() called");
                return _context.Products.OrderBy(p => p.Title).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Enumerable.Empty<Product>();

            }

        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _context.Products.Where(p => p.Category == category).ToList();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                return _context.Orders
                     .Include(o => o.Items).ThenInclude(i => i.Product).ToList();
            }
            return _context.Orders.ToList();
        }
        public Order GetOrderById(int id) => _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.Id == id)
                .FirstOrDefault();


        public void AddEntity(object model)
        {
            _context.Add(model);
        }

        public IEnumerable<Order> GetAllOrdersByUser(string userName, bool includeItems)
        {
            return includeItems
                ? _context.Orders
                    .Where(o => o.User.UserName == userName)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .ToList()
                : _context.Orders
                .Where(o => o.User.UserName == userName)
                .ToList();
        }

        public Order GetOrderById(string userName, int id) => _context.Orders
               .Include(o => o.Items)
               .ThenInclude(i => i.Product)
               .Where(o => o.Id == id && o.User.UserName == userName)
               .FirstOrDefault();
    }
}