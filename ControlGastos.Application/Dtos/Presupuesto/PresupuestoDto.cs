namespace ControlGastos.Application.Dtos;

public class PresupuestoDto : DtoBase
{
    public int Mes { get; set; }
    public int Anio { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal TotalGastado { get; set; }
    public decimal PorcentajeUsado { get; set; }
    public string Estado { get; set; } = string.Empty;
}
