using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace fShop.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public int SizeId { get; set; }

    public int ColorId { get; set; }

    public virtual Color Color { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Size Size { get; set; } = null!;

    [NotMapped]
    public string SizeName { get; set; }  
    
    [NotMapped]
    public string ColorName { get; set; }
}
