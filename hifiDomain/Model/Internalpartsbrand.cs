using System;
using System.Collections.Generic;

namespace hifiDomain.Model;

public partial class Internalpartsbrand : Entity
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<Customorderoption> Customorderoptions { get; set; } = new List<Customorderoption>();

    public virtual ICollection<Headphone> Headphones { get; set; } = new List<Headphone>();
}
