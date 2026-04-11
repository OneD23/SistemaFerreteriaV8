# Bloque AE — Consulta operativa de auditoría

## Problema que resuelve
- Se registraban eventos en auditoría, pero no existía una forma práctica de consulta dentro del sistema.

## Solución implementada
- Se amplía `IAuditService` con contrato de lectura:
  - `QueryAsync(AuditQuery request)`
- Se define `AuditQuery` con filtros mínimos operativos:
  - fecha desde/hasta
  - usuario
  - módulo
  - tipo de evento
  - operationId
  - límite de resultados
- Se implementa consulta en `AuditService` con filtros combinables y orden por fecha descendente.
- Se agrega pantalla WinForms `VentanaAuditoriaConsulta`:
  - filtros simples en la parte superior,
  - tabla clara de resultados,
  - extracción básica de `operationId` para lectura rápida.
- Se integra acceso desde `Usuarios` con botón `Auditoría` protegido por permiso `reportes.ver`.

## Impacto técnico y funcional
- Soporte y operación pueden consultar eventos sin ir directamente a MongoDB.
- Mejora diagnóstico de incidentes con filtros mínimos útiles.
- La solución mantiene complejidad baja y enfoque práctico.

## Riesgos pendientes
- `operationId` se extrae de `MetadataJson` por parsing simple; puede mejorarse con parser JSON robusto.
- Falta exportación de resultados (CSV/Excel) para soporte extendido.
