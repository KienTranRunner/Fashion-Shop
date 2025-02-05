using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace fShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductStyleController : Controller
    {
        private readonly fShopContext _context;

        public ProductStyleController(fShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Admin/Style/GetAll")]
        public IActionResult GetAll()
        {
            var productTypes = _context.Styles.Select(pt => new
            {
                StyleId = pt.StyleId,
                NameStyle = pt.NameStyle
            }).ToList();

            return Json(productTypes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Style/Create")]
        public IActionResult CreateStyle(Style model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Styles.Add(model);
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


        [Route("Admin/Style/Delete/{id}")]
        [HttpPost]
        public IActionResult DeleteStyle(int id)
        {
            try
            {
                var nt = _context.Styles.Find(id);
                if (nt == null)
                {
                    return Json(new { success = false, message = "Phong cách không tồn tại." });
                }

                bool hasRelatedEntries = _context.Products.Any(p => p.StyleId == id);

                if (hasRelatedEntries)
                {
                    return Json(new { success = false, message = "Không thể xóa danh mục vì đang có dữ liệu liên kết." });
                }

                _context.Styles.Remove(nt);
                _context.SaveChanges();

                return Json(new { success = true, message = "Xoá style thành công." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Đã xảy ra lỗi trong quá trình xoá" });
            }
        }


        [HttpGet]
        [Route("Admin/Style/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var productStyle = _context.Styles
            .Select(pc => new
            {
                StyleId = pc.StyleId,
                NameStyle = pc.NameStyle
            })
            .FirstOrDefault(pc => pc.StyleId == id);

            if (productStyle == null)
            {
                return Json(null);
            }
            return Json(productStyle);

        }

        [HttpPost]
        [Route("Admin/Style/Edit/{id}")]
        public IActionResult Edit(int id, [FromForm] Style model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            var productStyle = _context.Styles.FirstOrDefault(pc => pc.StyleId == id);
            if (productStyle == null)
            {
                return Json(new { success = false, message = "Phong cách không tồn tại." });
            }

            productStyle.NameStyle = model.NameStyle;

            try
            {
                _context.SaveChanges();
                return Json(new { success = true, message = "Cập nhật phong cách thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }




    }
}