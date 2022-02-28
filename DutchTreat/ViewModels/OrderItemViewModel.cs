using System.ComponentModel.DataAnnotations;

namespace DutchTreat.ViewModels
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int ProductId { get; set; }
        public string ProductCategory { get; set; } = string.Empty;
        public string ProductSize { get; set; } = string.Empty;
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductArtist { get; set; } = string.Empty;
        public string ProductArtId { get; set; } = string.Empty;
    }

}

