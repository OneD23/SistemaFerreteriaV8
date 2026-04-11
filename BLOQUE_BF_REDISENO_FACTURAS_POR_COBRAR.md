# Bloque BF — Rediseño modal VentanaFacturasPorCobrar

Fecha: 2026-04-11

## Problemas visuales detectados
- Ventana funcional pero con baja integración visual al sistema.
- Botonera inferior con exceso de rojo y sin jerarquía de acciones.
- Encabezado sin contexto operativo claro.
- Tabla usable pero sin suficiente protagonismo administrativo.

## Cambios aplicados
- Encabezado reforzado con título más claro y subtítulo contextual dinámico.
- DataGridView modernizado y priorizado como zona principal de trabajo.
- Barra inferior reorganizada con distribución uniforme de acciones.
- Reasignación semántica de colores:
  - Abrir factura = primario
  - Buscar por ID = secundario
  - Reimprimir = secundario
  - Cancelar = peligro
- Espaciado y tamaños consistentes para botones y zonas.
- Mantenimiento de flujo actual (abrir/buscar/reimprimir/cancelar) sin tocar lógica de negocio.

## Impacto de usabilidad
- Mayor claridad de propósito al entrar en la modal.
- Menor fricción para localizar y abrir facturas pendientes.
- Percepción de herramienta administrativa más seria y coherente con el resto del sistema.
