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
        
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                var username = HttpContext.Session.GetString("Username");
                ViewBag.Username = username;
                var objCatlist = _context.Sellers.ToList();
            if (objCatlist == null)
            {
                objCatlist = new List<Seller>(); // Khởi tạo một danh sách trống nếu không có dữ liệu
            }
            return View(objCatlist);
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
        public IActionResult EditSeller(Seller Sellerobj)
        {
            if (ModelState.IsValid)
            {
                _context.Sellers.Update(Sellerobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Data Updated Successfully!";
                return RedirectToAction("Index");
            }
            return View(Sellerobj);
        }

        public IActionResult DeleteSeller(int? id)
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
