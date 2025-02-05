using fShop.Models;
using fShop.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace fShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductManagementController : Controller
    {
        private readonly fShopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductManagementController(fShopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.ProductCategory = _context.ProductCategories.ToList();
            ViewBag.Style = _context.Styles.ToList();
            ViewBag.TargetAudience = _context.TargetAudiences.ToList();
            ViewBag.Materials = _context.Materials.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Seasons = _context.Seasons.ToList();

            return View();
        }

        [HttpGet]
        [Route("Admin/Product/GetAll")]
        public IActionResult GetAll()
        {
            var listProductCategory = _context.Products.ToList();
            return Json(listProductCategory);
        }

        [HttpPost]
        [Route("Admin/Product/Create")]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Dữ liệu không hợp lệ", errors });
            }

            var product = new Product
            {
                NameProduct = model.NameProduct,
                DescriptionProduct = model.DescriptionProduct,
                CategoryId = model.CategoryId,
                StyleId = model.StyleId,
                AudienceId = model.AudienceId,
                MaterialId = model.MaterialId,
                Price = model.Price
            };

            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                product.ImageUrl = "~/images/" + uniqueFileName;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            if (model.Sizes != null)
            {
                foreach (var sizeId in model.Sizes)
                {
                    _context.ProductSizes.Add(new ProductSize
                    {
                        ProductId = product.ProductId,
                        SizeId = sizeId
                    });
                }
            }

            if (model.Colors != null)
            {
                foreach (var colorId in model.Colors)
                {
                    _context.ProductColors.Add(new ProductColor
                    {
                        ProductId = product.ProductId,
                        ColorId = colorId
                    });
                }
            }

            if (model.Seasons != null)
            {
                foreach (var seasonId in model.Seasons)
                {
                    _context.ProductSeasons.Add(new ProductSeason
                    {
                        ProductId = product.ProductId,
                        SeasonId = seasonId
                    });
                }
            }

            _context.SaveChanges();


            return Json(new { success = true });
        }

        [Route("Admin/Product/Delete/{id}")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var product = _context.Products.Find(id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                }

                bool hasRelatedEntries = _context.OrderDetails.Any(p => p.ProductId == id) ||
                                         _context.ProductColors.Any(pc => pc.ProductId == id) ||
                                         _context.ProductSizes.Any(ps => ps.ProductId == id);

                if (hasRelatedEntries)
                {
                    _context.ProductColors.RemoveRange(_context.ProductColors.Where(pc => pc.ProductId == id));
                    _context.ProductSizes.RemoveRange(_context.ProductSizes.Where(ps => ps.ProductId == id));
                    _context.SaveChanges();
                }

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('~', '/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Products.Remove(product);
                _context.SaveChanges();

                return Json(new { success = true, message = "Xóa sản phẩm thành công." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Đã xảy ra lỗi trong quá trình xoá." });
            }
        }

        [HttpGet]
        [Route("Admin/Product/GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var product = _context.Products
                .Where(p => p.ProductId == id)
                .Select(p => new
                {
                    productId = p.ProductId,
                    nameProduct = p.NameProduct,
                    descriptionProduct = p.DescriptionProduct,
                    categoryId = p.CategoryId,
                    styleId = p.StyleId,
                    audienceId = p.AudienceId,
                    materialId = p.MaterialId,
                    price = p.Price,
                    imageUrl = p.ImageUrl,
                    selectedSizes = _context.ProductSizes
                                       .Where(ps => ps.ProductId == p.ProductId)
                                       .Select(ps => ps.SizeId)
                                       .ToList(),
                    selectedColors = _context.ProductColors
                                       .Where(pc => pc.ProductId == p.ProductId)
                                       .Select(pc => pc.ColorId)
                                       .ToList(),
                    selectedSeasons = _context.ProductSeasons
                                       .Where(ps => ps.ProductId == p.ProductId)
                                       .Select(ps => ps.SeasonId)
                                       .ToList()
                })
                .FirstOrDefault();

            if (product == null)
            {
                return Json(null);
            }

            return Json(product);
        }

        [HttpPost]
        [Route("Admin/Product/Update")]
        public async Task<IActionResult> Edit(ProductEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Dữ liệu không hợp lệ", errors });
            }

            var product = await _context.Products.FindAsync(model.ProductId);
            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại." });
            }

            product.NameProduct = model.NameProduct;
            product.DescriptionProduct = model.DescriptionProduct;
            product.CategoryId = model.CategoryId;
            product.StyleId = model.StyleId;
            product.AudienceId = model.AudienceId;
            product.MaterialId = model.MaterialId;
            product.Price = model.Price;



            // Update related sizes
            var existingSizes = _context.ProductSizes.Where(ps => ps.ProductId == product.ProductId);
            _context.ProductSizes.RemoveRange(existingSizes);

            if (model.Sizes != null)
            {
                foreach (var sizeId in model.Sizes)
                {
                    _context.ProductSizes.Add(new ProductSize
                    {
                        ProductId = product.ProductId,
                        SizeId = sizeId
                    });
                }
            }

            // Update related colors
            var existingColors = _context.ProductColors.Where(pc => pc.ProductId == product.ProductId);
            _context.ProductColors.RemoveRange(existingColors);

            if (model.Colors != null)
            {
                foreach (var colorId in model.Colors)
                {
                    _context.ProductColors.Add(new ProductColor
                    {
                        ProductId = product.ProductId,
                        ColorId = colorId
                    });
                }
            }

            var existingSeasons = _context.ProductSeasons.Where(ps => ps.ProductId == product.ProductId);
            _context.ProductSeasons.RemoveRange(existingSeasons);

            if (model.Seasons != null)
            {
                foreach (var seasonId in model.Seasons)
                {
                    _context.ProductSeasons.Add(new ProductSeason
                    {
                        ProductId = product.ProductId,
                        SeasonId = seasonId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Cập nhật sản phẩm thành công." });
        }

        [HttpPost]
        [Route("Admin/Product/Approve")]
        public async Task<IActionResult> Approve([FromBody] ApproveRequest request)
        {
            if (request == null || request.ProductId <= 0)
            {
                return BadRequest(new { success = false, message = "Thông tin không hợp lệ." });
            }

            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy sản phẩm." });
            }

            product.IsApproved = request.IsApproved;

            try
            {
                await _context.SaveChangesAsync();
                var status = product.IsApproved ? "đã duyệt" : "chưa duyệt";
                return Ok(new { success = true, message = $"Sản phẩm đã được cập nhật trạng thái: {status}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi cập nhật trạng thái.", error = ex.Message });
            }
        }








    }
}
