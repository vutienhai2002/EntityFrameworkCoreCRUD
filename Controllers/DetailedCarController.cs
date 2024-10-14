using Microsoft.AspNetCore.Mvc;
using EntityFrameworkCoreCRUD.Models;
using EntityFrameworkCoreCRUD.Data;

namespace EntityFrameworkCoreCRUD.Controllers
{
    public class DetailedCarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DetailedCarController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult DetailedCar(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            return View("DetailedCar", car);
        }
    }
}
