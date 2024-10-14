using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Models;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace EntityFrameworkCoreCRUD.Service
{
    public class CarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Car> GetAllCars()
        {
            return _context.Cars.ToList();
        }

        public Car GetCarById(int id)
        {
            /*            return _context.Cars.Find(id);
             *            
            */
            return _context.Cars.FirstOrDefault(c => c.CarId == id);
        }

        public void CreateCar(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
        }

        public void UpdateCar(Car car)
        {
            _context.Cars.Update(car);
            _context.SaveChanges();
        }

        public void DeleteCar(int id)
        {
            var car = _context.Cars.Find(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
        }
        public void GuiEmailXacMinh(Contact contact, String Email)
        {
            try
            {
                string fromEmail = "vutienhai2002@gmail.com";
                string password = "yemzrosipfnimkcn";
                string toEmail = Email;
                MailMessage message = new MailMessage(fromEmail, toEmail);
                message.Subject = "";
                StringBuilder bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine($" {contact.Message}");
                message.Body = bodyBuilder.ToString();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Loi: " + ex.Message);
            }
        }
    }
}
