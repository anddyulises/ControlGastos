using Microsoft.AspNetCore.Mvc;
using ControlGastos.Domain.Entities;
using ControlGastos.Infrastructure.Exceptions;
using ControlGastos.Infrastructure.Interfaces;
using ControlGastos.Infrastructure.Models;

namespace ControlGastos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropinasController : ControllerBase
{
    private readonly IPropinaRepository _repo;

    public PropinasController(IPropinaRepository repo)
    {
        _repo = repo;
    }

    private static PropinaModel ToModel(Propina p) => new()
    {
        Id = p.Id,
        Descripcion = p.Descripcion,
        Monto = p.Monto,
        Fecha = p.Fecha,
        Beneficiario = p.Beneficiario,
        PresupuestoId = p.PresupuestoId
    };

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropinaModel>>> GetAll()
        => Ok((await _repo.GetAllAsync()).Select(ToModel));

    [HttpGet("{id}")]
    public async Task<ActionResult<PropinaModel>> GetById(int id)
    {
        var p = await _repo.GetByIdAsync(id);
        return p is null ? NotFound() : Ok(ToModel(p));
    }

    [HttpGet("by-presupuesto/{presupuestoId}")]
    public async Task<ActionResult<IEnumerable<PropinaModel>>> GetByPresupuesto(int presupuestoId)
        => Ok(await _repo.GetPropinasByPresupuestoAsync(presupuestoId));

    [HttpPost]
    public async Task<ActionResult<PropinaModel>> Create(PropinaModel m)
    {
        try
        {
            var p = await _repo.AddAsync(new Propina
            {
                Descripcion = m.Descripcion,
                Monto = m.Monto,
                Fecha = m.Fecha == default ? DateTime.UtcNow : m.Fecha,
                Beneficiario = m.Beneficiario,
                PresupuestoId = m.PresupuestoId
            });
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, ToModel(p));
        }
        catch (PropinaException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PropinaModel m)
    {
        var p = await _repo.GetByIdAsync(id);
        if (p is null) return NotFound();
        p.Descripcion = m.Descripcion;
        p.Monto = m.Monto;
        p.Fecha = m.Fecha == default ? p.Fecha : m.Fecha;
        p.Beneficiario = m.Beneficiario;
        p.PresupuestoId = m.PresupuestoId;
        await _repo.UpdateAsync(p);
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
