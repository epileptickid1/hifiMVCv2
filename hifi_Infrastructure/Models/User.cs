using hifiDomain.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace hifi_Infrastructure.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }


        [NotMapped]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}