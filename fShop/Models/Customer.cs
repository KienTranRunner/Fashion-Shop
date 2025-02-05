using System;
using System.Collections.Generic;

namespace fShop.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string NameCustomer { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
