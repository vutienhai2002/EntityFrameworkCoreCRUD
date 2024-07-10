using EntityFrameworkCoreCRUD.Models;
using EntityFrameworkCoreCRUD.Service;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCoreCRUD.Controllers
{
	public class UserController : Controller
	{
		private readonly UserService _userService;
        private readonly BuyerService _buyerService;

        public UserController(UserService userService, BuyerService buyerService)
		{
			_userService = userService;
            _buyerService= buyerService;

        }
		public IActionResult Login()
		{
			return View();
		}
		public IActionResult Loginuser(string username, string password)
		{
			User u = _userService.CheckUsernamePassword(username, password);
            Buyer b = _buyerService.CheckUsernamePassword(username, password);


            if (u != null)
            {
                HttpContext.Session.SetString("Username", u.username);
                HttpContext.Session.SetInt32("UserId", u.userId);



                return RedirectToAction("Index", "Car");
            }
            else if (b != null)
            {
                HttpContext.Session.SetString("Username", b.Username);
                HttpContext.Session.SetInt32("UserId", b.BuyerId);

                return RedirectToAction("BuyCar", "BuyCar");

            }


            else
            {
                // Invalid credentials, display error message
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View("Login");
            }
		}
		public IActionResult Signupp()
		{
			return View();
		}
        public IActionResult Signupuser(User user, string checkpassword)
        {
            if (user.password != checkpassword)
            {	
                ModelState.AddModelError(string.Empty, "Mật khẩu và xác nhận mật khẩu không khớp");
                return View("Signupp");
            }

            if (_userService.IsUsernameTaken(user.username))
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập đã tồn tại");
                return View("Signupp");
            }
			user.role = "admin";
            _userService.AddUser(user);
            return RedirectToAction("Login", "User");
        }


}
}
