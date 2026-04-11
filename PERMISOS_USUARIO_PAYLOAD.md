# Payload técnico para futura UI de permisos por usuario

## Modelo de lectura (snapshot)
`UserPermissionSnapshot`
- `employeeId`
- `employeeName`
- `rolePermissions[]`
- `allowOverrides[]`
- `denyOverrides[]`
- `effectivePermissions[]`

## Modelo de escritura (overrides)
`SetOverridesAsync(employeeId, allowPermissions, denyPermissions)`

Reglas de validación recomendadas:
1. No permitir valores vacíos o duplicados.
2. Normalizar case (minúsculas) antes de persistir.
3. Si un permiso existe en allow y deny, prevalece deny.
4. Validar permisos contra catálogo conocido (`AppPermissions`).

## Flujo previsto para UI futura
1. Buscar empleado.
2. Leer snapshot efectivo (`GetSnapshotAsync`).
3. Editar allow/deny.
4. Confirmar cambios y guardar (`SetOverridesAsync`).
5. Mostrar permisos efectivos recalculados.

## Compatibilidad
- Si no existen overrides por usuario, el sistema conserva permisos heredados por rol.
- La migración es no disruptiva para usuarios actuales.
