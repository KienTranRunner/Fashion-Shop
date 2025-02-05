using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class Size
{
    public int SizeId { get; set; }

    public string NameSize { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
}
