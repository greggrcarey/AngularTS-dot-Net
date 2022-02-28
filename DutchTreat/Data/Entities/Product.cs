using System.ComponentModel.DataAnnotations.Schema;

namespace DutchTreat.Data.Entities
{
    public class Product
    {
        
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        [Column(TypeName = "decimal(20,2)")]
        public decimal Price { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ArtDescription { get; set; }
        public string ArtDating { get; set; } = string.Empty;
        public string ArtId { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public DateTime ArtistBirthDate { get; set; }
        public DateTime ArtistDeathDate { get; set; }
        public string ArtistNationality { get; set; } = string.Empty;
    }
}
