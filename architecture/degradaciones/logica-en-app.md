# Degradación: Lógica de negocio en la capa App

## Qué pasaría

Alguien necesita validar que el título de una tarea no esté vacío antes de crearla. En lugar de hacerlo dentro de `CreateTaskCommand`, lo agrega directo en `TodoConsoleApp.cs` porque "es más rápido".

```csharp
// En TodoConsoleApp.cs — esto NO debería estar aquí
if (string.IsNullOrWhiteSpace(title))
{
    Console.WriteLine("El título no puede estar vacío.");
    return;
}
```

## Por qué es un problema

La validación ahora vive en la pantalla, no en el negocio. Si mañana agregas una API o una interfaz web, esa regla no se aplica porque cada quien tiene que acordarse de copiarla. La lógica de negocio se empieza a escapar hacia afuera y la capa `App` deja de ser solo presentación.

## Dónde debería estar

Dentro de `CreateTaskCommand`, en la capa `Cqrs`, para que cualquier entrada al sistema pase por la misma validación.
