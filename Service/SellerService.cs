using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Models;

namespace EntityFrameworkCoreCRUD.Service
{
    public class SellerService
    {
        private readonly ApplicationDbContext _context;

        public SellerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsEmailExists(string email)
        {
            return _context.Sellers.Any(b => b.Email == email);
        }
        public void AddSeller(Seller seller)
        {
            _context.Sellers.Add(seller);
            _context.SaveChanges();
        }
        
    }
}
