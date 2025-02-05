using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fShop.Models;
using fShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypeController : Controller
    {
        private readonly fShopContext _context;

        public ProductTypeController(fShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Admin/ProductType/GetAll")]
        public IActionResult GetAll()
        {
            var productTypes = _context.ProductTypes.Select(pt => new
            {
                ProductTypeID = pt.ProductTypeId,
                Name = pt.NameProductType
            }).ToList();

            return Json(productTypes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/ProductType/Create")]
        public IActionResult CreateProductType(ProductType model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.ProductTypes.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("Index");

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Index");

        }

        [Route("Admin/ProductType/Delete/{id}")]
        [HttpPost]
        public IActionResult DeleteProductType(int id)
        {
            try
            {
                var nt = _context.ProductTypes.Find(id);
                if (nt == null)
                {
                    return Json(new { success = false, message = "Danh mục không tồn tại." });
                }

                bool hasRelatedEntries = _context.ProductCategories.Any(p => p.ProductTypeId == id);

                if (hasRelatedEntries)
                {
                    return Json(new { success = false, message = "Không thể xóa danh mục vì đang có dữ liệu liên kết." });
                }

                _context.ProductTypes.Remove(nt);
                _context.SaveChanges();

                return Json(new { success = true, message = "Xoá danh mục thành công." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Đã xảy ra lỗi trong quá trình xoá" });
            }
        }
        [HttpGet]
        [Route("Admin/ProductType/GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var productType = _context.ProductTypes
                .Select(pt => new
                {
                    ProductTypeID = pt.ProductTypeId,
                    NameProductType = pt.NameProductType
                })
                .FirstOrDefault(pt => pt.ProductTypeID == id);

            if (productType == null)
            {
                return NotFound(new { message = "Danh mục không tồn tại." });
            }

            return Json(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/ProductType/Edit/{id}")]
        public IActionResult EditProductType(int id, ProductType model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingProductType = _context.ProductTypes.Find(id);
                    if (existingProductType == null)
                    {
                        return Json(new { success = false, message = "Danh mục không tồn tại." });
                    }

                    existingProductType.NameProductType = model.NameProductType;
                    _context.SaveChanges();

                    return Json(new { success = true, message = "Cập nhật danh mục thành công!" });
                }
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Đã xảy ra lỗi trong quá trình cập nhật." });
            }
        }





    }
}