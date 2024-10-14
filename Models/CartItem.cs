using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreCRUD.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string Img { get; set; }

    }
}
