using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Models;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Index(int page = 1)
        {
            const int PageSize = 5; // Number of items per page

            if (HttpContext.Session.GetString("Username") != null)
            {
                var username = HttpContext.Session.GetString("Username");
                ViewBag.Username = username;

                var totalCars = _context.Cars.Count();
                var totalPages = (int)Math.Ceiling(totalCars / (double)PageSize);
                var cars = _context.Cars
                                    .OrderBy(c => c.CarId) // Optional: Order by some property
                                    .Skip((page - 1) * PageSize)
                                    .Take(PageSize)
                                    .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;

                return View(cars);
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

        public IActionResult CreateCar(Carmodel carObj, IFormFile image)
        {
            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (!allowedExtensions.Contains(fileExtension))
            {
                return RedirectToAction("Index");
            }
            // Ensure unique file name
            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);
            // Create directory if it doesn't exist
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            try
            {
                // Save the file
                using (var stream = new FileStream(path, FileMode.Create))
                {
                     image.CopyTo(stream);
                }
            }
            catch (Exception ex)
            {
                // Handle file saving exceptions
                // Optionally log the error and return an appropriate view
                return RedirectToAction("Error", "Home");
            }
            var relativePath = Path.Combine("images", uniqueFileName).Replace("\\", "/");
            var car = new Car
            {
                Manufacturer = carObj.Manufacturer,
                Model = carObj.Model,
                Year = carObj.Year,
                Color = carObj.Color,
                EngineType = carObj.EngineType,
                Mileage = carObj.Mileage,
                Price = carObj.Price,
                Img = relativePath,
            };
            // Save the car object to the database
            _context.Cars.Add(car);
            _context.SaveChanges();
            TempData["ResultOk"] = "Car created successfully!";

            return RedirectToAction("index", "Car");
        }



        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                var username = HttpContext.Session.GetString("Username");
                ViewBag.Username = username;
                var car = _context.Cars.Find(id);
                return View(car);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditCar(Car carObj, IFormFile image)
        {

            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (!allowedExtensions.Contains(fileExtension))
            {
                return RedirectToAction("Index");
            }
            // Ensure unique file name
            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);
            // Create directory if it doesn't exist
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            try
            {
                // Save the file
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
            }
            catch (Exception ex)
            {
                // Handle file saving exceptions
                // Optionally log the error and return an appropriate view
                return RedirectToAction("Error", "Home");
            }
            var relativePath = Path.Combine("images", uniqueFileName).Replace("\\", "/");

           
            var carFromDb = _context.Cars.Find(carObj.CarId);
            if (!string.IsNullOrEmpty(carFromDb.Img))
            {
                DeleteImage(carFromDb.Img);
            }
            carFromDb.Manufacturer = carObj.Manufacturer;
            carFromDb.Model = carObj.Model;
            carFromDb.Year = carObj.Year;
            carFromDb.Color = carObj.Color;
            carFromDb.EngineType = carObj.EngineType;
            carFromDb.Mileage = carObj.Mileage;
            carFromDb.Price = carObj.Price;
            carFromDb.Img = relativePath;

            _context.Cars.Update(carFromDb);
            _context.SaveChanges();
            

            return RedirectToAction("Index");
        }

       
        private bool DeleteImage(string imagePath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);
            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<IActionResult> DeleteCar(int id)
        {
            
                var carFromDb = _context.Cars.Find(id);
                
                if (!string.IsNullOrEmpty(carFromDb.Img))
                {
                    DeleteImage(carFromDb.Img);
                }
                _context.Cars.Remove(carFromDb);
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
        public async Task<IActionResult> CarDetails(int id)
        {
            var car = await _context.Cars
                .FirstOrDefaultAsync(c => c.CarId == id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }
    }



}
