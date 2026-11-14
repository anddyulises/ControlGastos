using ControlGastos.Domain.Core;

namespace ControlGastos.Domain.Entities;

public class Factura : Gasto
{
    public string Establecimiento { get; set; } = string.Empty;
    public string NumeroFactura { get; set; } = string.Empty;
    public Presupuesto? Presupuesto { get; set; }
}
