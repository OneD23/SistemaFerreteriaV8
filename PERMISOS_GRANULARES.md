# Permisos granulares por acción (Fase 3)

## Catálogo mínimo viable implementado

- `ventas.crear`
- `ventas.descuento`
- `ventas.cambiar_precio`
- `ventas.cancelar`
- `caja.abrir`
- `caja.cerrar`
- `inventario.editar`
- `inventario.ajustar_stock`
- `clientes.editar`
- `configuracion.editar`
- `empleados.gestionar`
- `reportes.ver`

Fuente de verdad: `Domain/Security/AppPermissions.cs`.

## Matriz de transición rol -> permisos

- **Administrador**: todos los permisos.
- **Cajera**: `ventas.crear`, `ventas.descuento`, `caja.abrir`, `caja.cerrar`, `reportes.ver`.
- **Inventario**: `inventario.editar`, `inventario.ajustar_stock`, `reportes.ver`.
- **Contabilidad**: `reportes.ver`, `ventas.cancelar`, `caja.cerrar`.

Fuente de verdad: `AppCore/Security/AuthorizationService.cs`.

## Dónde se aplica actualmente

- `VentanaRegistroCaja`: valida `caja.abrir` antes de iniciar apertura de caja.
- `Form1`:
  - Productos: `inventario.editar`
  - Usuarios/empleados: `empleados.gestionar`
  - Contabilidad: `reportes.ver`
  - Configuración: `configuracion.editar`
  - Cierre de caja: `caja.cerrar`
- `VentanaVentas`:
  - Registrar/cobrar venta: `ventas.crear`
  - Ajuste de descuento/cantidad: `ventas.descuento`
  - Edición de factura cargada: `ventas.cancelar`
- `VentanaPrecio`: cambio de precio por columna sensible: `ventas.cambiar_precio`
- `Usuarios`:
  - Gestión de clientes: `clientes.editar`
  - Gestión de empleados: `empleados.gestionar`
- `VentanaConfiguraciones`: acceso de pantalla: `configuracion.editar`

## Servicio central de validación

La autorización ya no se valida de forma dispersa en cada form. Se centraliza con:
- `IAuthorizationService` + `AuthorizationService`
- `PermissionAccess.EnsurePermissionAsync(...)`
- `SecurityPrompt` para elevación controlada cuando el usuario activo no tiene permiso

## Pendientes de migración

- Expandir uso de permisos a inventario avanzado, reportes detallados, anulaciones y flujos de caja secundarios.
- Guardar permisos explícitos por usuario (además de rol) para romper dependencia del rol fijo.
