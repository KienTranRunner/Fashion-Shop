using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fShop.Models;
using fShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace fShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly fShopContext _context;

        public ProductCategoryController(fShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.ProductTypeSelect = _context.ProductTypes.ToList();

            return View();
        }


        [HttpGet]
        [Route("Admin/ProductCategory/GetAll")]
        public IActionResult GetAll()
        {
            var listProductCategory = _context.ProductCategories
                .Include(pc => pc.ProductType)
                .Select(pc => new
                {
                    CategoryId = pc.CategoryId,
                    Name = pc.NameProductCategory,
                    ProductTypeId = pc.ProductTypeId,
                    ProductTypeName = pc.ProductType.NameProductType,
                });
            return Json(listProductCategory);
        }




        [HttpPost]
        [Route("Admin/ProductCategory/Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductCategory model)
        {
            if (!ModelState.IsValid)
            {
                _context.ProductCategories.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);


        }

        [Route("Admin/ProductCategory/Delete/{id}")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var pc = _context.ProductCategories.Find(id);
                if (pc == null)
                {
                    return Json(new { success = false, message = "Danh mục thể loại không tồn tại." });
                }

                bool hasRelatedEntries = _context.Products.Any(p => p.CategoryId == id);

                if (hasRelatedEntries)
                {
                    return Json(new { success = false, message = "Không thể xóa danh mục vì đang có dữ liệu liên kết." });
                }

                _context.ProductCategories.Remove(pc);
                _context.SaveChanges();

                return Json(new { success = true, message = "Xoá danh mục thể loại thành công." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Đã xảy ra lỗi trong quá trình xoá " });
            }
        }


        [HttpGet]
        [Route("Admin/ProductCategory/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var productCategory = _context.ProductCategories
            .Select(pc => new
            {
                categoryId = pc.CategoryId,
                nameProductCategory = pc.NameProductCategory,
                productTypeId = pc.ProductTypeId
            })
            .FirstOrDefault(pc => pc.categoryId == id);

            if (productCategory == null)
            {
                return Json(null);
            }
            return Json(productCategory);

        }
        
        [HttpPost]
        [Route("Admin/ProductCategory/Edit/{id}")]
        public IActionResult Edit(int id, [FromForm] ProductCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            var productCategory = _context.ProductCategories.FirstOrDefault(pc => pc.CategoryId == id);
            if (productCategory == null)
            {
                return Json(new { success = false, message = "Danh mục không tồn tại." });
            }

            productCategory.NameProductCategory = model.NameProductCategory;
            productCategory.ProductTypeId = model.ProductTypeId;

            try
            {
                _context.SaveChanges(); 
                return Json(new { success = true, message = "Cập nhật danh mục thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }






    }
}