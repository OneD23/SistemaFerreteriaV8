# Bloque BE — Rediseño POS de VentanaVentas / Facturación

Fecha: 2026-04-11

## Problemas visuales detectados (antes)
1. Exceso de acciones en rojo sin jerarquía clara.
2. Bloques clave (búsqueda/lista/totales) con mismo peso visual que acciones secundarias.
3. Distribución rígida por coordenadas, con sensación de paneles desconectados.
4. Acción principal de cobro poco dominante.

## Cambios aplicados
- Reorganización visual de la pantalla en bloques POS mediante layout dinámico (`SetBounds`) y resize-safe:
  - información de factura/cliente,
  - acciones,
  - lista de compra central,
  - observaciones/dirección,
  - resumen/totales.
- Jerarquía de acciones:
  - `Cobrar` más grande y destacado,
  - `Guardar/Obtener/Buscar` en tonos azules,
  - `Cancelar/Eliminar` en rojo,
  - `+1/-1` como acciones auxiliares discretas.
- Integración visual del checkbox `Para enviar`.
- Campo de escaneo (`Id`) y búsqueda por nombre (`NombreABuscar`) reforzados visualmente.
- Tipografía y estilo de totales mejorados, dando protagonismo al total final.
- Aplicación del helper común `UiConsistencia` en paneles, grillas, inputs y botones.

## Impacto en usabilidad
- Flujo de caja más legible y rápido.
- Menor carga cognitiva al distinguir acción principal vs secundarias.
- Lista de compra y búsqueda recuperan el protagonismo operativo de un POS.

## Compatibilidad
- Sin cambios de arquitectura.
- Sin alterar reglas de negocio de ventas.
- Navegación por teclado/foco conservada y reforzada con layout estable al redimensionar.
