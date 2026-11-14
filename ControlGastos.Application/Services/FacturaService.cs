using ControlGastos.Application.Contract;
using ControlGastos.Application.Core;
using ControlGastos.Application.Dtos;
using ControlGastos.Domain.Entities;
using ControlGastos.Infrastructure.Interfaces;

namespace ControlGastos.Application.Services;

public class FacturaService : BaseService, IFacturaService
{
    private readonly IFacturaRepository _repo;
    private readonly IPresupuestoRepository _presupuestoRepo;

    public FacturaService(IFacturaRepository repo, IPresupuestoRepository presupuestoRepo)
    {
        _repo = repo;
        _presupuestoRepo = presupuestoRepo;
    }

    public Task<ServiceResult> GetAllAsync() => ExecuteAsync(async () =>
    {
        var facturas = await _repo.GetAllAsync();
        return Ok(facturas.Select(ToDto).ToList());
    });

    public Task<ServiceResult> GetByIdAsync(int id) => ExecuteAsync(async () =>
    {
        var f = await _repo.GetByIdAsync(id);
        return f is null ? NotFound("Factura") : Ok(ToDto(f));
    });

    public Task<ServiceResult> GetByPresupuestoAsync(int presupuestoId) => ExecuteAsync(async () =>
    {
        var facturas = await _repo.GetAllAsync();
        return Ok(facturas.Where(f => f.PresupuestoId == presupuestoId).Select(ToDto).ToList());
    });

    public Task<ServiceResult> AddAsync(AddFacturaDto dto) => ExecuteAsync(async () =>
    {
        var errors = await ValidateAsync(dto);
        if (errors.Count > 0) return Fail(errors);

        var f = await _repo.AddAsync(new Factura
        {
            Descripcion = dto.Descripcion.Trim(),
            Monto = dto.Monto,
            Fecha = dto.Fecha == default ? DateTime.UtcNow : dto.Fecha,
            Establecimiento = dto.Establecimiento.Trim(),
            NumeroFactura = dto.NumeroFactura.Trim(),
            PresupuestoId = dto.PresupuestoId
        });
        return Ok(ToDto(f), "Factura registrada correctamente.");
    });

    public Task<ServiceResult> UpdateAsync(UpdateFacturaDto dto) => ExecuteAsync(async () =>
    {
        var f = await _repo.GetByIdAsync(dto.Id);
        if (f is null) return NotFound("Factura");

        var errors = await ValidateAsync(dto);
        if (errors.Count > 0) return Fail(errors);

        f.Descripcion = dto.Descripcion.Trim();
        f.Monto = dto.Monto;
        f.Fecha = dto.Fecha == default ? f.Fecha : dto.Fecha;
        f.Establecimiento = dto.Establecimiento.Trim();
        f.NumeroFactura = dto.NumeroFactura.Trim();
        f.PresupuestoId = dto.PresupuestoId;
        await _repo.UpdateAsync(f);
        return Ok(ToDto(f), "Factura actualizada correctamente.");
    });

    public Task<ServiceResult> RemoveAsync(int id) => ExecuteAsync(async () =>
    {
        if (!await _repo.ExistsAsync(id)) return NotFound("Factura");
        await _repo.DeleteAsync(id);
        return Ok(null, "Factura eliminada correctamente.");
    });

    // Validaciones de cada campo de la entidad Factura
    private async Task<List<string>> ValidateAsync(AddFacturaDto dto)
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
            errors.Add("La fecha de la factura no puede ser futura.");

        if (string.IsNullOrWhiteSpace(dto.Establecimiento))
            errors.Add("El establecimiento es requerido.");
        else if (dto.Establecimiento.Trim().Length > 150)
            errors.Add("El establecimiento no puede exceder 150 caracteres.");

        if (string.IsNullOrWhiteSpace(dto.NumeroFactura))
            errors.Add("El número de factura es requerido.");
        else if (dto.NumeroFactura.Trim().Length > 50)
            errors.Add("El número de factura no puede exceder 50 caracteres.");

        if (dto.PresupuestoId <= 0)
            errors.Add("El presupuesto es requerido.");
        else if (!await _presupuestoRepo.ExistsAsync(dto.PresupuestoId))
            errors.Add("El presupuesto no existe.");

        return errors;
    }

    private static FacturaDto ToDto(Factura f) => new()
    {
        Id = f.Id,
        Descripcion = f.Descripcion,
        Monto = f.Monto,
        Fecha = f.Fecha,
        Establecimiento = f.Establecimiento,
        NumeroFactura = f.NumeroFactura,
        PresupuestoId = f.PresupuestoId
    };
}
