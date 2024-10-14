using EntityFrameworkCoreCRUD.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class OrderDetailController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderDetailController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(int page = 1)
    {
        const int PageSize = 5; // Number of items per page

        if (HttpContext.Session.GetString("Username") != null)
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = username;

            var totalOrderDetails = _context.OrderDetails.Count();
            var totalPages = (int)Math.Ceiling(totalOrderDetails / (double)PageSize);
            var orderDetails = _context.OrderDetails
                                        .OrderBy(od => od.OrderDetailId) // Optional: Order by some property
                                        .Skip((page - 1) * PageSize)
                                        .Take(PageSize)
                                        .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(orderDetails);
        }
        else
        {
            return RedirectToAction("Login", "User");
        }
    }
    public IActionResult Delete(int id)
    {
        // Check if user is logged in
        if (HttpContext.Session.GetString("Username") != null)
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = username;
        }

        // Find the order detail by id
        var orderDetail = _context.OrderDetails.Find(id);
        if (orderDetail == null)
        {
            return NotFound();
        }

        // Remove the order detail from the context
        _context.OrderDetails.Remove(orderDetail);

        // Save changes to the database
        _context.SaveChanges();
        TempData["ResultOk"] = "Delete successfully!";

        // Redirect to the desired view after deletion
        return RedirectToAction("Index"); // Adjust "Index" to your target view or action method
    }



}
