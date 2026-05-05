using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using hifiDomain.Model;
using hifi_Infrastructure.Models;

namespace hifi_Infrastructure
{


    public class IdentityDBContext : IdentityDbContext<User>
    {
        public IdentityDBContext(DbContextOptions<IdentityDBContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Ignore<hifiDomain.Model.Order>();
            builder.Ignore<hifiDomain.Model.Headphone>();
            builder.Ignore<hifiDomain.Model.Internalpartsbrand>();
            builder.Ignore<hifiDomain.Model.Customorderoption>();
        }
    }
}