# Arquitectura en Capas

## Estado

Aprobado

## Contexto

El proyecto es un todo app de aprendizaje. El objetivo es practicar principios de arquitectura de software. Se necesita una estructura que permita separar las responsabilidades entre la presentación, la lógica de negocio y la persistencia, facilitando la comprensión del flujo de dependencias y la evolución del código a lo largo de la vida del software.

Sin la separación por capas, el código tiende a acoplarse, dificultando las pruebas, el reemplazo de implementaciones y la comprensión de las responsabilidades de cada componente.

## Decisión

Se utiliza una arquitectura en capas con cuatro niveles, donde las dependencias son unidireccionales apuntando desde fuera hacia adentro:

```
App (Presentación / Punto de entrada)
  └── UseCases (Aplicación)
        └── Cqrs (Dominio: contratos, modelos, comandos, consultas)
              └── Infra (Infraestructura: implementaciones concretas)
```

Cada capa es un proyecto de C# independiente dentro de la solución:

- `ArchitectureKata.TodoList.App` — Interfaz de consola y composición de dependencias.
- `ArchitectureKata.TodoList.UseCases` — Acciones principales del sistema.
- `ArchitectureKata.TodoList.Cqrs` — Interfaces, modelos e implementaciones de comandos y consultas.
- `ArchitectureKata.TodoList.Infra` — Repositorios concretos (JSON sobre disco).

## Consecuencias

**Positivo:**
- Las responsabilidades de cada componente son explícitas y fáciles de seguir.
- Las capas internas (como: `Cqrs`) no dependen de las capas externas (como: `Infra`, `App`, `UseCase`), lo que permite testearlas de forma aislada.
- Facilidad al sustituir la capa de infraestructura (ejemplo: reemplazar JSON por una base de datos) sin tocar la lógica de negocio.

**Negativo:**
- Introduce indirección y proyectos adicionales que pueden parecer innecesarios para una aplicación de consola sencilla.
- La capa de `UseCases` carece actualmente de lógica, solo consume la capa `Cqrs`, agregando una capa más de indirección.