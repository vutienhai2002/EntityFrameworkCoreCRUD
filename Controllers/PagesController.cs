using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCoreCRUD.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Blogdetails()
        {
            return View();
        }
        public IActionResult Cardetails()
        {
            return View();
        }
    }
}
