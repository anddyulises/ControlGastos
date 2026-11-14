using Microsoft.EntityFrameworkCore;
using ControlGastos.Domain.Entities;
using ControlGastos.Infrastructure.Context;
using ControlGastos.Infrastructure.Core;
using ControlGastos.Infrastructure.Exceptions;
using ControlGastos.Infrastructure.Interfaces;
using ControlGastos.Infrastructure.Models;

namespace ControlGastos.Infrastructure.Repositories;

public class PresupuestoRepository : BaseRepository<Presupuesto>, IPresupuestoRepository
{
    public PresupuestoRepository(ControlGastosContext context) : base(context) { }

    public override async Task<Presupuesto> AddAsync(Presupuesto presupuesto)
    {
        if (presupuesto.Mes < 1 || presupuesto.Mes > 12)
            throw new PresupuestoException("El mes debe estar entre 1 y 12.");
        if (presupuesto.Anio < 2000)
            throw new PresupuestoException("El año no es válido.");
        if (presupuesto.MontoTotal <= 0)
            throw new PresupuestoException("El monto del presupuesto debe ser mayor que cero.");
        if (await _set.AnyAsync(p => p.Mes == presupuesto.Mes && p.Anio == presupuesto.Anio && !p.Deleted))
            throw new PresupuestoException("Ya existe un presupuesto para ese mes y año.");

        return await base.AddAsync(presupuesto);
    }

    public async Task<IEnumerable<PresupuestoModel>> GetAllModelsAsync()
    {
        var presupuestos = await _set.Where(p => !p.Deleted).ToListAsync();
        var models = new List<PresupuestoModel>();
        foreach (var p in presupuestos)
            models.Add(await BuildModelAsync(p));
        return models;
    }

    public async Task<PresupuestoModel?> GetPresupuestoModelAsync(int id)
    {
        var presupuesto = await GetByIdAsync(id);
        return presupuesto is null ? null : await BuildModelAsync(presupuesto);
    }

    // Estados: < 80% Disponible, 80%-100% Próximo a agotarse, > 100% Excedido
    private async Task<PresupuestoModel> BuildModelAsync(Presupuesto p)
    {
        // Se suma en memoria porque SQLite no soporta SUM sobre decimal.
        var facturas = await _context.Facturas
            .Where(f => f.PresupuestoId == p.Id && !f.Deleted)
            .Select(f => f.Monto).ToListAsync();
        var propinas = await _context.Propinas
            .Where(x => x.PresupuestoId == p.Id && !x.Deleted)
            .Select(x => x.Monto).ToListAsync();

        var total = facturas.Sum() + propinas.Sum();
        var porcentaje = p.MontoTotal > 0 ? Math.Round(total / p.MontoTotal * 100, 2) : 0;
        var estado = porcentaje > 100 ? "Excedido"
                   : porcentaje >= 80 ? "Próximo a agotarse"
                   : "Disponible";

        return new PresupuestoModel
        {
            Id = p.Id,
            Mes = p.Mes,
            Anio = p.Anio,
            MontoTotal = p.MontoTotal,
            TotalGastado = total,
            PorcentajeUsado = porcentaje,
            Estado = estado
        };
    }
}
