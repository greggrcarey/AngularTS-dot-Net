
using System.ComponentModel.DataAnnotations.Schema;

namespace DutchTreat.Data.Entities
{
    public class Order
    {
        
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public ICollection<OrderItem>? Items { get; set; }
        public StoreUser User { get; set; } = new StoreUser();

    }
}
