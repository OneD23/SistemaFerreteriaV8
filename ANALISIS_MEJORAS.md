# Análisis de cambios para mejorar

## Resumen ejecutivo
Se revisó el código de `SistemaFerreteriaV8` para detectar mejoras de mantenibilidad, estabilidad y rendimiento.

### Hallazgos prioritarios
1. **Evitar `async void` en lógica de datos** para mejorar manejo de errores y composición async.
2. **Separar UI (`MessageBox`) de clases de dominio/datos** para facilitar pruebas y reutilización.
3. **Reducir duplicación** en módulos de impresión (`CreaTicket` y `pri`) y lógica repetida en facturación.
4. **Centralizar manejo de excepciones/logging** para mejorar trazabilidad en producción.
5. **Controlar creación de índices MongoDB** para evitar trabajo repetitivo en cada instancia.

## Cambio aplicado en esta iteración

### 1) Corrección de firma async en eliminación de productos
- Se cambió `TaskEliminarAsync` de `async void` a `Task`.
- Beneficio: permite `await`, propagación de errores y pruebas más seguras.

## Oportunidades de mejora sugeridas (siguientes iteraciones)

### Arquitectura
- Introducir una capa de servicios (por ejemplo: `FacturaService`, `ProductoService`) y dejar formularios solo para interacción UI.
- Mover mensajes de usuario fuera de clases en `Clases/*` para respetar separación de responsabilidades.

### Calidad de código
- Unificar convenciones de nombres (`PascalCase`, evitar nombres ambiguos como `pri.cs`).
- Eliminar métodos no usados o documentar su propósito.
- Agregar análisis estático (Roslyn analyzers) y reglas en CI.

### Estabilidad
- Estandarizar `try/catch` con logging estructurado y mensajes de error consistentes.
- Evitar capturas de excepción vacías (en algunos módulos se silencian errores).

### Rendimiento
- Revisar creación de índices MongoDB: actualmente se invoca desde constructor de entidad; sería mejor en inicialización de app.
- Revisar consultas con filtros de texto/regex para paginación en colecciones grandes.

### Testabilidad
- Empezar pruebas unitarias por dominio:
  - cálculo de totales/ganancias;
  - reglas de comprobantes fiscales;
  - serialización/deserialización de productos y facturas.

## Checklist recomendado
- [ ] Implementar logger central.
- [ ] Crear capa de servicios.
- [ ] Extraer impresoras a una sola implementación.
- [ ] Agregar pipeline de build/test en CI.
- [ ] Definir roadmap de refactors por riesgo e impacto.
