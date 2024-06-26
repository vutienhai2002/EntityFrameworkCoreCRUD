using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreCRUD.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        public int BuyerId { get; set; }
        [StringLength(500)]
        public string Message { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
