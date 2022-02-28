using System.ComponentModel.DataAnnotations.Schema;

namespace DutchTreat.Data.Entities
{
    public class OrderItem
    {
        
        public int Id { get; set; }
        public Product Product { get; set; } = new Product();
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(20, 2)")]
        public decimal UnitPrice { get; set; }
        public Order Order { get; set; } = new Order();
    }
}