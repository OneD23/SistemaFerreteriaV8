# Bloque AD — UI mínima de permisos por usuario

## Problema que resuelve
- Existía infraestructura de allow/deny, pero no había una pantalla práctica para operarla.

## Solución implementada (mínima y funcional)
- Nueva ventana `VentanaPermisosUsuario` con enfoque operativo:
  - búsqueda/listado de usuarios,
  - snapshot de permisos por rol,
  - permisos efectivos,
  - edición de `allow` y `deny` directos,
  - guardado de overrides.
- Validación de seguridad y consistencia:
  - acceso protegido por `AppPermissions.EmpleadosGestionar`,
  - validación para impedir el mismo permiso en allow y deny.
- Integración simple en `Usuarios`:
  - botón `Permisos Usuario` para abrir la pantalla.

## Impacto técnico y funcional
- Habilita administración real de overrides sin tocar base de datos manualmente.
- Reduce fricción operativa para soporte/administración.
- Mantiene complejidad visual baja y enfoque funcional.

## Riesgos pendientes
- Faltan acciones masivas (copiar permisos entre usuarios, plantillas por rol).
- La UI no incluye auditoría visual de cambios históricos de overrides.
