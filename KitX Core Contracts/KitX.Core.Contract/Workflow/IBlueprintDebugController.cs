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

    /// <summary>
    /// Forwards a named runtime value change to subscribers via
    /// <see cref="VariableChanged"/>. Used by the generated workflow code to
    /// publish both PubVar writes (name = PubVar identifier) and wire-value
    /// updates (name = <c>"w:{nodeId}"</c> or <c>"w:{nodeId}:{pinName}"</c>).
    /// The naming convention lets the frontend distinguish the two categories
    /// by checking the <c>w:</c> prefix.
    /// </summary>
    /// <remarks>
    /// Discussion notes §十二-M (data-tooltip MVP): wire values flow on the same
    /// channel as PubVar changes so the existing VariableChanged event and the
    /// existing frontend plumbing (BlueprintConnectorVM.RuntimeValue) can be
    /// reused without a separate WireValueChanged event.
    /// </remarks>
    void NotifyValueChanged(string name, object? value);

    Task CheckpointAsync(
        string statementId, string? blockName,
        System.Threading.CancellationToken cancellationToken);
}
