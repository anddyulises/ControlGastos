using ControlGastos.Application.Contract;
using ControlGastos.Application.Core;
using ControlGastos.Application.Dtos;
using ControlGastos.Domain.Entities;
using ControlGastos.Infrastructure.Interfaces;

namespace ControlGastos.Application.Services;

public class PropinaService : BaseService, IPropinaService
{
    private readonly IPropinaRepository _repo;
    private readonly IPresupuestoRepository _presupuestoRepo;

    public PropinaService(IPropinaRepository repo, IPresupuestoRepository presupuestoRepo)
    {
        _repo = repo;
        _presupuestoRepo = presupuestoRepo;
    }

    public Task<ServiceResult> GetAllAsync() => ExecuteAsync(async () =>
    {
        var propinas = await _repo.GetAllAsync();
        return Ok(propinas.Select(ToDto).ToList());
    });

    public Task<ServiceResult> GetByIdAsync(int id) => ExecuteAsync(async () =>
    {
        var p = await _repo.GetByIdAsync(id);
        return p is null ? NotFound("Propina") : Ok(ToDto(p));
    });

    public Task<ServiceResult> GetByPresupuestoAsync(int presupuestoId) => ExecuteAsync(async () =>
    {
        var propinas = await _repo.GetAllAsync();
        return Ok(propinas.Where(p => p.PresupuestoId == presupuestoId).Select(ToDto).ToList());
    });

    public Task<ServiceResult> AddAsync(AddPropinaDto dto) => ExecuteAsync(async () =>
    {
        var errors = await ValidateAsync(dto);
        if (errors.Count > 0) return Fail(errors);

        var p = await _repo.AddAsync(new Propina
        {
            Descripcion = dto.Descripcion.Trim(),
            Monto = dto.Monto,
            Fecha = dto.Fecha == default ? DateTime.UtcNow : dto.Fecha,
            Beneficiario = (dto.Beneficiario ?? string.Empty).Trim(),
            PresupuestoId = dto.PresupuestoId
        });
        return Ok(ToDto(p), "Propina registrada correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdatePropinaDto dto) => ExecuteAsync(async () =>
    {
        var p = await _repo.GetByIdAsync(dto.Id);
        if (p is null) return NotFound("Propina");

        var errors = await ValidateAsync(dto);
        if (errors.Count > 0) return Fail(errors);

        p.Descripcion = dto.Descripcion.Trim();
        p.Monto = dto.Monto;
        p.Fecha = dto.Fecha == default ? p.Fecha : dto.Fecha;
        p.Beneficiario = (dto.Beneficiario ?? string.Empty).Trim();
        p.PresupuestoId = dto.PresupuestoId;
        await _repo.UpdateAsync(p);
        return Ok(ToDto(p), "Propina actualizada correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Propina");
        await _repo.DeleteAsync(id);
        return Ok(null, "Propina eliminada correctamente.");
    });

    // Validaciones de cada campo de la entidad Propina
    private async Task<List<string>> ValidateAsync(AddPropinaDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Descripcion))
            errors.Add("La descripción es requerida.");
        else if (dto.Descripcion.Trim().Length > 200)
            errors.Add("La descripción no puede exceder 200 caracteres.");

        if (dto.Monto <= 0)
            errors.Add("El monto debe ser mayor que cero.");
        else if (dto.Monto > MaxMonto)
            errors.Add($"El monto no puede exceder {MaxMonto:N2}.");

        if (dto.Fecha != default && dto.Fecha.Date > DateTime.UtcNow.Date.AddDays(1))
            errors.Add("La fecha de la propina no puede ser futura.");

        if (dto.Beneficiario != null && dto.Beneficiario.Trim().Length > 150)
            errors.Add("El beneficiario no puede exceder 150 caracteres.");

        if (dto.PresupuestoId <= 0)
            errors.Add("El presupuesto es requerido.");
        else if (!await _presupuestoRepo.ExistsAsync(dto.PresupuestoId))
            errors.Add("El presupuesto no existe.");

        return errors;
    }

    private static PropinaDto ToDto(Propina p) => new()
    {
        Id = p.Id,
        Descripcion = p.Descripcion,
        Monto = p.Monto,
        Fecha = p.Fecha,
        Beneficiario = p.Beneficiario,
        PresupuestoId = p.PresupuestoId
    };
}
