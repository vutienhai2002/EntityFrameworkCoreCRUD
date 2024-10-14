using EntityFrameworkCoreCRUD.Models;
using EntityFrameworkCoreCRUD.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using static EntityFrameworkCoreCRUD.Service.BuyerService;

namespace EntityFrameworkCoreCRUD.Controllers
{
    public class ContactController : Controller
    {
        private readonly ContactService contactService;
        private readonly BuyerService buyerService;
        private readonly CarService carService;
        public ContactController(ContactService contactService, BuyerService buyerService , CarService carService)
        {
            this.contactService = contactService;
            this.buyerService = buyerService;   
            this.carService = carService;
        }
        public IActionResult Contact()
        {

            return View();
        }

        public IActionResult AddContact(Contact contact,Buyer buyer )
        {
            buyer.Password = "null";
            buyer.Username = "null";
            int BuyerId = buyerService.AddBuyers(buyer);
            contact.BuyerId= BuyerId;
            contact.Date= DateTime.Now;
            contactService.AddContact(contact);
            TempData["ResultOk"] = "Contact Successfully!";
            return RedirectToAction("Contact", "Contact");
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                var username = HttpContext.Session.GetString("Username");
                ViewBag.Username = username;
                List<Contact> contactList = contactService.ListContact();
                return View(contactList);
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

        [HttpPost]
        public IActionResult CreateContact(Contact contact, Buyer buyer)
        {
            if (ModelState.IsValid)
            {
                int buyerId = buyerService.AddBuyers(buyer);
                contact.BuyerId = buyerId;
                contact.Date = DateTime.Now;
                contactService.AddContact(contact);
                TempData["ResultOk"] = "Contact Added Successfully!";
                return RedirectToAction("Index");
            }
            return View("Create");
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

                var contactFromDb = contactService.GetContactById(id.Value);
                if (contactFromDb == null)
                {
                    return NotFound();
                }

                return View(contactFromDb);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

    
        public IActionResult EditContact(int ContactId)
        {
            
                Contact contact = contactService.GetContactById(ContactId);
                Buyer buyer = buyerService.GetBuyerByIdSS(contact.BuyerId);
                carService.GuiEmailXacMinh(contact, buyer.Email); 
                TempData["ResultOk"] = "Contact Updated Successfully!";
                return RedirectToAction("Index");
            
        }

        public IActionResult Delete(int? id)
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }

                var contactFromDb = contactService.GetContactById(id.Value);
                if (contactFromDb == null)
                {
                    return NotFound();
                }

                return View(contactFromDb); // Show a confirmation view
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }


        
        


    }
}
