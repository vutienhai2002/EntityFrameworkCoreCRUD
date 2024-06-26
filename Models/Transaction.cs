using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreCRUD.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        public int CarId { get; set; }

        public int SellerId { get; set; }

        public int BuyerId { get; set; }

        public DateTime TransactionDate { get; set; }

        [StringLength(100)]
        public string PaymentMethod { get; set; }

        [ForeignKey("CarId")]
        public Car Car { get; set; }

        [ForeignKey("SellerId")]
        public Seller Seller { get; set; }

        [ForeignKey("BuyerId")]
        public Buyer Buyer { get; set; }
    }
}
