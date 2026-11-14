using ControlGastos.Application.Core;
using ControlGastos.Application.Dtos;

namespace ControlGastos.Application.Contract;

public interface IPresupuestoService : IBaseService<AddPresupuestoDto, UpdatePresupuestoDto>
{
}
