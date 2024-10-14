using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreCRUD.Models
{
    public class Order
    {
        [Key]

        public int OrderId { get; set; }
        public int BuyerId { get; set; } 
      
        public string Address { get; set; }
        public string Messager { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        // Điều hướng tới mô hình 
        public Buyer Buyer { get; set; }

    }
}
        