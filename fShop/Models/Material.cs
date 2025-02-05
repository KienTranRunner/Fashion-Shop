using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class Material
{
    public int MaterialId { get; set; }

    public string NameMaterial { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
