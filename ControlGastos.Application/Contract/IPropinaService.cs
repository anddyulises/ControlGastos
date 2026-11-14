using ControlGastos.Application.Core;
using ControlGastos.Application.Dtos;

namespace ControlGastos.Application.Contract;

public interface IPropinaService : IBaseService<AddPropinaDto, UpdatePropinaDto>
{
    Task<ServiceResult> GetByPresupuestoAsync(int presupuestoId);
}
