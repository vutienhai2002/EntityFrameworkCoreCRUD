using EntityFrameworkCoreCRUD.Models;
using EntityFrameworkCoreCRUD.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EntityFrameworkCoreCRUD.Controllers
{
    public class BuycarController : Controller
    {
        private readonly CarService _carService;
        private readonly BuyerService _buyerService;

        public BuycarController(CarService carService, BuyerService buyerService)
        {
            _carService = carService;
            _buyerService = buyerService;
        }

        public IActionResult Buycar()
        {
            List<Car> cars = _carService.GetAllCars();
            

            return View(cars);
        }

        public IActionResult Register(Buyer buyer)
        {
            if (ModelState.IsValid)
            {
                _buyerService.AddBuyer(buyer);

                // Redirect to Buycar action after registration
                return RedirectToAction("Buycar");
            }

            // If registration fails, return to the registration form
            return View("Buycar", buyer);
        }
    }
}
