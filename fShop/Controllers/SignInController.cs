using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using fShop.Models;
using fShop.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace fShop.Controllers
{
    public class SignInController : Controller
    {
        private readonly fShopContext _context;

        public SignInController(fShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
    {
        return RedirectToAction("Index", "Home"); 
    }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignInAsync(SignInViewModel model)
        {
            // Kiểm tra
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // Tìm số điện thoại
            var user = _context.Users
                .Where(u => u.PhoneNumber == model.PhoneNumber)
                .Select(u => new
                {
                    u.UserId,
                    u.FullName,
                    u.PhoneNumber,
                    u.PasswordHash,
                    u.Role,
                    u.Email,
                    u.Address
                })
                .FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("", "Số điện thoại không tồn tại.");
                return View("Index", model);
            }

            if (user.PasswordHash != model.Password)
            {
                ModelState.AddModelError("", "Mật khẩu không đúng.");
                return View("Index", model);
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.FullName ?? "Unknown"),
        new Claim(ClaimTypes.Role, user.Role ?? "User"),
        new Claim("PhoneNumber", user.PhoneNumber ?? "Unknown"),
        new Claim("Email", user.Email ?? "Unknown"),
        new Claim("Address", user.Address ?? "Unknown"),
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            HttpContext.Session.SetInt32("CustomerId", user.UserId);

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}