using ControlGastos.Application.Core;
using ControlGastos.Application.Dtos;

namespace ControlGastos.Application.Contract;

public interface IFacturaService : IBaseService<AddFacturaDto, UpdateFacturaDto>
{
    Task<ServiceResult> GetByPresupuestoAsync(int presupuestoId);
}
