using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingMaterialRent.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }
        [Required]
        [MaxLength(50)]
        public string Image { get; set; }
        [Required]
        public int Stock { get; set; }

        public int CategoryId { get; set; }
    }
}
