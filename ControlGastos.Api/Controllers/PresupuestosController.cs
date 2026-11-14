using Microsoft.AspNetCore.Mvc;
using ControlGastos.Application.Contract;
using ControlGastos.Application.Dtos;

namespace ControlGastos.Api.Controllers;

[Route("api/[controller]")]
public class PresupuestosController : BaseApiController
{
    private readonly IPresupuestoService _service;

    public PresupuestosController(IPresupuestoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => FromResult(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => FromResult(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(AddPresupuestoDto dto) => FromResult(await _service.AddAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePresupuestoDto dto)
    {
        dto.Id = id;
        return FromResult(await _service.UpdateAsync(dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => FromResult(await _service.RemoveAsync(id));
}
