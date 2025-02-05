using fShop.Models;
using fShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

public class CartController : Controller
{
    private readonly CartService _cartService;
    private readonly fShopContext _context;

    public CartController(CartService cartService, fShopContext context)
    {
        _cartService = cartService;
        _context = context;
    }

    public IActionResult Index()
    {
        var cart = _cartService.GetCart();
        return View(cart);
    }

    public IActionResult AddToCart(int id, string name, string imageUrl, decimal price, int quantity = 1, int sizeId = 0, int colorId = 0)
    {
        var sizeName = _context.Sizes.FirstOrDefault(s => s.SizeId == sizeId)?.NameSize ?? "N/A";
        var colorName = _context.Colors.FirstOrDefault(c => c.ColorId == colorId)?.NameColor ?? "N/A";

        var item = new CartItem
        {
            ProductId = id,
            ProductName = name,
            ImageUrl = imageUrl,
            Price = price,
            Quantity = quantity,
            SizeId = sizeId,
            SizeName = sizeName,
            ColorId = colorId,
            ColorName = colorName
        };

        _cartService.AddToCart(item);
        return RedirectToAction("Index");
    }


    public IActionResult RemoveFromCart(int id)
    {
        _cartService.RemoveFromCart(id);
        return RedirectToAction("Index");
    }

    public IActionResult ClearCart()
    {
        _cartService.ClearCart();
        return RedirectToAction("Index");
    }

    [Authorize]
    public IActionResult Checkout()
    {
        var cart = _cartService.GetCart();
        if (cart.Count == 0)
        {
            return RedirectToAction("Index");
        }
        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder()
    {
        // Kiểm tra nếu giỏ hàng trống
        var cart = _cartService.GetCart();
        if (cart.Count == 0)
        {
            return RedirectToAction("Index");
        }

        // Kiểm tra xem người dùng đã đăng nhập chưa
        if (!User.Identity.IsAuthenticated)
        {
            // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
            return RedirectToAction("Index", "SignIn");
        }

        // Lấy ID người dùng hiện tại từ claims
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            // Nếu userId vẫn null, có thể không tìm thấy Claim, chuyển hướng đến trang Aboutus
            return RedirectToAction("Index", "Aboutus");
        }

        var user = await _context.Users.FindAsync(int.Parse(userId));
        if (user == null)
        {
            return RedirectToAction("Index", "Contactus");
        }

        // Tạo đơn hàng mới
        var order = new Order
        {
            UserId = user.UserId,
            OrderDate = DateTime.Now,
            Status = "Pending" // Trạng thái mặc định khi đặt hàng
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(); // Lưu đơn hàng vào cơ sở dữ liệu

        // Lưu chi tiết đơn hàng
        foreach (var item in cart)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = order.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price,
                SizeId = item.SizeId,
                ColorId = item.ColorId,
            };
            _context.OrderDetails.Add(orderDetail);
        }

        await _context.SaveChangesAsync(); // Lưu chi tiết đơn hàng

        // Xóa giỏ hàng sau khi đặt hàng thành công
        _cartService.ClearCart();

        return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
    }
    public IActionResult OrderConfirmation(int orderId)
{
    var order = _context.Orders
        .Where(o => o.OrderId == orderId)
        .Select(o => new Order
        {
            OrderId = o.OrderId,
            OrderDate = o.OrderDate,
            OrderDetails = _context.OrderDetails
                .Where(od => od.OrderId == o.OrderId)
                .Select(od => new OrderDetail
                {
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    Price = od.Price,
                    SizeId = od.SizeId,
                    ColorId = od.ColorId,
                    SizeName = _context.Sizes
                        .Where(s => s.SizeId == od.SizeId)
                        .Select(s => s.NameSize)
                        .FirstOrDefault() ?? "N/A",
                    ColorName = _context.Colors
                        .Where(c => c.ColorId == od.ColorId)
                        .Select(c => c.NameColor)
                        .FirstOrDefault() ?? "N/A",
                    Product = _context.Products
                        .Where(p => p.ProductId == od.ProductId)
                        .Select(p => new Product
                        {
                            NameProduct = p.NameProduct
                        }).FirstOrDefault()
                }).ToList()
        }).FirstOrDefault();

    if (order == null)
    {
        return NotFound();
    }

    return View(order);
}






}
