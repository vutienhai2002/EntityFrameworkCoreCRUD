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
            List<Buyer> buyers = _buyerService.GetAllBuyers();

            ViewBag.Buyers = buyers;

            // Fetch the logged-in buyer
            Buyer loggedInBuyer = null;
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name; // Assuming username is the unique identifier
                loggedInBuyer = _buyerService.GetBuyerByUsername(username);
            }
            ViewBag.LoggedInBuyer = loggedInBuyer;

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
