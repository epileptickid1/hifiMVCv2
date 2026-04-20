using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hifiDomain.Model;

public partial class Internalpartsbrand : Entity
{
    [Display(Name = "Назва ")]
    public string Name { get; set; } = null!;
    [Display(Name = "Опис  ")]
    public string? Description { get; set; }
    [Display(Name = "Країна ")]
    public string? Country { get; set; }

    public virtual ICollection<Customorderoption> Customorderoptions { get; set; } = new List<Customorderoption>();

    public virtual ICollection<Headphone> Headphones { get; set; } = new List<Headphone>();
}
