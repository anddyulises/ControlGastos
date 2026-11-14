namespace ControlGastos.Application.Dtos;

public class FacturaDto : DtoBase
{
    public string Descripcion { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public string Establecimiento { get; set; } = string.Empty;
    public string NumeroFactura { get; set; } = string.Empty;
    public int PresupuestoId { get; set; }
}
