using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiniInventoryManagementAPI.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Customer Name is required")]
        [StringLength(250)]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Order Date is required")]
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public Status Status { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public enum Status
    {
        Pending=0,
        Completed=1,
        Cancelled=2
    }
}
