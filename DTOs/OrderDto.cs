using MiniInventoryManagementAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MiniInventoryManagementAPI.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Customer Name is required")]
        [StringLength(250)]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Order Date is required")]
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public Status Status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }

        public string? ProductName { get; set; }
    }

    public class OrderItemDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal SubTotal { get; set; }
    }

}
