# Bloque AQ — Modernización de VentanaCierreCaja

## Antes
- El cierre de caja funcionaba, pero la jerarquía visual de montos/estado era limitada.
- El feedback dependía principalmente de MessageBox.
- Navegación por teclado poco optimizada para operación rápida.

## Después
- Se moderniza visualmente `VentanaCierreCaja` con:
  - botones estandarizados,
  - tabla de facturas con mejor legibilidad,
  - jerarquía visual en montos clave (esperado/reportado/resultado).
- Se agrega feedback inline consistente con `lblEstado`.
- Se optimiza teclado:
  - `Enter` ejecuta cierre,
  - `Esc` cierra la ventana.
- Se resalta diferencia/cuadre con color semántico para lectura rápida.

## Impacto técnico y funcional
- Mejor lectura operativa del cierre de caja en turnos de alto tráfico.
- Menor fricción para cierre por teclado.
- Mayor claridad entre estado OK vs descuadre sin depender exclusivamente de diálogos.

## Riesgos pendientes
- Prompt de balance inicial/final sigue siendo `InputBox` (se puede migrar a campo embebido en próxima iteración).
- Falta un componente de estado compartido entre formularios para reutilizar estilos de feedback.
