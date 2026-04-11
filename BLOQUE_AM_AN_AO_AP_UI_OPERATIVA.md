# Bloques AM + AN + AO + AP (iteración incremental)

## Problema abordado
- Formulario de factura y apertura de caja aún tenían estilos/feedback menos consistentes que ventas.
- Flujo por teclado y estados operativos podían mejorarse para uso intensivo.

## Cambios aplicados
### AM — Modernización visual funcional
- `VentanaFactura`:
  - estandarización visual de botones y tabla.
  - tipografía y contraste más consistentes.
- `VentanaRegistroCaja`:
  - mantiene estilo base y agrega estado inline visible.

### AN — Navegación y foco por teclado
- `VentanaFactura`:
  - `Esc` cierra ventana.
  - `Enter` en `IdFactura` dispara actualización.
- `VentanaRegistroCaja`:
  - `Enter` intenta iniciar sesión de caja.
  - `Esc` cierra ventana.

### AO — Feedback visual y estados
- `VentanaFactura`:
  - estado inline para actualización/eliminación/errores de búsqueda.
- `VentanaRegistroCaja`:
  - estado inline para errores de autenticación/permiso/validación y éxito de inicio.

### AP — Limpieza legacy incremental
- Se priorizan formularios operativos de alto uso (factura y caja) sin romper compatibilidad.
- Se deja pendiente limpieza adicional en formularios administrativos más legacy.

## Riesgos pendientes
- Aún hay formularios con estilos heterogéneos fuera de este alcance.
- Falta un componente visual común reutilizable para estados inline entre todas las ventanas.
