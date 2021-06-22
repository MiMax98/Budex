using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingMaterialRent.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }

        public int OrderId { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }
    }
}
