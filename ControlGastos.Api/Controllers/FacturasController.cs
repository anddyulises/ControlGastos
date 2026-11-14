using Microsoft.AspNetCore.Mvc;
using ControlGastos.Domain.Entities;
using ControlGastos.Infrastructure.Exceptions;
using ControlGastos.Infrastructure.Interfaces;
using ControlGastos.Infrastructure.Models;

namespace ControlGastos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacturasController : ControllerBase
{
    private readonly IFacturaRepository _repo;

    public FacturasController(IFacturaRepository repo)
    {
        _repo = repo;
    }

    private static FacturaModel ToModel(Factura f) => new()
    {
        Id = f.Id,
        Descripcion = f.Descripcion,
        Monto = f.Monto,
        Fecha = f.Fecha,
        Establecimiento = f.Establecimiento,
        NumeroFactura = f.NumeroFactura,
        PresupuestoId = f.PresupuestoId
    };

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FacturaModel>>> GetAll()
        => Ok((await _repo.GetAllAsync()).Select(ToModel));

    [HttpGet("{id}")]
    public async Task<ActionResult<FacturaModel>> GetById(int id)
    {
        var f = await _repo.GetByIdAsync(id);
        return f is null ? NotFound() : Ok(ToModel(f));
    }

    [HttpGet("by-presupuesto/{presupuestoId}")]
    public async Task<ActionResult<IEnumerable<FacturaModel>>> GetByPresupuesto(int presupuestoId)
        => Ok(await _repo.GetFacturasByPresupuestoAsync(presupuestoId));

    [HttpPost]
    public async Task<ActionResult<FacturaModel>> Create(FacturaModel m)
    {
        try
        {
            var f = await _repo.AddAsync(new Factura
            {
                Descripcion = m.Descripcion,
                Monto = m.Monto,
                Fecha = m.Fecha == default ? DateTime.UtcNow : m.Fecha,
                Establecimiento = m.Establecimiento,
                NumeroFactura = m.NumeroFactura,
                PresupuestoId = m.PresupuestoId
            });
            return CreatedAtAction(nameof(GetById), new { id = f.Id }, ToModel(f));
        }
        catch (FacturaException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, FacturaModel m)
    {
        var f = await _repo.GetByIdAsync(id);
        if (f is null) return NotFound();
        f.Descripcion = m.Descripcion;
        f.Monto = m.Monto;
        f.Fecha = m.Fecha == default ? f.Fecha : m.Fecha;
        f.Establecimiento = m.Establecimiento;
        f.NumeroFactura = m.NumeroFactura;
        f.PresupuestoId = m.PresupuestoId;
        await _repo.UpdateAsync(f);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _repo.ExistsAsync(id)) return NotFound();
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
