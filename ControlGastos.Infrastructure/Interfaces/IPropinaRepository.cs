using ControlGastos.Domain.Entities;
using ControlGastos.Domain.Repository;
using ControlGastos.Infrastructure.Models;

namespace ControlGastos.Infrastructure.Interfaces;

public interface IPropinaRepository : IBaseRepository<Propina>
{
    Task<IEnumerable<PropinaModel>> GetPropinasByPresupuestoAsync(int presupuestoId);
}
