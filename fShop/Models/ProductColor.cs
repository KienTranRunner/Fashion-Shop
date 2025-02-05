using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class ProductColor
{
    public int ProductColorId { get; set; }

    public int ProductId { get; set; }

    public int ColorId { get; set; }

    public virtual Color Color { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
