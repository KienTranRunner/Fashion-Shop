using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fShop.Models;
using fShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace fShop.Controllers
{
    public class SignUpController : Controller
    {
        private readonly fShopContext _context;

        public SignUpController(fShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(SignUpViewModel model)
        {
            // Kiểm tra dữ liệu nhập
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra mật khẩu và xác nhận mật khẩu
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Mật khẩu và xác nhận mật khẩu không khớp.");
                return View(model);
            }

            // kiểm tra có trùng tk hay không
            var existEmail = _context.Users.FirstOrDefault(x => x.Email == model.Email);
            var existPhone = _context.Users.FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber);

            if (existEmail != null )
            {
                ModelState.AddModelError("", "Tên tài khoản hoặc email đã được sử dụng.");
                return View(model);
            }
             if (existPhone != null )
            {
                ModelState.AddModelError("", "Số điện thoại đã được sử dụng.");
                return View(model);
            }

            // Tạo kh mới
            var newUserCustomer = new User
            {
                FullName = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                PasswordHash = model.Password,
                Role = "Customer"

            };

             _context.Users.Add(newUserCustomer);
            _context.SaveChanges();


            return RedirectToAction("Index", "SignIn");


        }

    }
}