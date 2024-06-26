using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCoreCRUD.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy danh sách các giao dịch từ cơ sở dữ liệu
            List<Transaction> transactions = _context.Transactions
                .Include(t => t.Car)
                .Include(t => t.Seller)
                .Include(t => t.Buyer)
                .ToList();

            // Trả về trang index.cshtml và truyền danh sách giao dịch vào
            return View(transactions);
        }

        public IActionResult Create()
        {
            List<Car> listcart = _context.Cars.ToList();
            return View(listcart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transaction);
                _context.SaveChanges();
                TempData["ResultOk"] = "Transaction created successfully!";
                return RedirectToAction("Index");
            }
            // Reload dropdown lists in case of validation errors
            ViewBag.CarIdList = new SelectList(_context.Cars, "CarId", "CarName", transaction.CarId);
            ViewBag.SellerIdList = new SelectList(_context.Sellers, "SellerId", "SellerName", transaction.SellerId);
            ViewBag.BuyerIdList = new SelectList(_context.Buyers, "BuyerId", "BuyerName", transaction.BuyerId);
            return View(transaction);
        }

    }
}
