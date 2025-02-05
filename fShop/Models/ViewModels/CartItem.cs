using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fShop.Models.ViewModels
{
    public class CartItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int SizeId { get; set; }
    public int ColorId { get; set; }
    public string SizeName { get; set; }
    public string ColorName { get; set; }
    public decimal Total => Price * Quantity;
}

}