using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class ProductMaterial
{
    public int ProductMaterialId { get; set; }

    public int ProductId { get; set; }

    public int MaterialId { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
