using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hifiDomain.Model;

public partial class Customer : Entity
{
    [Display(Name = "Ім'я ")]
    public string Name { get; set; } = null!;
    [Display(Name = "Пошта ")]
    public string? Email { get; set; }
    [Display(Name = "Телефон ")]
    [Required(ErrorMessage = "формат номеру <<+123 45 678 9000>>")]
    public string Phone { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
