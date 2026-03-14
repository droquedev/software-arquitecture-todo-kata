# Degradación: Acceso directo al repositorio desde un Use Case

## Qué pasaría

Para duplicar una tarea, `DuplicateTaskUseCase` necesita leer la tarea original. Alguien decide inyectarle `ITaskRepository` directamente y hacer la consulta ahí, saltándose `DuplicateTaskCommand`.

```csharp
// En DuplicateTaskUseCase.cs — esto NO debería estar aquí
var task = await _taskRepository.GetByIdAsync(request.TaskId, ct);
if (task is null) return new DuplicateTaskResult("Tarea no encontrada.");
```

## Por qué es un problema

Los casos de uso dejan de ser orquestadores y se convierten en una mezcla de lógica y acceso a datos. La capa `Cqrs` existe exactamente para encapsular eso. Con el tiempo, los use cases se van llenando de queries y la separación entre capas se borra sola.

## Dónde debería estar

Dentro de `DuplicateTaskCommand`, que ya tiene acceso al repositorio y es el responsable de toda la operación.
