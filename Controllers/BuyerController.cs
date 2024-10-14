using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Models;
using EntityFrameworkCoreCRUD.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EntityFrameworkCoreCRUD.Controllers
{
    public class BuyerController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly BuyerService _buyerService;


        public BuyerController(ApplicationDbContext context, BuyerService buyerService)
        {
            _context = context;
            _buyerService = buyerService;

        }

       
        public IActionResult Index(int page = 1)
        {
            const int PageSize = 5; // Number of items per page

            if (HttpContext.Session.GetString("Username") != null)
            {
                var username = HttpContext.Session.GetString("Username");
                ViewBag.Username = username;

                var totalBuyers = _context.Buyers.Count();
                var totalPages = (int)Math.Ceiling(totalBuyers / (double)PageSize);
                var buyers = _context.Buyers
                                     .OrderBy(b => b.BuyerId) // Optional: Order by some property
                                     .Skip((page - 1) * PageSize)
                                     .Take(PageSize)
                                     .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;

                return View(buyers);
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


        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult CreateBuyer(Buyer buyer)
        {
            if (ModelState.IsValid)
            {
                // Check for existing records
                if (_buyerService.IsUsernameExists(buyer.Name))
                {
                    ViewData["ErrorMessage"] = "Username already exists.";
                    return View("Create");
                }

                if (_buyerService.IsEmailExists(buyer.Email))
                {
                    ViewData["ErrorMessage"] = "Email already exists.";
                    return View("Create");
                }

                if (_buyerService.IsPhoneExists(buyer.Phone))
                {
                    ViewData["ErrorMessage"] = "Phone number already exists.";
                    return View("Create");
                }

                // Add the buyer
                _buyerService.AddBuyer(buyer);
                TempData["ResultOk"] = "Record Added Successfully!";
                return RedirectToAction("Index");
            }

            return View("index",buyer);
        }

        public IActionResult GetCurrentBuyer()
        {

            int buyerId = HttpContext.Session.GetInt32("UserId").Value;

            // Tìm Buyer trong cơ sở dữ liệu
            Buyer buyer = _context.Buyers.FirstOrDefault(b => b.BuyerId == buyerId);

            if (buyer == null)
            {
                return NotFound();
            }

            // Trả về thông tin buyer dưới dạng JSON
            return Json(buyer);
        }

    


    public IActionResult SignUpBuyer(Buyer buyer)
        {
            if (_buyerService.IsUsernameExists(buyer.Username))
            {
                ModelState.AddModelError(string.Empty, "Username already exists.");
            }

            if (_buyerService.IsEmailExists(buyer.Email))
            {
                ModelState.AddModelError(string.Empty, "Email already exists.");
            }

            if (_buyerService.IsPhoneExists(buyer.Phone))
            {
                ModelState.AddModelError(string.Empty, "Phone number already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View("SignUp", buyer);
            }

            _buyerService.AddBuyer(buyer);
            TempData["ResultOk"] = "Registration successful!";
            return RedirectToAction("SignUp");
        }

        public IActionResult Login()
        {
            return View();
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
                var Buyerfromdb = _context.Buyers.Find(id);
                if (Buyerfromdb == null)
                {
                    return NotFound();
                }
                return View(Buyerfromdb);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }


        public IActionResult EditBuyer(Buyer Buyerobj)
        {

            if (ModelState.IsValid)
            {
                _context.Buyers.Update(Buyerobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Data Updated Successfully!";
                return RedirectToAction("Index");
            }
            return View(Buyerobj);
        }









        public IActionResult DeleteBuyer(int? id)
        {
            var deleterecord = _context.Buyers.Find(id);
            if (deleterecord == null)
            {
                return NotFound();
            }
            _context.Buyers.Remove(deleterecord);
            _context.SaveChanges();
            TempData["ResultOk"] = "Data Deleted Successfully!";
            return RedirectToAction("Index");
        }

    }

}