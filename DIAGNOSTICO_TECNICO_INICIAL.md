# Diagnóstico técnico inicial (Fase 1) — SistemaFerreteriaV8

> Fecha de auditoría: 2026-04-09
> Alcance: revisión estática inicial de arquitectura, seguridad, rendimiento, mantenibilidad y experiencia de uso en WinForms.

## 1) Módulos principales identificados

### UI / Operación diaria
- `Form1`: shell principal y navegación de módulos.
- `VentanaVentas`: flujo de caja/ventas (facturación, descuentos, cliente, registro de productos).
- `VentanaProductos`, `VentanaInventario`, `VentanaCliente`, `VentanaEmpleado`.
- `VentanaFacturas`, `VentanaFactura`, `VentanaFacturasPorCobrar`, `VentanaPagar`, `VentanaRegistroCaja`, `VentanaCierreCaja`, `VentanaReportes`, `VentanaConfiguraciones`.

### Dominio / datos (carpeta `Clases`)
- Entidades activas con acceso directo a MongoDB: `Factura`, `Productos`, `Empleado`, `Caja`, `Cuenta`, `Clientes`, etc.
- Soporte de impresión y reportes: `CreaTicket`, `Print`, `PrinterClass`, `Reportes`.
- Configuración runtime: `OneKeys`, `AppInstanceSettings`.
- Tematización visual: `ThemeManager`.

## 2) Hallazgos críticos (prioridad alta)

1. **Seguridad débil en autenticación y autorización**
   - Las contraseñas de empleados se consultan en texto plano por campo `contrasena`.
   - Se usa `Interaction.InputBox` para solicitar clave admin en múltiples flujos críticos.
   - Existe una **puerta trasera hardcodeada** (`"3322"`) que concede acceso administrador.
   - Impacto: escalamiento de privilegios, suplantación y riesgo de fraude en caja.

2. **Arquitectura acoplada (UI + negocio + datos mezclados)**
   - Formularios grandes controlan reglas de negocio, cálculos, acceso a MongoDB y UX simultáneamente.
   - `VentanaVentas` y `Factura` están sobredimensionados y concentran demasiadas responsabilidades.
   - Impacto: alta probabilidad de regresiones, difícil testing y alto costo de cambio.

3. **Persistencia sin capa de aplicación (patrón Active Record informal)**
   - Entidades de dominio crean su propio `MongoClient`, ejecutan CRUD y lógica funcional.
   - No hay repositorios/servicios definidos por interfaz ni inyección de dependencias.
   - Impacto: baja testabilidad, acoplamiento técnico y poca observabilidad.

4. **Gestión de configuración sensible mejorable**
   - URI de MongoDB y datos de instancia se guardan en `appsettings.ini` plano en `CommonApplicationData`.
   - No se observan credenciales cifradas ni estrategia de secretos por entorno.
   - Impacto: exposición de configuración sensible y despliegues poco gobernados.

5. **Riesgo de rendimiento por operaciones síncronas en momentos críticos**
   - Se observan consultas `.ToList()` y búsquedas por fila dentro de flujos de venta.
   - Parte de la lógica en formularios puede bloquear UX con colecciones grandes.
   - Impacto: lentitud en caja e inventario al crecer la data.

## 3) Hallazgos importantes (prioridad media)

1. **Carga técnica por tamaño de archivos y complejidad**
   - `VentanaVentas.cs` (~1300 líneas), `Factura.cs` (~1100), `VentanaProductos.cs` (~700), `Reportes.cs` (~660).
   - Esto indica baja cohesión y falta de partición por casos de uso.

2. **Manejo de errores inconsistente**
   - Hay `catch` amplios y silenciosos en puntos de inicialización y conexión.
   - Sin logging estructurado, auditar incidentes en producción será difícil.

3. **Índices MongoDB parcialmente bien orientados, pero estrategia incompleta**
   - Se crean índices de `Factura` y `Productos`, pero algunos se disparan desde constructores.
   - Falta un bootstrap de índices único al inicio de aplicación.

4. **Modelo de roles simple (string)**
   - Reglas basadas en comparaciones de texto (`"Administrador"`, `"Cajera"`).
   - Sin catálogo de permisos granulares por módulo/acción.

5. **UX con fricción en flujos sensibles**
   - Uso intensivo de `InputBox` y popups para operaciones clave (edición, cierre, búsqueda).
   - Falta consistencia de validación y retroalimentación visual contextual.

## 4) Hallazgos positivos (base aprovechable)

- Proyecto ya en `.NET 8` WinForms (`net8.0-windows`).
- Existen esfuerzos de modernización visual (`ThemeManager`) y algunos métodos asíncronos.
- Se introdujeron índices en colecciones críticas de facturación/productos.
- Hay separación inicial por carpetas y clases de dominio reutilizables.

## 5) Diagnóstico por áreas del negocio ferretero

### Caja / ventas
- El flujo existe y es funcional, pero requiere endurecimiento de permisos, validación transaccional y desacople de UI.

### Inventario
- Buen punto de partida con métricas y agregaciones; falta robustecer entradas/salidas con trazabilidad (kardex y auditoría).

### Clientes / crédito / CxC
- Cobranza y cuentas por cobrar presentes; faltan reglas de antigüedad, alertas y bitácora de cambios.

### Reportes
- Hay capacidades de exportación y reportes; falta normalizar servicios de consulta y mejorar performance con datasets grandes.

### Usuarios/roles
- Es la brecha principal de seguridad del producto para venta comercial.

## 6) Recomendación ejecutiva inmediata (sin romper WinForms)

1. **Bloquear riesgos críticos en 1er sprint**
   - Eliminar backdoor `3322`.
   - Migrar contraseñas a hash seguro (Argon2id o PBKDF2 fuerte + salt único).
   - Introducir servicio central de autenticación/autorización.

2. **Reorganizar por capas manteniendo WinForms**
   - `UI` (Forms) → `Application` (casos de uso) → `Domain` (reglas) → `Infrastructure` (Mongo, impresión, archivos).

3. **Crear base de observabilidad**
   - Logging estructurado (archivo + rotación), trazas de errores y auditoría de acciones críticas.

4. **Optimizar rendimiento de caja e inventario**
   - Quitar consultas repetidas por fila, usar cargas paginadas y caché corta para catálogos.

## 7) Próximo paso sugerido (Fase 2)

Preparar propuesta técnica detallada de reestructura (carpetas/proyectos, interfaces, servicios, plan de migración incremental por módulos), con foco en:
- Seguridad primero.
- Cero downtime funcional.
- Mejoras visibles de UX en caja e inventario desde el primer release.

