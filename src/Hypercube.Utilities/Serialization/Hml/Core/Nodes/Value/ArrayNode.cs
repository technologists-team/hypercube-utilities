namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;

public class ArrayNode : ValueNode
{
    public List<KeyValuePair<string, ValueNode>> Elements { get; set; } = [];
}
