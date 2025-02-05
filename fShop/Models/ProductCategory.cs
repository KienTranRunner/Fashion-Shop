using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class ProductCategory
{
    public int CategoryId { get; set; }

    public int ProductTypeId { get; set; }

    public string NameProductCategory { get; set; } = null!;

    public virtual ProductType ProductType { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
