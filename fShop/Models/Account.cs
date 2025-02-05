using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Password { get; set; } = null!;

    public int CustomerId { get; set; }

    public string Role { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
