using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreCRUD.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        [Required]
        public string Model { get; set; }

        public int Year { get; set; }

        public string Color { get; set; }

        public string EngineType { get; set; }

        public int Mileage { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        public string? Img { get; set; }
        public int? ContactId { get; set; }
        [ForeignKey("ContactId")]
        public Contact Contact { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    }
}