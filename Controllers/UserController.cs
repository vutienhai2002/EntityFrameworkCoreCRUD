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
                HttpContext.Session.SetString("Name", b.Name);
                HttpContext.Session.SetString("Address", b.Address);
                HttpContext.Session.SetString("Phone", b.Phone);
                HttpContext.Session.SetString("Email", b.Email);




                return RedirectToAction("Index", "Home");

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Account or password is incorrect");
                return View("Login");
            }
		}
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa hết các session

            return RedirectToAction("Login", "User"); // Chuyển hướng đến trang đăng nhập (hoặc trang chính của bạn)
        }

        public IActionResult Signupp()
		{
			return View();
		}
        public IActionResult Signupuser(User user, string checkpassword)
        {
            if (user.password != checkpassword)
            {	
                ModelState.AddModelError(string.Empty, "Password and confirm password do not match");
                return View("Signupp");
            }

            if (_userService.IsUsernameTaken(user.username))
            {
                ModelState.AddModelError(string.Empty, "Username available");
                return View("Signupp");
            }

			user.role = "admin";
            _userService.AddUser(user);
            return RedirectToAction("Login", "User");
        }


}
}
