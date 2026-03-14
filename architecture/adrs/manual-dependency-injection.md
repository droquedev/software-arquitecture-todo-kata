# Inyección de Dependencias Manual

## Estado

Aprobado

## Contexto

La app usa inyección de dependencias para que las capas no se conozcan entre sí y sea fácil cambiar implementaciones, por ejemplo al correr tests. La manera en que armamos y conectamos los objetos afecta qué tan fácil es leer el código y cuánta complejidad tiene.

Las opciones que se evaluaron fueron:

1. **Contenedor de DI de .NET** (`Microsoft.Extensions.DependencyInjection`): registras los servicios y él solo los resuelve.
2. **Composición manual (Pure DI)**: tú mismo armas todo a mano en `Program.cs`, sin ayuda de ningún contenedor.

## Decisión

Se decidió armar todo a mano con **Pure DI** en `Program.cs`. Cada dependencia — repositorios, comandos, consultas y casos de uso — se crea y se pasa directamente al constructor del objeto que la necesita, sin ningún contenedor de por medio.

## Consecuencias

**Positivo:**
- Todo el grafo de dependencias queda en un solo lugar (`Program.cs`), lo que lo hace perfecto para aprender: no hay ninguna "magia" detrás.
- El compilador te avisa si falta alguna dependencia; los errores se ven al compilar, no cuando ya está corriendo.
- No se agrega ninguna dependencia extra al proyecto.
- Queda muy claro quién depende de quién.

**Negativo:**
- Mientras más clases se agreguen al proyecto, `Program.cs` va a crecer y puede volverse difícil de leer.
- Hay cosas que otros proyectos resuelven solos de forma automática, pero aquí tendrías que programarlas a mano. Por ejemplo, si quisieras escribir en un log cada vez que se llama a cualquier caso de uso, tendrías que agregarlo en cada clase por separado, en lugar de definirlo una sola vez en un lugar central.
