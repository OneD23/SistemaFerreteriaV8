# Bloques AI + AJ + AK + AL (iteración inicial en VentanaVentas)

## Antes
- La pantalla de ventas funcionaba, pero tenía estilos mixtos, poca señalización visual de acciones críticas y flujo de búsqueda/selección no optimizado para caja rápida.
- El feedback de errores/éxitos dependía casi siempre de `MessageBox`, generando fricción operativa.

## Después
### AI — Modernización visual funcional
- Botones críticos estandarizados (cobrar, venta rápida, cancelar, guardar) con estilos consistentes.
- Tablas (`ListaDeCompras`, `ListaProductos`) con cabeceras y selección más legibles.
- Se mantiene diseño WinForms simple sin sobrecargar interfaz.

### AJ — Optimización del flujo de ventas
- Búsqueda rápida por nombre ahora permite **Enter para agregar el primer resultado**.
- Flujo de caja más ágil: buscar -> Enter -> agregar -> foco vuelve al campo de escáner (`Id`).
- Se reducen clics para operación repetitiva de mostrador.

### AK — Robustez operativa
- Se añade feedback visual inline (`Aviso`) con temporizador para éxito/error.
- Fallos de persistencia ahora también notifican en estado inline además de `MessageBox`.
- Mejora de estado de UI sin bloquear siempre con diálogo modal.

### AL — Limpieza legacy (alcance de esta iteración)
- Continuamos priorizando rutas críticas de ventas para desacoplar operaciones; quedan módulos administrativos legacy para siguientes bloques.

## Riesgos pendientes
- Aún existen formularios legacy fuera de ventas que requieren estandarización visual adicional.
- Persisten accesos directos a inventario en módulos administrativos (`VentanaProductos`, `VentanaInventario`).
