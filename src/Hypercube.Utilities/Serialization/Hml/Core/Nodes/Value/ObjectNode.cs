namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;

public class ObjectNode : ValueNode
{
    public Dictionary<string, ValueNode> Properties1 { get; set; } = new();
    public List<KeyValuePairNode> Properties { get; set; } = new();
}
