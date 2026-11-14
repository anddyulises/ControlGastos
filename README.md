# Control de Gastos - API

**Autor:** Anddy Ortega · **Matrícula:** 2024-0243

API Web (Controllers) para la gestión de facturas y propinas con alertas por porcentaje del presupuesto mensual.
Capas de Dominio e Infraestructura (.NET, EF Core, SQLite).

## Estructura

- `ControlGastos.Domain`: `Core` (BaseEntity, Gasto), `Entities` (Presupuesto, Factura, Propina), `Repository` (IBaseRepository).
- `ControlGastos.Infrastructure`: `Context`, `Core` (BaseRepository), `Exceptions`, `Interfaces`, `Models` (DTOs), `Repositories`.
- `ControlGastos.Application` (rama `feature/capa-servicio`): `Contract` (interfaces de servicio), `Core` (BaseService, IBaseService, ServiceResult), `Dtos` (Presupuesto, Factura, Propina, DtoBase) y `Service` (PresupuestoService, FacturaService, PropinaService) con validación de cada campo.
- `ControlGastos.Api`: Controllers, `Program.cs`, `appsettings.json`.

## Estados del presupuesto

| Porcentaje | Estado |
|------------|--------|
| < 80% | Disponible |
| 80% - 100% | Próximo a agotarse |
| > 100% | Excedido |

## Ejecutar

```bash
dotnet run --project ControlGastos.Api
```
Swagger en `/swagger`. La base de datos SQLite (`controlgastos.db`) se crea sola al iniciar.
