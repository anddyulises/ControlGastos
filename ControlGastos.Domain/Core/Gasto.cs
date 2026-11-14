namespace ControlGastos.Domain.Core;

// Base común de Factura y Propina (igual que Person en School).
public abstract class Gasto : BaseEntity
{
    public string Descripcion { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public int PresupuestoId { get; set; }
}
