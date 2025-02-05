using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace fShop.Models.ViewModels
{
    public class ProductEditViewModel
    {
        public int ProductId { get; set; }

        [Required]
        public string NameProduct { get; set; }

        [Required]
        public string DescriptionProduct { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int StyleId { get; set; }

        [Required]
        public int AudienceId { get; set; }

        [Required]
        public int MaterialId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid price")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }

        public List<int> Sizes { get; set; }

        public List<int> Colors { get; set; }

        public List<int> Seasons { get; set; }
    
    }
}