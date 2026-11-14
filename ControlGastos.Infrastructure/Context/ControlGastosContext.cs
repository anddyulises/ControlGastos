using Microsoft.EntityFrameworkCore;
using ControlGastos.Domain.Entities;

namespace ControlGastos.Infrastructure.Context;

public class ControlGastosContext : DbContext
{
    public ControlGastosContext(DbContextOptions<ControlGastosContext> options) : base(options) { }

    public DbSet<Presupuesto> Presupuestos { get; set; }
    public DbSet<Factura> Facturas { get; set; }
    public DbSet<Propina> Propinas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Presupuesto>(e =>
        {
            e.ToTable("Presupuestos");
            e.HasMany(p => p.Facturas).WithOne(f => f.Presupuesto)
             .HasForeignKey(f => f.PresupuestoId);
            e.HasMany(p => p.Propinas).WithOne(x => x.Presupuesto)
             .HasForeignKey(x => x.PresupuestoId);
        });

        modelBuilder.Entity<Factura>(e =>
        {
            e.ToTable("Facturas");
            e.Property(f => f.Descripcion).IsRequired().HasMaxLength(200);
            e.Property(f => f.Establecimiento).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Propina>(e =>
        {
            e.ToTable("Propinas");
            e.Property(p => p.Descripcion).IsRequired().HasMaxLength(200);
            e.Property(p => p.Beneficiario).HasMaxLength(150);
        });
    }
}
