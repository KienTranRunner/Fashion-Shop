using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class ProductType
{
    public int ProductTypeId { get; set; }

    public string NameProductType { get; set; } = null!;

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
