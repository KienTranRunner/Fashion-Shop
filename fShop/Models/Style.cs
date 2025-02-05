using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class Style
{
    public int StyleId { get; set; }

    public string NameStyle { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
