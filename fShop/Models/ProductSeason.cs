using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class ProductSeason
{
    public int ProductSeasonId { get; set; }

    public int ProductId { get; set; }

    public int SeasonId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Season Season { get; set; } = null!;
}
