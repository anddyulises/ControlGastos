using Microsoft.AspNetCore.Mvc;
using ControlGastos.Application.Contract;
using ControlGastos.Application.Dtos;

namespace ControlGastos.Api.Controllers;

[Route("api/[controller]")]
public class PropinasController : BaseApiController
{
    private readonly IPropinaService _service;

    public PropinasController(IPropinaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => FromResult(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => FromResult(await _service.GetByIdAsync(id));

    [HttpGet("by-presupuesto/{presupuestoId}")]
    public async Task<IActionResult> GetByPresupuesto(int presupuestoId)
        => FromResult(await _service.GetByPresupuestoAsync(presupuestoId));

    [HttpPost]
    public async Task<IActionResult> Create(AddPropinaDto dto) => FromResult(await _service.AddAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePropinaDto dto)
    {
        dto.Id = id;
        return FromResult(await _service.UpdateAsync(dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => FromResult(await _service.RemoveAsync(id));
}
