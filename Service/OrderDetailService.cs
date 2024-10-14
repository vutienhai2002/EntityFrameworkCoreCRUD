using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Models;
using Microsoft.EntityFrameworkCore;

public class OrderDetailService
{
    private readonly ApplicationDbContext _context;

    public OrderDetailService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderDetail>> GetAllAsync()
    {
        return await _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Cars)
            .ToListAsync();
    }

    public OrderDetail GetByIdAsync(int id)
    {
        return _context.OrderDetails
            .Include(od => od.Order)
            .Include(od => od.Cars)
            .FirstOrDefault(od => od.OrderDetailId == id);
    }

    public void AddOrderDetail(OrderDetail orderDetail)
    {
        _context.OrderDetails.Add(orderDetail);
        _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(OrderDetail orderDetail)
    {
        _context.OrderDetails.Update(orderDetail);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var orderDetail = await _context.OrderDetails.FindAsync(id);
        if (orderDetail != null)
        {
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
        }
    }
}
