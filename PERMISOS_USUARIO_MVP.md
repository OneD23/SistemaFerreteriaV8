# Permisos por usuario (MVP) — transición desde permisos por rol

## Problema
La autorización por acción ya existe, pero dependía principalmente de la matriz de rol.

## Diseño aplicado (compatible)
Se agregan overrides por usuario en `Empleado`:
- `permisosAllow`: permisos explícitamente otorgados
- `permisosDeny`: permisos explícitamente denegados

## Prioridad de evaluación
1. **Deny por usuario** (máxima prioridad)
2. **Allow por usuario**
3. **Permisos heredados por rol**
4. Denegado por defecto

## Implementación actual
- `Empleado` ahora incluye `PermisosAllow` y `PermisosDeny`.
- `AuthenticationService` y `PermissionAccess.BuildSession` incluyen estos overrides al construir `AuthResult`.
- `AuthorizationService.HasPermission(...)` aplica la prioridad definida.
- `IEmployeeRepository` incluye escritura de overrides (`SetUserPermissionOverridesAsync`).
- `IUserPermissionService`/`UserPermissionService` permiten consultar snapshot efectivo y preparar futura UI administrativa.

## Estrategia de migración gradual
1. Mantener matriz actual por rol para no romper operación.
2. Habilitar overrides por usuario solo donde haga falta.
3. Incorporar UI administrativa para editar permisos por usuario en un siguiente sprint.
4. Auditar permisos denegados/otorgados antes de retirar dependencias de rol en módulos críticos.
