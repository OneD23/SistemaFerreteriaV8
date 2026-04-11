# Bloque AC — Expansión de ProductService

## Problema que resuelve
- Seguían existiendo accesos directos a `Productos` desde formularios/clases legacy.
- La búsqueda y validaciones de inventario no estaban completamente centralizadas.

## Cambios aplicados
- Se amplió el contrato `IProductService` con:
  - `FindByCodeAsync`
  - `SearchByNameAsync`
  - `GetStockSnapshotAsync`
  - `AdjustStockAsync`
- Se agregaron resultados estructurados para búsqueda, snapshot y ajuste:
  - `ProductSearchResult`
  - `StockSnapshotResult`
  - `StockAdjustmentRequest`
  - `StockAdjustmentResult`
- `ProductService` ahora centraliza:
  - búsqueda por código/id/nombre,
  - búsqueda por texto para UI,
  - snapshot de stock,
  - ajuste manual seguro de stock con validación de negativos.
- `VentanaVentas` se desacopló más de Mongo/`Productos`:
  - búsqueda dinámica usa `AppServices.Product.SearchByNameAsync`
  - selección desde resultados usa `FindByCodeAsync`
  - cambio de precio usa `FindByNameAsync`

## Accesos directos a inventario detectados (estado)
### Eliminados en este bloque
- `VentanaVentas.textBox1_TextChanged`: consulta Mongo directa de productos.
- `VentanaVentas.ListaProductos_CellContentClick`: `Productos.BuscarAsync` directo.
- `VentanaVentas.ListaDeCompras_CellDoubleClick`: `Productos.BuscarPorClaveAsync` directo.

### Pendientes (legacy / siguientes iteraciones)
- `VentanaProductos`: CRUD y ajustes directos con `new Productos()`.
- `VentanaInventario`: listados/reportes directos sobre `Productos`.
- `Factura.ReponerInventarioPorEliminacionAsync` y `RegistrarProductosAsync`: ajustes directos de stock.
- `VentanaFactura`: búsqueda directa de producto por nombre.

## Riesgos pendientes
- Parte de inventario sigue acoplado a modelo legacy `Productos`.
- Ajustes manuales de stock aún no están conectados a una UI administrativa unificada.
