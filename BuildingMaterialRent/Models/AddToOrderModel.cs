using System.ComponentModel.DataAnnotations;

namespace BuildingMaterialRent.Models
{
    public class AddToOrderModel
    {
        public int ProductId { get; set; }
        [Range(1, 999)]
        public int Quantity { get; set; }
    }
}
