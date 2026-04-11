# Bloque X — Consolidación de CashRegisterService

## Problema que resuelve (antes)
- Apertura y cierre dependían parcialmente de validaciones en formularios.
- No había detección explícita de estados inconsistentes (múltiples cajas abiertas).
- El cierre calculaba datos fuera del servicio, dejando lógica financiera dispersa.

## Cambios aplicados (después)
- `CashRegisterService` ahora centraliza:
  - consulta de caja activa (`GetActiveAsync`),
  - validación de montos,
  - apertura robusta (`OpenAsync`),
  - previsualización de cierre (`BuildClosePreviewAsync`),
  - cierre final (`CloseAsync`).
- Se agregan resultados estructurados con:
  - `ErrorType`,
  - `ExpectedBalance`,
  - `ReportedBalance`,
  - `Difference`,
  - `OperationId`,
  - facturas relacionadas para cierre.
- Se registran eventos de auditoría de caja (`cash_register.*`) sin romper operación si falla auditoría.
- `VentanaRegistroCaja` y `VentanaCierreCaja` delegan más en el servicio y reducen cálculo local.

## Impacto técnico
- Menos acoplamiento entre UI y reglas de caja.
- Mayor trazabilidad operativa para apertura/cierre.
- Menor riesgo de aperturas duplicadas invisibles y cierres sobre estado inconsistente.

## Impacto funcional
- Usuarios reciben mensajes más precisos ante conflictos de caja.
- Cierre usa un resumen centralizado y consistente desde servicio.
- Base preparada para integrar movimientos de caja en etapas siguientes.

## Riesgos pendientes
- Aún no existe índice único parcial en Mongo para forzar por BD "solo una caja abierta".
- Falta pantalla administrativa para resolver automáticamente estados inconsistentes.
