﻿
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

        public bool IsEmailExists(string email)
        {
            return _context.Buyers.Any(b => b.Email == email);
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

        public Buyer GetBuyerById(int id)
        {
            return _context.Buyers.FirstOrDefault(b => b.BuyerId == id);
        }

        public interface IBuyerService
        {
            Buyer GetBuyerById(int id);
            // Other method definitions
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
    }
}
