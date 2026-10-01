using Microsoft.EntityFrameworkCore;
using ProGear.Api.Models;

namespace ProGear.Api.Data;

public class ProGearDbContext : DbContext
{
    public ProGearDbContext(DbContextOptions<ProGearDbContext> options) : base(options)
    {
    }

    public DbSet<Producto> Productos { get; set; }
    
    public DbSet<Inventario> Inventarios { get;set; }

    public DbSet<HistorialPrecio> HistorialPrecios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Producto");

            entity.HasKey(p => p.Id);

            entity.HasIndex(p => p.Sku)
            .IsUnique();

            entity.Property(p => p.Nombre)
            .HasMaxLength(200)
            .IsRequired();

            entity.Property(p => p.Marca)
            .HasMaxLength(20)
            .IsRequired();

            entity.Property(p => p.Sku)
            .HasMaxLength(7)
            .IsRequired();
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
           entity.ToTable("Inventario");

           entity.HasKey(i => i.Id);

           entity.HasIndex(i => i.IdProducto)
           .IsUnique();

           entity.HasOne<Producto>()
              .WithOne()
                .HasForeignKey<Inventario>(i => i.IdProducto); 
        });

        modelBuilder.Entity<HistorialPrecio>(entity =>
        {
           entity.ToTable("HistorialPrecio");

           entity.HasKey(h => h.Id);

           entity.Property(h => h.Precio)
                .HasPrecision(10, 2);

            entity.HasOne<Producto>()
                .WithMany()
                .HasForeignKey(h => h.IdProducto); 
        });
    }
}


