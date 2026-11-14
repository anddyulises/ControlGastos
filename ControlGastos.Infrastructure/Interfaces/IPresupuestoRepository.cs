using ControlGastos.Domain.Entities;
using ControlGastos.Domain.Repository;
using ControlGastos.Infrastructure.Models;

namespace ControlGastos.Infrastructure.Interfaces;

public interface IPresupuestoRepository : IBaseRepository<Presupuesto>
{
    Task<IEnumerable<PresupuestoModel>> GetAllModelsAsync();
    Task<PresupuestoModel?> GetPresupuestoModelAsync(int id);
}
