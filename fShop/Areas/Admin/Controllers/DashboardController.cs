using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using fShop.Models;

namespace fShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly fShopContext _context;

        public DashboardController(fShopContext context)
        {
            _context = context;
        }

        // Hiển thị trang Dashboard
        public IActionResult Index()
        {
            return View();
        }

        // Lấy dữ liệu cho biểu đồ doanh thu theo tháng
        
        public async Task<JsonResult> GetRevenueData()
        {
            var revenueData = await _context.Orders
                .Where(o => o.Status == "Completed")
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Sum(o => o.OrderDetails.Sum(od => od.Quantity * od.Price))
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            return Json(revenueData);
        }

        // Lấy dữ liệu cho biểu đồ sản phẩm bán chạy
        public async Task<JsonResult> GetTopSellingProducts()
        {
            var topSellingProducts = await _context.OrderDetails
                .GroupBy(od => od.Product.NameProduct)
                .Select(g => new
                {
                    ProductName = g.Key,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(g => g.TotalQuantity)
                .Take(5)
                .ToListAsync();

            return Json(topSellingProducts);
        }

        // Lấy dữ liệu cho biểu đồ phân loại sản phẩm
        public async Task<JsonResult> GetProductCategoryData()
        {
            var productCategoryData = await _context.Products
                .GroupBy(p => p.Category.NameProductCategory)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalProducts = g.Count()
                })
                .ToListAsync();

            return Json(productCategoryData);
        }
        public JsonResult GetTotalUsers()
        {
            var totalUsers = _context.Users.Count();
            return Json(new { totalUsers });
        }
    }
}