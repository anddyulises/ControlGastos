namespace ControlGastos.Application.Dtos;

public class AddPropinaDto
{
    public string Descripcion { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
    public string Beneficiario { get; set; } = string.Empty;
    public int PresupuestoId { get; set; }
}
