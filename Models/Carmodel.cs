using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreCRUD.Models
{
    public class Carmodel
    {
      
        public int CarId { get; set; }

       
        public string Manufacturer { get; set; }

        
        public string Model { get; set; }

        public int Year { get; set; }

        public string Color { get; set; }

        public string EngineType { get; set; }

        public int Mileage { get; set; }

        public decimal Price { get; set; }
        public string Img { get; set; }
      
    }
}