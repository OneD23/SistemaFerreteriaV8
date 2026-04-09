# Estado de atomicidad: guardar venta/factura + descontar stock

## Qué quedó centralizado
- Persistencia de factura (insert/update)
- Descuento de stock por línea no genérica
- Resultado estructurado (`SalesWorkflowResult`) en un solo método de alto nivel (`ConfirmSaleAsync`)

## Estrategia actual ante fallos parciales
- Si falla descuento de stock después de persistir factura:
  - se intenta compensación best-effort de stock para productos ya tocados;
  - la factura se marca con `Estado = stock_error` e `Informacion`.

## Limitaciones actuales
- Sin transacción Mongo multi-documento garantizada en este punto.
- Compensación best-effort puede no recuperar todos los escenarios extremos (caída de red/proceso).

## Próximo paso recomendado
1. Evaluar soporte de transacciones (replica set) y versión de driver.
2. Si no es viable, implementar patrón outbox/cola de compensación para consistencia eventual auditable.
3. Agregar bitácora de eventos de venta/stock para trazabilidad de recuperación.
