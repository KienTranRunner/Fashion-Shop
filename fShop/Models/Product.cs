using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public int StyleId { get; set; }

    public int AudienceId { get; set; }

    public int MaterialId { get; set; }

    public string NameProduct { get; set; } = null!;

    public string? DescriptionProduct { get; set; }

    public string? ImageUrl { get; set; }

    public decimal Price { get; set; }

    public bool IsApproved { get; set; }

    public virtual TargetAudience Audience { get; set; } = null!;

    public virtual ProductCategory Category { get; set; } = null!;

    public virtual Material Material { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();

    public virtual ICollection<ProductSeason> ProductSeasons { get; set; } = new List<ProductSeason>();

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();

    public virtual Style Style { get; set; } = null!;
}
