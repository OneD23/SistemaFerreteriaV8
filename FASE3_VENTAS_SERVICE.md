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

3. **Construcción persistible de factura/venta**
   - `BuildInvoiceDraft(...)` arma estructura de persistencia con líneas, subtotales, descuentos, impuestos, total y metadatos operativos.

## Clases nuevas

- `ISalesService` (`AppCore/Abstractions`)
- `SalesService` (`AppCore/Sales`)
- `SaleLineInput` y `SalesTotals` (`AppCore/Sales`)
- `InvoiceDraft`, `InvoiceDraftLine`, `InvoiceDraftMetadata` (`AppCore/Sales`)
- `AppServices` (`Infrastructure/Services`) para resolver el servicio en transición.

## Integración actual en UI

`VentanaVentas` ahora usa `AppServices.Sales` para:
- `AsignarTotales()` (delegando cálculo al servicio)
- validar venta antes de registrar (`button18_Click`)
- validar venta antes de cobrar (`Cobrar_Click`)
- armar líneas de entrada hacia `SalePreparationRequest` y aplicar `SalePreparationResult`
- construir draft persistible y mapearlo a `Factura` antes de guardar

## Beneficio inmediato

- Reduce responsabilidad del formulario en reglas de negocio de ventas, consistencia de stock y armado persistible.
- Facilita pruebas del cálculo sin depender de WinForms.
- Prepara extracción de siguientes bloques (stock, preparación de factura, persistencia).

## Próximo bloque recomendado

1. Mover validación de precios/cambio de precio completamente al servicio.
2. Añadir test unitarios del `SalesService` (cálculo, stock, errores de línea y draft persistible).
3. Endurecer atomicidad del `SalesWorkflowService` con transacción real o compensación auditable.
