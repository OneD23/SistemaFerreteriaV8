# Bloque AF — Consolidación final de ProductService (iteración actual)

## Problema que resuelve
- Aún quedaban accesos directos a inventario/producto fuera de `ProductService`, incluyendo rutas de ajuste de stock en facturas y consultas directas en formularios legacy.

## Accesos eliminados en esta iteración
- `Factura.ReponerInventarioPorEliminacionAsync`:
  - migrado de edición directa de `Productos` a `AppServices.Product.AdjustStockAsync`.
- `Factura.RegistrarProductosAsync`:
  - migrado de manipulación directa de `Cantidad/Vendido` a `AdjustStockAsync`.
- `VentanaFactura.RegistrarFactura`:
  - lookup por nombre migrado de `new Productos().Buscar(...)` a `AppServices.Product.FindByNameAsync`.

## Estado de consolidación
- Ventas y compensación principal ya dependen de `ProductService`.
- Flujos de reposición y registro de productos en factura se alinean ahora con el servicio.

## Pendientes legacy identificados
- `VentanaProductos`: múltiples operaciones CRUD/inventario directas sobre `Productos`.
- `VentanaInventario`: listados y métricas directas sobre `Productos`.
- `VentanaEstadisticas`: accesos directos para productos más vendidos/bajo stock.

## Riesgos pendientes
- Persisten rutas legacy con acceso directo en formularios administrativos.
- Faltan pruebas automatizadas de regresión para ajustes de stock post-factura.
