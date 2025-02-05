using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using fShop.Models;

namespace fShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly fShopContext _context;

    public HomeController(ILogger<HomeController> logger,  fShopContext fShopContext)
    {
        _logger = logger;
        _context = fShopContext;
    }

    public IActionResult Index()
    {
        var NewProducts = _context.Products
                       .Where(p => p.IsApproved == true)
                       .OrderByDescending(p => p.ProductId);
        

        var WomenProducts = _context.Products.Where(p => p.Audience.NameTargetAudience.Equals("Nữ")).OrderByDescending(p=> p.ProductId);
        var MenProducts = _context.Products.Where(p => p.Audience.NameTargetAudience.Equals("Nam")).OrderByDescending(p=> p.ProductId);


        ViewBag.NewProduct = NewProducts.ToList().Take(8);
        ViewBag.WomenProducts = WomenProducts.ToList().Take(3);
        ViewBag.MenProducts = MenProducts.ToList().Take(3);


        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
