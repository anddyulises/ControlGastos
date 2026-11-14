using Microsoft.EntityFrameworkCore;
using ControlGastos.Domain.Entities;
using ControlGastos.Infrastructure.Context;
using ControlGastos.Infrastructure.Core;
using ControlGastos.Infrastructure.Exceptions;
using ControlGastos.Infrastructure.Interfaces;
using ControlGastos.Infrastructure.Models;

namespace ControlGastos.Infrastructure.Repositories;

public class FacturaRepository : BaseRepository<Factura>, IFacturaRepository
{
    public FacturaRepository(ControlGastosContext context) : base(context) { }

    public override async Task<Factura> AddAsync(Factura factura)
    {
        if (string.IsNullOrWhiteSpace(factura.Descripcion))
            throw new FacturaException("La descripción de la factura es requerida.");
        if (string.IsNullOrWhiteSpace(factura.Establecimiento))
            throw new FacturaException("El establecimiento es requerido.");
        if (factura.Monto <= 0)
            throw new FacturaException("El monto debe ser mayor que cero.");
        if (!await _context.Presupuestos.AnyAsync(p => p.Id == factura.PresupuestoId && !p.Deleted))
            throw new FacturaException("El presupuesto no existe.");

        return await base.AddAsync(factura);
    }

    public async Task<IEnumerable<FacturaModel>> GetFacturasByPresupuestoAsync(int presupuestoId)
    {
        var facturas = await _set
            .Where(f => f.PresupuestoId == presupuestoId && !f.Deleted)
            .ToListAsync();

        return facturas.Select(f => new FacturaModel
        {
            Id = f.Id,
            Descripcion = f.Descripcion,
            Monto = f.Monto,
            Fecha = f.Fecha,
            Establecimiento = f.Establecimiento,
            NumeroFactura = f.NumeroFactura,
            PresupuestoId = f.PresupuestoId
        });
    }
}
