# Arquitectura

Documentación de las decisiones de arquitectura y ejemplos de degradación del proyecto Todo List.

## Decisiones de Arquitectura (ADRs)

| # | Decisión | Estado |
|---|---|---|
| 1 | [Arquitectura en Capas](./adrs/arquitectura-por-capas.md) | Aprobado |
| 2 | [Inyección de Dependencias Manual](./adrs/inyeccion-manual-de-dependencias.md) | Aprobado |

## Degradaciones

Ejemplos de cómo la arquitectura se puede romper con el tiempo si no se tienen cuidado.

| Degradación | Descripción corta |
|---|---|
| [Lógica de negocio en la capa App](./degradaciones/logica-en-app.md) | Meter validaciones en la consola en lugar de en el comando |
| [Repositorio en Use Case](./degradaciones/repositorio-en-usecase.md) | Acceder directo a datos desde un use case saltándose la capa Cqrs |
