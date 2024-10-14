
using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Models;

namespace EntityFrameworkCoreCRUD.Service
{
    public class BuyerService : IBuyerService
    {
        private readonly ApplicationDbContext _context;

        public BuyerService(ApplicationDbContext context)
        {
            _context = context;
        }


        public bool IsUsernameExists(string username)
        {
            return _context.Buyers.Any(b => b.Username == username);
        }

        public bool IsEmailExists(string email)
        {
            return _context.Buyers.Any(b => b.Email == email);
        }

        public bool IsPhoneExists(string phone)
        {
            return _context.Buyers.Any(b => b.Phone == phone);
        }

        public void AddBuyer(Buyer buyer)
        {
            _context.Buyers.Add(buyer);
            _context.SaveChanges();
        }
        public int AddBuyers(Buyer buyer)
        {
            _context.Buyers.Add(buyer);
            _context.SaveChanges();
            return buyer.BuyerId; // Assuming that Id is the primary key and it is set after SaveChanges.
        }

        public List<Buyer> GetAllBuyers()
        {
            return _context.Buyers.ToList();
        }
            public Buyer GetBuyerById(int id)
        {
            return _context.Buyers.FirstOrDefault(b => b.BuyerId == id);
        }
        public Buyer GetBuyerByIdSS(int id)
        {
            return _context.Buyers.Find(id);
        }

        public interface IBuyerService
        {
            Buyer GetBuyerById(int id);
            // Other method definitions
        }
        public Buyer GetBuyerByUsername(string username)
        {
            return _context.Buyers.FirstOrDefault(b => b.Username == username);
        }
        public Buyer CheckUsernamePassword(string username, string password)
        {
            return _context.Buyers.FirstOrDefault(b => b.Username == username && b.Password == password);
        }
        public void UpdateBuyer(Buyer buyer)
        {
            var existingBuyer = _context.Buyers.FirstOrDefault(b => b.BuyerId == buyer.BuyerId);
            if (existingBuyer == null)
            {
                throw new InvalidOperationException("Buyer not found.");
            }

            // Kiểm tra email đã tồn tại
            if (_context.Buyers.Any(b => b.Email == buyer.Email && b.BuyerId != buyer.BuyerId))
            {
                throw new InvalidOperationException("Email already exists.");
            }

            existingBuyer.Name = buyer.Name;
            existingBuyer.Address = buyer.Address;
            existingBuyer.Phone = buyer.Phone;
            existingBuyer.Email = buyer.Email;

            _context.SaveChanges();
        }
      /*  public string GetBuyerRole(int buyerId)
        {
            var buyer = _context.Buyers.Find(buyerId);
            return buyer?.role;
        }*/
    }
}
