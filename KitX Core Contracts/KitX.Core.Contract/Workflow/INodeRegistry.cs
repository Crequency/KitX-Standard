using System.Collections.Generic;

namespace KitX.Core.Contract.Workflow;

/// <summary>
/// Unified registry for node type creation and metadata.
/// Replaces INodeCreationService and INodeTemplateProvider with a single,
/// self-describing approach where each node type carries its own descriptor.
/// </summary>
public interface INodeRegistry
{
    /// <summary>
    /// Creates a new node instance of the given type with default pin configuration.
    /// </summary>
    BlueprintNode Create(BlueprintNodeType type);

    /// <summary>
    /// Gets the descriptor (layout + pin info) for a node type.
    /// Delegates to the node's GetDescriptor() method.
    /// Results are cached for performance.
    /// </summary>
    NodeDescriptor GetDescriptor(BlueprintNodeType type);

    /// <summary>
    /// Returns all registered node types.
    /// </summary>
    IReadOnlySet<BlueprintNodeType> RegisteredTypes { get; }

    /// <summary>
    /// Creates a <see cref="BuiltinFunctionNode"/> pre-configured from an
    /// <see cref="IBuiltinFunctionDefinition"/>. The node's pins, dimensions,
    /// and display name are all driven by the definition.
    /// </summary>
    /// <param name="functionName">The built-in function name (must be registered in BuiltinFunctionRegistry)</param>
    /// <returns>A fully configured BuiltinFunctionNode</returns>
    /// <exception cref="ArgumentException">Thrown when the function name is not registered</exception>
    BlueprintNode CreateBuiltinFunctionNode(string functionName);
}
