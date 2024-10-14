using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Models;
using EntityFrameworkCoreCRUD.Services;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCoreCRUD.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
           
                // Check if the user is logged in
                if (HttpContext.Session.GetString("Username") != null)
                {
                    var username = HttpContext.Session.GetString("Username");
                    ViewBag.Username = username; // Display username in the view
                }
                // tính tổng số xe
                List<Car> listcart = _context.Cars.ToList();
            int tongsoluongxe = listcart.Count;
            ViewData["tongsoluongxe"] = tongsoluongxe;
            // tính tỏng khách hàng
            List<Buyer> listbuyer = _context.Buyers.ToList();
            int tongsoluongkhachhang = listbuyer.Count;
            ViewData["tongsoluongkhachhang"] = tongsoluongkhachhang;
            /// tính tổng đơn hàng mà khách hàng đã mua xe
            List<Order> listorder = _context.Orders.ToList();
            int tongsoluongorder = listorder.Count;
            ViewData["tongsoluongorder"] = tongsoluongorder;

            // tính tỏng doanh thu
            decimal doanhthu = listorder.Sum(dt => dt.TotalAmount);
            ViewData["doanhthu"] = doanhthu;
            return View();
        }
        public IActionResult TongPhanTramsanPham()
        {
            var listOrderDetail = _context.OrderDetails.ToList();

            // Calculate total quantity rented
            int totalQuantityRented = listOrderDetail.Sum(orderDetail => orderDetail.Quantity);

            // Group by CarId and sum quantities
            var groupedOrderDetails = listOrderDetail
                .GroupBy(order => order.CarId)
                .Select(group => new
                {
                    CarId = group.Key,
                    Quantity = group.Sum(order => order.Quantity),
                })
                .ToList();

            // Calculate percentages and fetch cars in a single query
            var carPercentages = groupedOrderDetails
                .Select(item =>
                {
                    var car = _context.Cars.Find(item.CarId); // Change to Find for efficiency
                    car.OrderDetails = null;
                    car.Contact = null;
                    return new Modeldatas
                    {
                        car = car,
                        toangdoanhthu = totalQuantityRented > 0 ? (item.Quantity / (decimal)totalQuantityRented) * 100 : 0
                    };
                })
                .Where(item => item.car != null) // Filter out any null cars
                .OrderByDescending(p => p.toangdoanhthu)
                .ToList();

            return Ok(carPercentages);
        }

        public IActionResult Doanhthutheothang()
        {
            List<Order> orders = _context.Orders.ToList();

            var doanhthutheothang = orders.GroupBy(od => od.OrderDate.Month)
                .Select(g => new
                {
                    Thang = g.Key,
                    Tongdoanhthu = g.Sum(dt => dt.TotalAmount)
                }).ToList();
            List<object> result = new List<object>();
            foreach (var doanhthu in doanhthutheothang)
            {
                Doanhthuthangs doanhthuthangs = new Doanhthuthangs()
                {
                    Thang = doanhthu.Thang,
                    tongdoanhthu = (float)doanhthu.Tongdoanhthu,
                };
                result.Add(doanhthuthangs);
            }
            return Json(result);
        }
        private class Doanhthuthangs
        {
            public int Thang { get; set; }
            public float tongdoanhthu { get; set; }
        }
        public class Modeldatas
        {
            public Car car { get; set; }
            public decimal toangdoanhthu { get; set; }
        }

    }
}
