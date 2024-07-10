using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Models;
using EntityFrameworkCoreCRUD.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkCoreCRUD.Controllers
{
    public class BuyerController  : Controller 
    {

        private readonly ApplicationDbContext _context;
        private readonly BuyerService _buyerService;


        public BuyerController(ApplicationDbContext context, BuyerService buyerService)
        {
            _context = context;
            _buyerService = buyerService;

        }

        public IActionResult Index()
        {

            if (HttpContext.Session.GetString("Username") != null )
            {

                var username = HttpContext.Session.GetString("Username");
                ViewBag.Username = username;
                var objCatlist = _context.Buyers.ToList();
            if (objCatlist == null)
            {   
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
            if (_buyerService.IsUsernameExists(buyer.Username))
            {
                ViewData["ErrorMessage"] = "Username already exists.";
                return View("Create");
            }

            if (_buyerService.IsEmailExists(buyer.Email))
            {
                ViewData["ErrorMessage"] = "Email already exists.";
                return View("Create");  
            }

            if (ModelState.IsValid)
            {
                _buyerService.AddBuyer(buyer);
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
