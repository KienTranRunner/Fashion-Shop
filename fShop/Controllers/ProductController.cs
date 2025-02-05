using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using fShop.Models;

namespace fShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly fShopContext _context;
        public ProductController(fShopContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, int priceRange = 1000000, string[] sizes = null!, string[] colors = null!, string search = "")
        {
            int pageSize = 9;

            // Lọc sản phẩm theo giá và trạng thái
            var products = _context.Products
                   .Where(p => p.IsApproved == true 
                               && p.Price <= priceRange 
                               && (string.IsNullOrEmpty(search) || p.NameProduct.Contains(search)))
                   .OrderByDescending(p => p.ProductId);

            // Lọc sản phẩm theo size nếu có size được chọn
            if (sizes != null && sizes.Length > 0)
            {
                var sizeIds = _context.Sizes
                                      .Where(s => sizes.Contains(s.NameSize))
                                      .Select(s => s.SizeId)
                                      .ToList();

                products = (IOrderedQueryable<Product>)products
                           .Where(p => _context.ProductSizes
                                              .Where(ps => sizeIds.Contains(ps.SizeId))
                                              .Select(ps => ps.ProductId)
                                              .Contains(p.ProductId));
            }
            if (colors != null && colors.Length > 0)
            {
                var colorIds = _context.Colors
                                       .Where(c => colors.Contains(c.NameColor))
                                       .Select(c => c.ColorId)
                                       .ToList();

                products = (IOrderedQueryable<Product>)products
                           .Where(p => _context.ProductColors
                                              .Where(pc => colorIds.Contains(pc.ColorId))
                                              .Select(pc => pc.ProductId)
                                              .Contains(p.ProductId));
            }
                ViewBag.Colors = _context.Colors.ToList();



            var pagedProducts = products
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            int totalProducts = products.Count();
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SelectedPriceRange = priceRange;
            ViewBag.SelectedSizes = sizes;
            ViewBag.SelectedColors = colors;
            ViewBag.SearchKeyword = search; 




            return View(pagedProducts);
        }


        public IActionResult Detail(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Colors = _context.ProductColors
                    .Where(pc => pc.ProductId == id)
                    .Join(_context.Colors,
                          pc => pc.ColorId,
                          c => c.ColorId,
                          (pc, c) => new { c.ColorId, c.NameColor })
                    .ToList();

            ViewBag.Sizes = _context.ProductSizes
                .Where(ps => ps.ProductId == id)
                .Join(_context.Sizes,
                      ps => ps.SizeId,
                      s => s.SizeId,
                      (ps, s) => new { s.SizeId, s.NameSize, ps.Quantity })
                .ToList();
            return View(product);
        }




    }
}