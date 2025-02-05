using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class Season
{
    public int SeasonId { get; set; }

    public string NameSeason { get; set; } = null!;

    public virtual ICollection<ProductSeason> ProductSeasons { get; set; } = new List<ProductSeason>();
}
