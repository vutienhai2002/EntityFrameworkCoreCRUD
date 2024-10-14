using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Models;
using EntityFrameworkCoreCRUD.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkCoreCRUD.Controllers
{
    public class SellerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SellerService _sellerService;

        public SellerController(ApplicationDbContext context, SellerService sellerService)
        {
            _context = context;
            _sellerService = sellerService;
        }

        public IActionResult Index(int page = 1)
        {
            const int PageSize = 5; // Number of items per page

            if (HttpContext.Session.GetString("Username") != null)
            {
                var username = HttpContext.Session.GetString("Username");
                ViewBag.Username = username;

                var totalSellers = _context.Sellers.Count();
                var totalPages = (int)Math.Ceiling(totalSellers / (double)PageSize);
                var sellers = _context.Sellers
                                       .OrderBy(s => s.SellerId) // Optional: Order by some property
                                       .Skip((page - 1) * PageSize)
                                       .Take(PageSize)
                                       .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;

                return View(sellers);
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
        

        public IActionResult CreateSeller(Seller seller)
        {
            if (_sellerService.IsEmailExists(seller.Email))
            {
                ViewData["ErrorMessage"] = "Email already exists.";
                return View("Create");
            }

            if (ModelState.IsValid)
            {
                _sellerService.AddSeller(seller);
                TempData["ResultOk"] = "Record Added Successfully!";
                return RedirectToAction("Index");
            }
            return View("index");
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
                var Sellerfromdb = _context.Sellers.Find(id);
                if (Sellerfromdb == null)
                {
                    return NotFound();
                }
                return View(Sellerfromdb);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        public IActionResult EditSeller(Seller seller)
        {

            if (ModelState.IsValid)
            {
                _context.Sellers.Update(seller);
                _context.SaveChanges();
                TempData["ResultOk"] = "Data Updated Successfully !";
                return RedirectToAction("Index");
            }

            return View(seller);
        }

        public IActionResult DeleteSeller(int? id)
        {
            var deleterecord = _context.Sellers.Find(id);
            if (deleterecord == null)
            {
                return NotFound();
            }
            _context.Sellers.Remove(deleterecord);
            _context.SaveChanges();
            TempData["ResultOk"] = "Data Deleted Successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
