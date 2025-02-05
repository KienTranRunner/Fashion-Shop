using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace fShop.Models.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        public string NameProduct { get; set; } = null!;

        [Required(ErrorMessage = "Mô tả sản phẩm là bắt buộc.")]
        public string DescriptionProduct { get; set; } = null!;

        [Required(ErrorMessage = "Danh mục là bắt buộc.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Phong cách là bắt buộc.")]
        public int StyleId { get; set; }

        [Required(ErrorMessage = "Đối tượng là bắt buộc.")]
        public int AudienceId { get; set; }

        [Required(ErrorMessage = "Chất liệu là bắt buộc.")]
        public int MaterialId { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        public decimal Price { get; set; }

        public IFormFile? ImageFile { get; set; }


        public List<int>? Sizes { get; set; }
        public List<int>? Colors { get; set; }
        public List<int>? Seasons { get; set; }
    }
}