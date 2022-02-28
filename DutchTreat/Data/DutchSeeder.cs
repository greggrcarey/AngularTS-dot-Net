using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;

        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext context, IWebHostEnvironment environment, UserManager<StoreUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("gcarey@dutchtreat.com");

            if (user is null)
            {
                user = new StoreUser()
                {
                    FirstName = "Gregg",
                    LastName = "Carey",
                    Email = "gcarey@dutchtreat.com",
                    UserName = "gcarey@dutchtreat.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Could not create new user in Seed()");
                }
            }

            if (!_context.Products.Any())
            {
                //Create some sample data
                var filePath = Path.Combine(_environment.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                _context.Products.AddRange(products!);

                var order = new Order
                {
                    User = user,
                    OrderDate = DateTime.UtcNow,
                    OrderNumber = "1000",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products!.First(),
                            Quantity = 5,
                            UnitPrice = products!.First().Price
                        }
                    }
                };

                _context.Orders.Add(order);

                _context.SaveChanges();


            }
        }

    }
}
