# Roadmap: Sistema de Ventas Profesional y Avanzado

## Objetivo
Transformar el sistema actual en una plataforma de ventas robusta, medible y auditable, con foco en rendimiento, UX y reportes ejecutivos.

## Fase 1 (0-30 días) — Estabilidad y rendimiento
- [x] Paginación de facturas en UI.
- [x] Índices MongoDB para consultas frecuentes (`Id`, `Fecha`, `NombreCliente`).
- [ ] Logging centralizado de errores.
- [ ] Telemetría básica de tiempos de respuesta por pantalla.
- [ ] Validación de datos de entrada (cliente, pagos, montos).

## Fase 2 (30-60 días) — Operación profesional
- [ ] Dashboard ejecutivo (ventas por día, ticket promedio, mix de pagos, top clientes).
- [ ] Cierre de caja auditable con diferencias y responsable.
- [ ] Gestión de cuentas por cobrar con antigüedad de saldos.
- [ ] Reglas de descuentos por rol/permiso.
- [ ] Bitácora de cambios en facturas (quién, cuándo, qué cambió).

## Fase 3 (60-90 días) — Reportes avanzados y BI
- [x] Reporte PDF con KPIs y top clientes.
- [x] Exportación CSV para análisis en Excel/Power BI.
- [ ] Reportes por vendedor/categoría/marca.
- [ ] Proyección de demanda y alertas de inventario bajo.
- [ ] Indicadores de margen bruto/neto por período.

## Fase 4 (90+ días) — Escala y gobierno
- [ ] Backups automáticos + verificación de restauración.
- [ ] Gestión multi-sucursal y consolidado corporativo.
- [ ] Control de acceso por permisos finos.
- [ ] Pipeline CI/CD con pruebas automáticas.
- [ ] Manual operativo y guía de soporte.

## KPIs sugeridos (mínimos)
- Ventas diarias / semanales / mensuales.
- Ticket promedio.
- Conversión de cotización a factura.
- Margen por categoría.
- % ventas a crédito y mora > 30 días.
- Rotación de inventario.

## Entregables técnicos recomendados
1. `DashboardVentasService` para concentrar métricas.
2. `ReporteVentasExporter` (PDF/CSV/XLSX).
3. `CajaAuditService` para trazabilidad de cierres.
4. `CreditoService` para gestión de CxC y cobros.
5. `ErrorLogger` + `ActivityLog` para observabilidad.
