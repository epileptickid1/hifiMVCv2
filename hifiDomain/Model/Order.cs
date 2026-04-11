using System;
using System.Collections.Generic;

namespace hifiDomain.Model;

public partial class Order : Entity
{
    public DateOnly? Orderdate { get; set; }

    public decimal? Totalamount { get; set; }

    public int Customerid { get; set; }

    public int? Quantity { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Customorderoption> Customorderoptions { get; set; } = new List<Customorderoption>();

    public virtual ICollection<Headphone> Headphones { get; set; } = new List<Headphone>();
}
