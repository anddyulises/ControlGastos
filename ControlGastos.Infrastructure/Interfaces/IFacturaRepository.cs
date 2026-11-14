using ControlGastos.Domain.Entities;
using ControlGastos.Domain.Repository;
using ControlGastos.Infrastructure.Models;

namespace ControlGastos.Infrastructure.Interfaces;

public interface IFacturaRepository : IBaseRepository<Factura>
{
    Task<IEnumerable<FacturaModel>> GetFacturasByPresupuestoAsync(int presupuestoId);
}
