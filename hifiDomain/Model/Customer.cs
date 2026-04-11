using System;
using System.Collections.Generic;

namespace hifiDomain.Model;

public partial class Customer : Entity
{
    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
