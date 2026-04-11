using System;
using System.Collections.Generic;

namespace hifiDomain.Model;

public partial class Customorderoption
{
    public int Ordersid { get; set; }

    public int Internalpartsbrandid { get; set; }

    public string? Customshell { get; set; }

    public string? Wire { get; set; }

    public virtual Internalpartsbrand Internalpartsbrand { get; set; } = null!;

    public virtual Order Orders { get; set; } = null!;
}
