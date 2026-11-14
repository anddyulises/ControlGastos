namespace ControlGastos.Application.Dtos;

public class AddPresupuestoDto
{
    public int Mes { get; set; }
    public int Anio { get; set; }
    public decimal MontoTotal { get; set; }
}
