using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCoreCRUD.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
