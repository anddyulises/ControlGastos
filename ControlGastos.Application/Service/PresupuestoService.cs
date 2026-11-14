using ControlGastos.Application.Contract;
using ControlGastos.Application.Core;
using ControlGastos.Application.Dtos;
using ControlGastos.Domain.Entities;
using ControlGastos.Infrastructure.Interfaces;
using ControlGastos.Infrastructure.Models;

namespace ControlGastos.Application.Service;

public class PresupuestoService : BaseService, IPresupuestoService
{
    private readonly IPresupuestoRepository _repo;

    public PresupuestoService(IPresupuestoRepository repo)
    {
        _repo = repo;
    }

    public Task<ServiceResult> GetAllAsync() => ExecuteAsync(async () =>
    {
        var models = await _repo.GetAllModelsAsync();
        return Ok(models.Select(ToDto).ToList());
    });

    public Task<ServiceResult> GetByIdAsync(int id) => ExecuteAsync(async () =>
    {
        var model = await _repo.GetPresupuestoModelAsync(id);
        return model is null ? NotFound("Presupuesto") : Ok(ToDto(model));
    });

    public Task<ServiceResult> AddAsync(AddPresupuestoDto dto) => ExecuteAsync(async () =>
    {
        var errors = await ValidateAsync(dto, null);
        if (errors.Count > 0) return Fail(errors);

        var p = await _repo.AddAsync(new Presupuesto
        {
            Mes = dto.Mes,
            Anio = dto.Anio,
            MontoTotal = dto.MontoTotal
        });
        var model = await _repo.GetPresupuestoModelAsync(p.Id);
        return Ok(ToDto(model!), "Presupuesto creado correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdatePresupuestoDto dto) => ExecuteAsync(async () =>
    {
        var p = await _repo.GetByIdAsync(dto.Id);
        if (p is null) return NotFound("Presupuesto");

        var errors = await ValidateAsync(dto, dto.Id);
        if (errors.Count > 0) return Fail(errors);

        p.Mes = dto.Mes;
        p.Anio = dto.Anio;
        p.MontoTotal = dto.MontoTotal;
        await _repo.UpdateAsync(p);

        var model = await _repo.GetPresupuestoModelAsync(p.Id);
        return Ok(ToDto(model!), "Presupuesto actualizado correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Presupuesto");
        await _repo.DeleteAsync(id);
        return Ok(null, "Presupuesto eliminado correctamente.");
    });

    // Validaciones de cada campo de la entidad Presupuesto
    private async Task<List<string>> ValidateAsync(AddPresupuestoDto dto, int? currentId)
    {
        var errors = new List<string>();

        if (dto.Mes < 1 || dto.Mes > 12)
            errors.Add("El mes debe estar entre 1 y 12.");

        if (dto.Anio < 2000 || dto.Anio > 2100)
            errors.Add("El año debe estar entre 2000 y 2100.");

        if (dto.MontoTotal <= 0)
            errors.Add("El monto del presupuesto debe ser mayor que cero.");
        else if (dto.MontoTotal > MaxMonto)
            errors.Add($"El monto del presupuesto no puede exceder {MaxMonto:N2}.");

        if (errors.Count == 0)
        {
            var existentes = await _repo.GetAllAsync();
            if (existentes.Any(x => x.Id != currentId && x.Mes == dto.Mes && x.Anio == dto.Anio))
                errors.Add("Ya existe un presupuesto para ese mes y año.");
        }

        return errors;
    }

    private static PresupuestoDto ToDto(PresupuestoModel m) => new()
    {
        Id = m.Id,
        Mes = m.Mes,
        Anio = m.Anio,
        MontoTotal = m.MontoTotal,
        TotalGastado = m.TotalGastado,
        PorcentajeUsado = m.PorcentajeUsado,
        Estado = m.Estado
    };
}
