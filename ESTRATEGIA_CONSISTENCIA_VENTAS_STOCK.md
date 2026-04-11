# Estrategia de consistencia operativa (Bloques T-U-V-W)

## Bloque T — Consistencia de ventas + stock

### Antes
- La venta se persistía y luego se intentaba descontar stock.
- Si fallaba el descuento había compensación best-effort, pero sin estrategia explícita de estados.
- El flujo podía fallar por eventos de auditoría y contaminar el resultado de la operación.

### Después
- `ConfirmSaleAsync` define `operationId` único y persiste la venta en estado `pending` antes del movimiento de stock.
- El descuento de stock se delega a `IProductService.ApplySaleStockAsync` con retry automático simple para fallos transitorios.
- Si hubo aplicación parcial de stock, se ejecuta `CompensateStockAsync` y se marca la venta como `compensated` o `stock_error` según resultado.
- Si el fallo ocurre al finalizar factura después de descontar stock, se intenta compensar y se marca `compensated` o `failed`.
- La auditoría no rompe el flujo de negocio: se encapsula en `SafeAuditAsync`.

### Puntos de fallo identificados en `ConfirmSaleAsync`
1. Mapeo/validación de líneas (`validation_failed`).
2. Persistencia inicial de factura (`invoice_persisted` no alcanzado).
3. Descuento de stock completo o parcial (`stock_apply_failed`).
4. Persistencia final de estado confirmado (`invoice_finalize_failed`).
5. Fallos inesperados no clasificados (`unexpected_error`).

## Bloque U — Estados de venta

Estados operativos definidos:
- `pending`
- `confirmed`
- `stock_error`
- `failed`
- `compensated`

Transición principal:
- `pending -> confirmed` (flujo exitoso)
- `pending -> stock_error` (no se pudo aplicar stock y no fue compensable)
- `pending -> compensated` (fallo con aplicación parcial, luego compensación exitosa)
- `pending -> failed` (fallo en finalización + compensación no exitosa)

## Bloque V — Auditoría operativa

Mejoras:
- `operationId` consistente para todos los eventos de una venta.
- Event types normalizados:
  - `sales.validation_failed`
  - `sales.invoice_persisted`
  - `sales.stock_applied`
  - `sales.stock_apply_failed`
  - `sales.invoice_finalize_failed`
  - `sales.confirmed`
  - `sales.unexpected_error`
- Metadata con `invoiceId`, `status`, `attempts`, flags de compensación y contexto.

## Bloque W — Evolución de ProductService

Se extiende el contrato para ser fuente de verdad de inventario:
- `ApplySaleStockAsync(...)`
- `CompensateStockAsync(...)`
- Se centraliza validación y movimiento en infraestructura, reduciendo acoplamiento de UI.

## Escenarios de fallo cubiertos

1. **Venta guardada pero stock no actualizado**
   - Estado final: `stock_error` o `compensated`.
   - Auditoría: `sales.stock_apply_failed`.
2. **Stock actualizado pero error posterior**
   - Se intenta compensación.
   - Estado final: `compensated` o `failed`.
   - Auditoría: `sales.invoice_finalize_failed`.
3. **Error de auditoría**
   - No tumba la operación de venta.
   - Se preserva consistencia funcional del flujo.

## Riesgos restantes

- Sin transacción distribuida entre colecciones, sigue existiendo ventana de inconsistencia ante caída total del proceso en momentos críticos.
- La compensación depende de disponibilidad de MongoDB en el momento del rollback.
- Falta aún explotación visual completa en UI para diferenciar todos los estados en listados/reportes.
