namespace ControlGastos.Infrastructure.Models;

public class PresupuestoModel
{
    public int Id { get; set; }
    public int Mes { get; set; }
    public int Anio { get; set; }
    public decimal MontoTotal { get; set; }

    // Calculados por el repositorio (solo lectura)
    public decimal TotalGastado { get; set; }
    public decimal PorcentajeUsado { get; set; }
    public string Estado { get; set; } = string.Empty;
}
