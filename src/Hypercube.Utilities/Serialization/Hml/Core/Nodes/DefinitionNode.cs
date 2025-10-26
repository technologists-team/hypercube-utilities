using Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes;

public class DefinitionNode : Node
{
    public string Name { get; set; } = "";
    public string TypeName { get; set; } = "";
    public ValueNode Value { get; set; } = default!;
}
