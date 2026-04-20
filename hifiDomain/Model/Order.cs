using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hifiDomain.Model;

public partial class Order : Entity
{
    [Display(Name = "Дата замовлення ")]
    public DateOnly? Orderdate { get; set; }
    [Display(Name = "Всього ")]
    public decimal? Totalamount { get; set; }
    [Display(Name = "Покупець ")]
    public int Customerid { get; set; }
    [Display(Name = "Кількість ")]
    public int? Quantity { get; set; }

    public virtual Customer Customer { get; set; } = null!;
    
    public virtual ICollection<Customorderoption> Customorderoptions { get; set; } = new List<Customorderoption>();
    
    public virtual ICollection<Headphone> Headphones { get; set; } = new List<Headphone>();
}
