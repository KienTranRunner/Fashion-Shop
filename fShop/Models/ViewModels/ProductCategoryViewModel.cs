using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace fShop.Models.ViewModels
{
    public class ProductCategoryViewModel
    {
       

        [Required(ErrorMessage = "Tên danh mục thể loại là bắt buộc.")]
        public string NameProductCategory { get; set; } = null!;

        [Required(ErrorMessage = "Nhóm danh mục là bắt buộc.")]
        public int ProductTypeId { get; set; }

    }
}