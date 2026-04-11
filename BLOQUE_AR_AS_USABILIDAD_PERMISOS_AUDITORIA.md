# Bloques AR + AS — Usabilidad en Permisos y Auditoría

Fecha: 2026-04-11

## Objetivo
Reducir fricción operativa en dos ventanas de uso frecuente:

- `VentanaPermisosUsuario`: edición segura de overrides por usuario.
- `VentanaAuditoriaConsulta`: lectura más rápida y navegación de filtros.

## Cambios aplicados

### AR — VentanaPermisosUsuario
- Se añadió indicador explícito de **cambios pendientes** para evitar cierres con sensación de trabajo no guardado.
- Se incluyó botón **Cerrar** y atajos:
  - `Ctrl+S` para guardar.
  - `Ctrl+W` para cerrar.
- Se mejoró el ciclo de carga/sincronización para que los checks cargados desde snapshot no marquen falsos pendientes.
- Se configuró `AcceptButton` y `CancelButton` para flujo de teclado.

### AS — VentanaAuditoriaConsulta
- Se añadió botón **Limpiar filtros**.
- Se agregó resumen de resultados cargados (`Resultados: N`).
- Se mejoró la interacción de grilla (`FullRowSelect`, selección inicial de primera fila).
- Se añadieron atajos:
  - `Ctrl+F` para ir al filtro de usuario.
  - `F5` para refrescar consulta.
- Se inicializa el panel de filtros con defaults útiles al limpiar (últimos 7 días, límite 300).

## Riesgos/Notas
- No se altera lógica de negocio de autorización ni consulta de auditoría; los cambios son de UX y navegación.
- En próximos bloques conviene añadir confirmación de salida si hay pendientes sin guardar en permisos.
