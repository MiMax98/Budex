using System.ComponentModel.DataAnnotations;

namespace BuildingMaterialRent.Models
{
    public class UpdateOrderItem
    {
        public int OrderItemId { get; set; }
        [Range(1, 999)]
        public int Quantity { get; set; }
    }
}
