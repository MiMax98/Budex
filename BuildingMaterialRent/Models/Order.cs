using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingMaterialRent.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? PickupDate { get; set; }
        public DateTime? DueDate { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }

    public enum OrderStatus
    {
        Basket = 1,
        OrderCreated = 2,
        OrderPickedUp = 3,
        OrderReturned = 4
    }
}
