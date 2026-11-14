using Microsoft.AspNetCore.Mvc;
using ControlGastos.Application.Contract;
using ControlGastos.Application.Dtos;

namespace ControlGastos.Api.Controllers;

[Route("api/[controller]")]
public class FacturasController : BaseApiController
{
    private readonly IFacturaService _service;

    public FacturasController(IFacturaService service)
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
    public async Task<IActionResult> Create(AddFacturaDto dto) => FromResult(await _service.AddAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateFacturaDto dto)
    {
        dto.Id = id;
        return FromResult(await _service.UpdateAsync(dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => FromResult(await _service.RemoveAsync(id));
}
