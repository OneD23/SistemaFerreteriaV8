# Fase 3 — Primer bloque extraído a SalesService

## Qué se extrajo de `VentanaVentas`

Se movió lógica de negocio de cálculo/validación hacia `AppCore/Sales/SalesService`:

1. **Cálculo de totales de venta**
   - subtotal
   - descuento (porcentaje o monto)
   - total neto

2. **Preparación y validación de venta**
   - `PrepareSale(...)` valida líneas, normaliza datos y devuelve estructura lista para persistencia.
   - valida stock por línea para productos no genéricos.
   - identifica productos no encontrados para validación de inventario.

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
- armar líneas de entrada hacia `SalePreparationRequest` y aplicar `SalePreparationResult`

## Beneficio inmediato

- Reduce responsabilidad del formulario en reglas de negocio de ventas y consistencia de stock.
- Facilita pruebas del cálculo sin depender de WinForms.
- Prepara extracción de siguientes bloques (stock, preparación de factura, persistencia).

## Próximo bloque recomendado

1. Extraer armado final de `Factura` persistible (`BuildInvoiceDraft`) para eliminar duplicación en `RegistrarFacturaAsync`.
2. Mover validación de precios/cambio de precio completamente al servicio.
3. Añadir test unitarios del `SalesService` (cálculo, stock, errores de línea).
