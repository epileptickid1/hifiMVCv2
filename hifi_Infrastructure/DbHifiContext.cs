using System;
using System.Collections.Generic;
using hifiDomain.Model;
using Microsoft.EntityFrameworkCore;

namespace hifi_Infrastructure;

public partial class DbHifiContext : DbContext
{
    public DbHifiContext()
    {
    }

    public DbHifiContext(DbContextOptions<DbHifiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customorderoption> Customorderoptions { get; set; }

    public virtual DbSet<Headphone> Headphones { get; set; }

    public virtual DbSet<Internalpartsbrand> Internalpartsbrands { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=labdb;Username=vlad;Password=abcdfg;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_pkey");

            entity.ToTable("customer");

            entity.HasIndex(e => e.Email, "customer_email_key").IsUnique();

            entity.HasIndex(e => e.Phone, "customer_phone_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Customorderoption>(entity =>
        {
            entity.HasKey(e => new { e.Ordersid, e.Internalpartsbrandid }).HasName("customorderoption_pkey");

            entity.ToTable("customorderoption");

            entity.Property(e => e.Ordersid).HasColumnName("ordersid");
            entity.Property(e => e.Internalpartsbrandid).HasColumnName("internalpartsbrandid");
            entity.Property(e => e.Customshell)
                .HasMaxLength(50)
                .HasColumnName("customshell");
            entity.Property(e => e.Wire)
                .HasMaxLength(50)
                .HasColumnName("wire");

            entity.HasOne(d => d.Internalpartsbrand).WithMany(p => p.Customorderoptions)
                .HasForeignKey(d => d.Internalpartsbrandid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customorderoption_internalpartsbrandid_fkey");

            entity.HasOne(d => d.Orders).WithMany(p => p.Customorderoptions)
                .HasForeignKey(d => d.Ordersid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customorderoption_ordersid_fkey");
        });

        modelBuilder.Entity<Headphone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("headphone_pkey");

            entity.ToTable("headphone");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Framematerial)
                .HasMaxLength(50)
                .HasColumnName("framematerial");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(12, 2)
                .HasColumnName("price");
            entity.Property(e => e.Weight)
                .HasPrecision(12, 2)
                .HasColumnName("weight");
        });

        modelBuilder.Entity<Internalpartsbrand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("internalpartsbrand_pkey");

            entity.ToTable("internalpartsbrand");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Country)
                .HasMaxLength(30)
                .HasColumnName("country");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");

            entity.HasMany(d => d.Headphones).WithMany(p => p.Internalpartsbrands)
                .UsingEntity<Dictionary<string, object>>(
                    "Headphonecomponent",
                    r => r.HasOne<Headphone>().WithMany()
                        .HasForeignKey("Headphoneid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("headphonecomponent_headphoneid_fkey"),
                    l => l.HasOne<Internalpartsbrand>().WithMany()
                        .HasForeignKey("Internalpartsbrandid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("headphonecomponent_internalpartsbrandid_fkey"),
                    j =>
                    {
                        j.HasKey("Internalpartsbrandid", "Headphoneid").HasName("headphonecomponent_pkey");
                        j.ToTable("headphonecomponent");
                        j.IndexerProperty<int>("Internalpartsbrandid").HasColumnName("internalpartsbrandid");
                        j.IndexerProperty<int>("Headphoneid").HasColumnName("headphoneid");
                    });
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Orderdate).HasColumnName("orderdate");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Totalamount).HasColumnName("totalamount");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_customerid_fkey");

            entity.HasMany(d => d.Headphones).WithMany(p => p.Orders)
                .UsingEntity<Dictionary<string, object>>(
                    "Orderitem",
                    r => r.HasOne<Headphone>().WithMany()
                        .HasForeignKey("Headphoneid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("orderitem_headphoneid_fkey"),
                    l => l.HasOne<Order>().WithMany()
                        .HasForeignKey("Ordersid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("orderitem_ordersid_fkey"),
                    j =>
                    {
                        j.HasKey("Ordersid", "Headphoneid").HasName("orderitem_pkey");
                        j.ToTable("orderitem");
                        j.IndexerProperty<int>("Ordersid").HasColumnName("ordersid");
                        j.IndexerProperty<int>("Headphoneid").HasColumnName("headphoneid");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
