using EntityFrameworkCoreCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EntityFrameworkCoreCRUD.Service;
using EntityFrameworkCoreCRUD.Services;
using System.Text.Json;

public class OrderController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly OrderService orderService;
    private readonly OrderDetailService orderDetailService;

    public OrderController (OrderService orderService, OrderDetailService orderDetailService, IHttpContextAccessor httpContextAccessor)
    {
        this.orderService = orderService;
        this.orderDetailService = orderDetailService;

        _httpContextAccessor = httpContextAccessor;
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetCartItemsJson()
    {
        List<CartItem> cartItems = GetCartItems();
        return Json(cartItems);
    }
    public IActionResult ListCart()
    {
        return View();
    }
    private List<CartItem> GetCartItems()
    {
        var cartItemsJson = _httpContextAccessor.HttpContext.Session.GetString("CartItems");

        List<CartItem> cartItemList = new List<CartItem>();

        if (!string.IsNullOrEmpty(cartItemsJson))
        {
            cartItemList = JsonSerializer.Deserialize<List<CartItem>>(cartItemsJson);
        }

        return cartItemList;
    }
    public IActionResult ThanhToan(Order order)
    {
        // Kiểm tra xem người dùng đã đăng nhập chưa
        if (!HttpContext.Session.TryGetValue("UserId", out _))
        {
            TempData["ErrorMessage"] = "Bạn cần đăng nhập trước khi thực hiện thanh toán.";
            return RedirectToAction("Login", "User"); // Chuyển hướng đến trang đăng nhập
        }
        // Retrieve cart items
        List<CartItem> cartItems = GetCartItems();

        // Calculate Total manually if not set correctly
        foreach (var item in cartItems)
        {
            item.Total = item.Price * item.Quantity;
        }

        // Log cart items for debugging
        Console.WriteLine("Cart Items: ");
        foreach (var item in cartItems)
        {
            Console.WriteLine($"ItemId: {item.CartItemId}, Quantity: {item.Quantity}, Price: {item.Price}, Total: {item.Total}");
        }

        int buyerId = HttpContext.Session.GetInt32("UserId").Value;
        order.BuyerId = buyerId;

        decimal total = 0;
        foreach (var item in cartItems)
        {
            total += item.Total;
        }
        order.TotalAmount = total;

        // Add order and get the orderId
        int orderId = orderService.AddOrder(order); // Ensure this method is synchronous

        foreach (var item in cartItems)
        {
            OrderDetail orderDetail = new()
            {
                OrderId = orderId,
                Price = item.Price * item.Quantity,
                Quantity = item.Quantity,
                CarId = item.CartItemId
            };
            Console.WriteLine($"Adding OrderDetail: OrderId={orderDetail.OrderId}, Price={orderDetail.Price}, Quantity={orderDetail.Quantity}, CarId={orderDetail.CarId}");

            orderDetailService.AddOrderDetail(orderDetail);
        }

        // Clear cart items from session
        HttpContext.Session.Remove("CartItems");
        TempData["ResultOk"] = "Order Successfully!";

        return RedirectToAction("Create");
    }

}
