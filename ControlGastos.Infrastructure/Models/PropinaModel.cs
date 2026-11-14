namespace ControlGastos.Infrastructure.Models;

public class PropinaModel
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public string Beneficiario { get; set; } = string.Empty;
    public int PresupuestoId { get; set; }
}
