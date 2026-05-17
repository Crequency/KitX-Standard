using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitX.Core.Contract.Workflow;

public enum ExecutionSpeed
{
    RealTime,
    Slow,
    StepByStep
}

public interface IBlueprintDebugController
{
    event Action<string>? NodeExecuting;
    event Action<string>? NodeExecuted;
    event Action<string>? BlockEntered;
    event Action<string, object?>? VariableChanged;
    event Action? ExecutionPaused;
    event Action? ExecutionResumed;

    void SetBreakpoint(string nodeId);
    void RemoveBreakpoint(string nodeId);
    void ClearBreakpoints();
    bool HasBreakpoint(string nodeId);

    void Pause();
    void StepNext();
    void Continue();
    void SetSpeed(ExecutionSpeed speed);

    ExecutionSpeed Speed { get; }
    bool IsPaused { get; }

    IReadOnlyDictionary<string, object?> CurrentVariableSnapshot { get; }

    void UpdateVariableSnapshot(Dictionary<string, object?> variables);

    Task CheckpointAsync(
        string statementId, string? blockName,
        System.Threading.CancellationToken cancellationToken);
}
