using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreCRUD.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }
        public int CarId { get; set; }

        public Car Cars { get; set; }

    }
}
