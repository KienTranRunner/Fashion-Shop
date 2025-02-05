using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly fShopContext _context;

        public CustomerController(fShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Admin/Customer/GetAll")]
        public IActionResult GetAll()
        {
            var listUserCustomer = _context.Users
             .Where(u => u.Role == "Customer") 
             .Select(u => new
             {
                 UserId = u.UserId,
                 FullName = u.FullName,
                 Email = u.Email,
                 PhoneNumber = u.PhoneNumber,
                 Address = u.Address,
                 Role = u.Role,
                 TongTien = _context.OrderDetails
                .Where(od => od.Order.UserId == u.UserId)
                .Sum(od => od.Quantity * od.Price)
             })
             .ToList();

            return Json(listUserCustomer);
        }

    }
}