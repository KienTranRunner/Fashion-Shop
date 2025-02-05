using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class Color
{
    public int ColorId { get; set; }

    public string NameColor { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
}
