using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Bridge interface for communication between BlueprintEditor and WorkflowEditor.
/// BlueprintEditor always runs in association with a WorkflowEditor instance.
/// </summary>
public interface IWorkflowEditorBridge
{
    /// <summary>
    /// Gets the current BlockScript source code from the WorkflowEditor's main program area
    /// </summary>
    string? GetCurrentScript();

    /// <summary>
    /// Gets the current helper functions list from the WorkflowEditor
    /// </summary>
    List<HelperFunction>? GetHelperFunctions();

    /// <summary>
    /// Writes BlockScript source code back to the WorkflowEditor's main program area
    /// and triggers UI update
    /// </summary>
    void SetScript(string sourceCode, List<HelperFunction>? helpers);

    /// <summary>
    /// Appends output text to the WorkflowEditor's Output panel
    /// </summary>
    void AppendOutput(string output);

    /// <summary>
    /// Triggers script execution in the WorkflowEditor
    /// </summary>
    void TriggerExecution();
}
