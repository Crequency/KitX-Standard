using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Validation result for block scripts
/// </summary>
public class BlockScriptValidationResult
{
    /// <summary>
    /// Whether the script is valid
    /// </summary>
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// List of errors found
    /// </summary>
    public List<string> Errors { get; set; } = [];

    /// <summary>
    /// List of warnings
    /// </summary>
    public List<string> Warnings { get; set; } = [];

    /// <summary>
    /// Adds an error
    /// </summary>
    public void AddError(string error) => Errors.Add(error);

    /// <summary>
    /// Adds a warning
    /// </summary>
    public void AddWarning(string warning) => Warnings.Add(warning);
}