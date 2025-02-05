using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class TargetAudience
{
    public int AudienceId { get; set; }

    public string NameTargetAudience { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
