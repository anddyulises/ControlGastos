using Microsoft.EntityFrameworkCore;
using ControlGastos.Domain.Entities;
using ControlGastos.Infrastructure.Context;
using ControlGastos.Infrastructure.Core;
using ControlGastos.Infrastructure.Exceptions;
using ControlGastos.Infrastructure.Interfaces;
using ControlGastos.Infrastructure.Models;

namespace ControlGastos.Infrastructure.Repositories;

public class PropinaRepository : BaseRepository<Propina>, IPropinaRepository
{
    public PropinaRepository(ControlGastosContext context) : base(context) { }

    public override async Task<Propina> AddAsync(Propina propina)
    {
        if (string.IsNullOrWhiteSpace(propina.Descripcion))
            throw new PropinaException("La descripción de la propina es requerida.");
        if (propina.Monto <= 0)
            throw new PropinaException("El monto debe ser mayor que cero.");
        if (!await _context.Presupuestos.AnyAsync(p => p.Id == propina.PresupuestoId && !p.Deleted))
            throw new PropinaException("El presupuesto no existe.");

        return await base.AddAsync(propina);
    }

    public async Task<IEnumerable<PropinaModel>> GetPropinasByPresupuestoAsync(int presupuestoId)
    {
        var propinas = await _set
            .Where(p => p.PresupuestoId == presupuestoId && !p.Deleted)
            .ToListAsync();

        return propinas.Select(p => new PropinaModel
        {
            Id = p.Id,
            Descripcion = p.Descripcion,
            Monto = p.Monto,
            Fecha = p.Fecha,
            Beneficiario = p.Beneficiario,
            PresupuestoId = p.PresupuestoId
        });
    }
}
