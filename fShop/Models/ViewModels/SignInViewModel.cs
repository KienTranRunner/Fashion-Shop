using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace fShop.Models.ViewModels
{
    public class SignInViewModel
    {

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 số.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự.")]
        public string Password { get; set; } = null!;
    }
}