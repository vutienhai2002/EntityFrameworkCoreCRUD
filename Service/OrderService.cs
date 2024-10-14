using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCoreCRUD.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }


        public int AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.OrderId; // Assuming that Id is the primary key and it is set after SaveChanges.
        }


        // Other methods...
    }

}
