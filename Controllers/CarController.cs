using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Models;

namespace EntityFrameworkCoreCRUD.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;


        public CarController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                var username = HttpContext.Session.GetString("Username");
                ViewBag.Username = username;
                List<Car> carList = _context.Cars.ToList();
            return View(carList);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                var username = HttpContext.Session.GetString("Username");
                ViewBag.Username = username;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public IActionResult CreateCar(Carmodel carObj )
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem file đã được upload chưa

                var car = new Car {
                    Manufacturer = carObj.Manufacturer,
                    Model = carObj.Model,
                    Year = carObj.Year,
                    Color = carObj.Color,
                    EngineType = carObj.EngineType,
                    Mileage = carObj.Mileage,
                    Price = carObj.Price,
                    Img = carObj.Img,

                };

                _context.Cars.Add(car);
                _context.SaveChanges();

                TempData["ResultOk"] = "Record Added Successfully!";
                return RedirectToAction("Index", "Car");
            }

            // Nếu trạng thái mô hình không hợp lệ, hãy trả về chế độ xem có lỗi xác thực         
            return RedirectToAction("create", "Car");

        }

        public IActionResult Edit(int? id)
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                var username = HttpContext.Session.GetString("Username");
                ViewBag.Username = username;
                if (id == null || id == 0)
            {
                return NotFound();
            }
            var carFromDb = _context.Cars.Find(id);
            if (carFromDb == null)
            {
                return NotFound();
            }
            return View(carFromDb);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
            
        
        public IActionResult EditCar(Carmodel carObj)
        {
            if (ModelState.IsValid)
            {
                var car = new Car
                {
                    Manufacturer = carObj.Manufacturer,
                    Model = carObj.Model,
                    Year = carObj.Year,
                    Color = carObj.Color,
                    EngineType = carObj.EngineType,
                    Mileage = carObj.Mileage,
                    Price = carObj.Price,
                    Img = carObj.Img,

                };
                _context.Cars.Update(car);
                _context.SaveChanges();
                TempData["ResultOk"] = "Data Updated Successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(carObj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var carFromDb = _context.Cars.Find(id);
            if (carFromDb == null)
            {
                return NotFound();
            }
            return View(carFromDb);
        }

      
        public IActionResult DeleteCar(int id)
        {

            var empfromdb = _context.Cars.Find(id);

            _context.Cars.Remove(empfromdb);
            _context.SaveChanges();
            TempData["ResultOk"] = "Data Deleted Successfully!";
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Viewcar()
        {
            List<Car> carList = _context.Cars.ToList();
            return View(carList);
        }

        public IActionResult SearchCar(string searchTerm)
        {
            var cars = string.IsNullOrEmpty(searchTerm)
                ? _context.Cars.ToList()
                : _context.Cars.Where(c => c.Manufacturer.Contains(searchTerm)
                                           || c.Model.Contains(searchTerm)
                                           || c.Color.Contains(searchTerm)
                                           || c.EngineType.Contains(searchTerm)
                                           || c.Year.ToString().Contains(searchTerm)
                                           || c.Mileage.ToString().Contains(searchTerm)
                                           || c.Price.ToString().Contains(searchTerm)).ToList();

            return View("ViewCar", cars);
        }
        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();
                
            // Optionally, you can also clear any cookies related to authentication
            Response.Cookies.Delete(".AspNetCore.Identity.Application");

            // Redirect to the login page or home page
            return RedirectToAction("Login", "User");
        }
    }


}
