# Fase 2 — Propuesta de estructura técnica profesional (WinForms + MongoDB)

Fecha: 2026-04-09

## Objetivo
Definir una arquitectura escalable y mantenible para evolucionar el sistema POS de ferretería sin abandonar Windows Forms.

## Estructura objetivo

```text
SistemaFerreteriaV8/
  UI/
    Forms/
    Controls/
    Presenters/
  AppCore/
    Abstractions/
    Security/
    Sales/
    Inventory/
    Reporting/
  Domain/
    Entities/
    Security/
    ValueObjects/
    Policies/
  Infrastructure/
    Persistence/
      Mongo/
      Repositories/
    Security/
    Printing/
    Integrations/
  Shared/
    Logging/
    Validation/
    Result/
```

## Principios de diseño aplicados
- **SRP**: cada clase con responsabilidad única.
- **DIP**: la UI depende de interfaces de AppCore, no de MongoDB.
- **Open/Closed**: reglas de negocio extensibles por servicios.
- **Fail-safe defaults**: seguridad por defecto en acciones críticas.
- **Observabilidad**: trazabilidad en caja, facturación y cierre.

## Responsabilidades por capa

- **UI / Forms**
  - Renderizar pantallas, capturar interacción del usuario.
  - No contener reglas de negocio ni acceso directo a Mongo.
- **Application**
  - Orquestar casos de uso (login, apertura/cierre de caja, venta, ajustes de precios, reportes).
  - Validar reglas de flujo y permisos por operación.
- **Domain**
  - Modelos y reglas puras del negocio ferretero.
  - Roles, permisos, resultados y políticas de seguridad.
- **Infrastructure**
  - Repositorios MongoDB, seguridad criptográfica, impresión, integraciones externas.
- **Shared/Helpers**
  - Logging, validación reusable, manejo estándar de errores/resultados.

## Flujos de alto impacto (target)

### 1) Autenticación y autorización
`Form` → `IAuthenticationService` → `IEmployeeRepository` → MongoDB

- Reemplazar validaciones ad-hoc por servicio único.
- Migrar contraseña en texto plano a hash PBKDF2.
- Introducir validación central de permisos por operación.

### 2) Venta en caja
`VentanaVentas` → `SalesService` → `FacturaRepository`, `ProductosRepository`, `CajaService`

- La UI no debe tocar colecciones Mongo directamente.
- Reglas de descuentos y validaciones en capa AppCore/Domain.
- Operaciones críticas con consistencia transaccional cuando aplique.

### 3) Inventario y stock
`VentanaProductos` / `VentanaInventario` → `InventoryService` → repositorios

- Registrar entradas/salidas en bitácora (kardex).
- Alertas de stock bajo centralizadas.

## Migración incremental propuesta

### Etapa A (rápida, bajo riesgo)
1. Agregar interfaces y servicios base de seguridad.
2. Introducir `PasswordHasher` con PBKDF2.
3. Agregar `EmployeeRepository` y empezar a encapsular acceso.

### Etapa B (núcleo de negocio)
1. Extraer casos de uso de ventas a `SalesService`.
2. Reducir tamaño de `VentanaVentas` delegando reglas.
3. Crear repositorios de facturas/productos con contratos claros.

### Etapa C (operación y soporte)
1. Logging estructurado y auditoría de acciones sensibles.
2. Manejo uniforme de errores (sin `catch` silenciosos).
3. Normalizar validaciones de entrada y mensajes de UX.

## Entregables incluidos en esta fase
- Se creó el **esqueleto inicial de capas** dentro del proyecto actual:
  - `AppCore/Abstractions`
  - `AppCore/Security`
  - `Domain/Security`
  - `Infrastructure/Security`
- Se incorporaron contratos y clases base para seguridad:
  - `IAuthenticationService`, `IEmployeeRepository`, `IPasswordHasher`
  - `AuthenticationService`
  - `Pbkdf2PasswordHasher`
  - `AuthResult`, `SystemRole`

## Módulos candidatos para refactor inmediato

1. **Login / autenticación / apertura de caja** (`VentanaRegistroCaja`, `Form1`, `VentanaPrecio`).
2. **Ventas en caja** (`VentanaVentas`) por tamaño y concentración de lógica.
3. **Datos de productos** (`Productos`) por creación de índices y rutas sync/async mezcladas.
4. **Configuración de instancia** (`AppInstanceSettings`) por sensibilidad de `mongo_uri`.

## Qué no se cambia aún
- No se modifica la UI de forma destructiva.
- No se elimina aún la autenticación legacy de formularios existentes.
- No se altera el flujo operacional diario mientras se hace la migración.

## Criterio de aceptación de Fase 2
- Arquitectura propuesta documentada y validada.
- Contratos base listos para conectar formularios en Fase 3.
- Sin ruptura funcional en el sistema actual.
