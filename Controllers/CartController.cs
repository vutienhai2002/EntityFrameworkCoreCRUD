using EntityFrameworkCoreCRUD.Models;
using EntityFrameworkCoreCRUD.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class CartController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CartController(IHttpContextAccessor httpContextAccessor) { 
    
            _httpContextAccessor = httpContextAccessor;
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
    private void SaveCartItems(List<CartItem> cartItems)
    {
        var serializedCartItems = JsonSerializer.Serialize(cartItems);
        _httpContextAccessor.HttpContext.Session.SetString("CartItems", serializedCartItems);
    }
    [HttpPost]
    public IActionResult Addtocart(int CarId, string Manufacturer, Decimal Price, int quantitys, string Img) {

        List<CartItem> cartItems = GetCartItems();
        var existingItem = cartItems.FirstOrDefault(item => item.CartItemId == CarId);

        if (existingItem != null)
        {
            int previousQuantity = existingItem.Quantity;

            existingItem.Quantity += quantitys;

            if (previousQuantity != existingItem.Quantity)
            {
                existingItem.Price = existingItem.Quantity * Price;
            }
        }
        else
        {
            cartItems.Add(new CartItem { CartItemId = CarId, ProductName = Manufacturer, Price = quantitys * Price, Quantity = quantitys, Img = Img });
        }
        SaveCartItems(cartItems);
            TempData["ResultOk"] = "Cart Successfully!";
                 return RedirectToAction("Car", "viewcar");

/*        return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng thành công!" });
*/    }
    public IActionResult xoa()
    {
        _httpContextAccessor.HttpContext.Session.Remove("CartItems");
        TempData["ResultOk"] = "Delete successfully!";

        return RedirectToAction("listcart", "Cart");

    }
    public IActionResult DeleteQuantity(int id)
    {
        List<CartItem> cartItems = GetCartItems();
        var existingItem = cartItems.FirstOrDefault(item => item.CartItemId == id);

        if (existingItem != null)
        {
            existingItem.Price = existingItem.Price / existingItem.Quantity;

            existingItem.Quantity -= 1;

            if (existingItem.Quantity <= 0)
            {
                cartItems.Remove(existingItem);
            }
            else
            {
                existingItem.Price = existingItem.Price * existingItem.Quantity;
            }
        }

        SaveCartItems(cartItems);
        return RedirectToAction("listcart", "Cart");
    }

}
