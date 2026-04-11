# Bloque BG — Rediseño Lista de Envíos (WinForms)

Fecha: 2026-04-11

## Problemas visuales detectados
- Espacio muerto en cabecera y zona inferior.
- Tabla sin suficiente protagonismo frente al resto de controles.
- Botones inferiores sin jerarquía clara.
- Falta de contexto de estado para el operador.

## Cambios aplicados
- Se convirtió la tabla en el foco principal con mejor ocupación dinámica en resize.
- Se reforzó cabecera con título claro + contexto operacional (`_lblContexto`).
- Botonera inferior alineada y homogénea:
  - Registrar entrega (primario)
  - Cerrar (secundario)
- Se aplicó el sistema visual global (`UiConsistencia`) en formulario, grilla y botones.
- Se mejoró espaciado y distribución general para una ventana más compacta y profesional.

## Impacto
- Mejor lectura y foco en pendientes de entrega.
- Menos fricción para completar la acción principal.
- Pantalla visualmente consistente con el resto del sistema.
