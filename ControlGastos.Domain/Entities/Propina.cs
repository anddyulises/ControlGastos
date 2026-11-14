using ControlGastos.Domain.Core;

namespace ControlGastos.Domain.Entities;

public class Propina : Gasto
{
    public string Beneficiario { get; set; } = string.Empty;
    public Presupuesto? Presupuesto { get; set; }
}
