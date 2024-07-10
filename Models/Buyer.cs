using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreCRUD.Models
{
    public class Buyer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BuyerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)] // Điều chỉnh độ dài cho phù hợp
        public string Username { get; set; }

        [Required]
        [StringLength(50)] // Điều chỉnh độ dài cho phù hợp
        public string Password { get; set; }

    }
}
