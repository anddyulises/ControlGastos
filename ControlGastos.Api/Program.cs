using Microsoft.EntityFrameworkCore;
using ControlGastos.Application.Contract;
using ControlGastos.Application.Service;
using ControlGastos.Infrastructure.Context;
using ControlGastos.Infrastructure.Interfaces;
using ControlGastos.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ControlGastosContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPresupuestoRepository, PresupuestoRepository>();
builder.Services.AddScoped<IFacturaRepository, FacturaRepository>();
builder.Services.AddScoped<IPropinaRepository, PropinaRepository>();

builder.Services.AddScoped<IPresupuestoService, PresupuestoService>();
builder.Services.AddScoped<IFacturaService, FacturaService>();
builder.Services.AddScoped<IPropinaService, PropinaService>();

var app = builder.Build();

// Crea la base de datos SQLite si no existe
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ControlGastosContext>().Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
