using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hifiDomain.Model;


public partial class Headphone : Entity
{
    [Display(Name ="Назва ")]
    public string Name { get; set; } = null!;

    [Display(Name = "Опис ")]
    public string? Description { get; set; }

    [Display(Name = "Ціна, грн ")]
    public decimal? Price { get; set; }
    [Display(Name = "Матеріал ")]
    public string? Framematerial { get; set; }
    [Display(Name = "Вага, грамм")]
    public decimal? Weight { get; set; }

    public virtual ICollection<Internalpartsbrand> Internalpartsbrands { get; set; } = new List<Internalpartsbrand>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
