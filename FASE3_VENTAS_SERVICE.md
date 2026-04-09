# Fase 3 — Primer bloque extraído a SalesService

## Qué se extrajo de `VentanaVentas`

Se movió lógica de negocio de cálculo/validación hacia `AppCore/Sales/SalesService`:

1. **Cálculo de totales de venta**
   - subtotal
   - descuento (porcentaje o monto)
   - total neto

2. **Validación mínima de venta cobrable/registrable**
   - `CanCreateSale(...)` exige al menos una línea válida (cantidad > 0 y precio > 0).

## Clases nuevas

- `ISalesService` (`AppCore/Abstractions`)
- `SalesService` (`AppCore/Sales`)
- `SaleLineInput` y `SalesTotals` (`AppCore/Sales`)
- `AppServices` (`Infrastructure/Services`) para resolver el servicio en transición.

## Integración actual en UI

`VentanaVentas` ahora usa `AppServices.Sales` para:
- `AsignarTotales()` (delegando cálculo al servicio)
- validar venta antes de registrar (`button18_Click`)
- validar venta antes de cobrar (`Cobrar_Click`)

## Beneficio inmediato

- Reduce responsabilidad del formulario en reglas de negocio de ventas.
- Facilita pruebas del cálculo sin depender de WinForms.
- Prepara extracción de siguientes bloques (stock, preparación de factura, persistencia).

## Próximo bloque recomendado

1. Extraer preparación de `Factura` a `SalesService` (`BuildSaleDraft`).
2. Extraer validación de stock por línea.
3. Centralizar reglas de cambio de precio y descuento por permiso.
