# Bloques AZ, BA, BB, BC — Flujo real de caja y pulido final

Fecha: 2026-04-11

## Ventana de entrada priorizada
`VentanaVentas`

## Ajustes implementados

### AZ — Flujo continuo de caja
- Se añadió manejo consistente de `Escape` para cerrar el panel de búsqueda rápida y devolver foco al campo de código (`Id`).
- Al seleccionar producto desde resultados, el foco vuelve automáticamente a `Id` para continuar escaneo/captura sin pasos extra.
- En agregado rápido por Enter, se reutiliza la información de la fila seleccionada para feedback inmediato.

### BA — Micro-optimizaciones
- Se introdujo debounce de búsqueda (`_searchDebounceTimer`) para evitar consultas en cada pulsación del usuario.
- Se añadió control de versión de consulta (`_searchRequestVersion`) para descartar respuestas atrasadas y evitar repintados inconsistentes.
- Se eliminó una consulta redundante (`FindByCodeAsync`) en el flujo de agregar primer producto filtrado.

### BB — Detalles finales de UX
- Se unificó feedback de validación de dirección de envío usando estado inline + foco directo al campo `direccion`.
- Se mantuvo consistencia de teclado con `KeyPreview` habilitado para atajos de interacción rápida.

## Validación operativa (BC)
- Flujo continuo revisado en código para:
  - búsqueda rápida,
  - agregado repetido,
  - retorno de foco para ciclo de cobro.
- Queda pendiente validación manual end-to-end en entorno con runtime para cierre completo del checklist BC.

## Pendientes finales
- Ejecutar prueba de carga manual de ventas continuas en caja real.
- Validar escenarios de red intermitente durante consultas de producto y persistencia de venta.
