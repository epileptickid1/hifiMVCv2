using System;
using System.Collections.Generic;

namespace hifiDomain.Model;

public partial class Headphone : Entity
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? Framematerial { get; set; }

    public decimal? Weight { get; set; }

    public virtual ICollection<Internalpartsbrand> Internalpartsbrands { get; set; } = new List<Internalpartsbrand>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
