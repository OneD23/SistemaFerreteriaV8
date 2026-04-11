# Ranking de próximos servicios a extraer (impacto/riesgo)

## 1) CashRegisterService (Impacto Muy Alto / Riesgo Medio)
- Apertura/cierre de caja, balances y auditoría operativa.
- Alto riesgo de inconsistencias financieras si se mantiene en UI.

## 2) ProductService (Impacto Alto / Riesgo Medio)
- Validaciones de stock, ajustes, precios y reglas de inventario.
- Reduce errores de inventario y duplicación entre formularios.

## 3) EmployeeService (Impacto Alto / Riesgo Medio)
- Gestión de usuarios, roles y permisos por usuario (allow/deny).
- Clave para cerrar gobernanza de seguridad.

## 4) ConfigurationService (Impacto Medio / Riesgo Bajo)
- Centralizar settings de negocio/instancia y validaciones de configuración.
- Facilita despliegue, soporte y hardening.

## 5) ReportingService (Impacto Medio / Riesgo Bajo)
- Consolidar consultas y agregaciones de reportes fuera de UI.
- Mejora mantenibilidad/performance en reportes complejos.

## Recomendación de orden
1. CashRegisterService
2. ProductService
3. EmployeeService
4. ConfigurationService
5. ReportingService
