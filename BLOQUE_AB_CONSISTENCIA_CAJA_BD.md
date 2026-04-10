# Bloque AB — Consistencia fuerte de caja en base de datos

## Problema que resuelve
- La restricción de "no más de una caja abierta" existía solo en lógica de servicio.
- En concurrencia o múltiples procesos, podían colarse aperturas simultáneas.

## Regla de persistencia definida
- **Regla elegida:** una sola caja abierta global (`estado == "true"`) para toda la operación.
- Implementación: índice único parcial en MongoDB sobre `estado` filtrado por `estado == "true"`.

## Cambios aplicados
- `Caja` ahora intenta crear índice `idx_caja_single_open_global` en inicialización estática.
- `CashRegisterService.OpenAsync` ahora captura `DuplicateKey` y lo traduce a resultado estructurado `DuplicateOpen`.
- La validación en servicio queda alineada con la restricción real de base de datos.

## Concurrencia: cubierto vs no cubierto
### Cubierto
- Dos aperturas concurrentes en procesos distintos: solo una inserción puede ganar por la restricción única parcial.
- El perdedor recibe error `DuplicateKey` y el servicio responde con mensaje operativo seguro.

### No cubierto
- Si existen datos legados con múltiples cajas abiertas, la creación del índice puede fallar hasta saneamiento de datos.
- Caídas de red o indisponibilidad de Mongo pueden impedir aplicar/verificar la restricción en runtime.

## Impacto técnico y funcional
- Mayor garantía de consistencia real en persistencia.
- Menor riesgo de estado inválido por carrera en apertura de caja.
- Se mantiene compatibilidad porque el servicio conserva detección de inconsistencias legadas.
