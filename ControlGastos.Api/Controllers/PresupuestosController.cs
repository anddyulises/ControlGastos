using Microsoft.AspNetCore.Mvc;
using ControlGastos.Domain.Entities;
using ControlGastos.Infrastructure.Exceptions;
using ControlGastos.Infrastructure.Interfaces;
using ControlGastos.Infrastructure.Models;

namespace ControlGastos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PresupuestosController : ControllerBase
{
    private readonly IPresupuestoRepository _repo;

    public PresupuestosController(IPresupuestoRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PresupuestoModel>>> GetAll()
        => Ok(await _repo.GetAllModelsAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<PresupuestoModel>> GetById(int id)
    {
        var model = await _repo.GetPresupuestoModelAsync(id);
        return model is null ? NotFound() : Ok(model);
    }

    [HttpPost]
    public async Task<ActionResult<PresupuestoModel>> Create(PresupuestoModel m)
    {
        try
        {
            var p = await _repo.AddAsync(new Presupuesto
            {
                Mes = m.Mes,
                Anio = m.Anio,
                MontoTotal = m.MontoTotal
            });
            var created = await _repo.GetPresupuestoModelAsync(p.Id);
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, created);
        }
        catch (PresupuestoException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PresupuestoModel m)
    {
        var p = await _repo.GetByIdAsync(id);
        if (p is null) return NotFound();
        p.Mes = m.Mes;
        p.Anio = m.Anio;
        p.MontoTotal = m.MontoTotal;
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
