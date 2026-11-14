using ControlGastos.Domain.Core;

namespace ControlGastos.Domain.Entities;

public class Presupuesto : BaseEntity
{
    public int Mes { get; set; }
    public int Anio { get; set; }
    public decimal MontoTotal { get; set; }
    public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
    public ICollection<Propina> Propinas { get; set; } = new List<Propina>();
}
