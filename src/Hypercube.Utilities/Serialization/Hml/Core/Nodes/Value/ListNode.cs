namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;

public class ListNode : ValueNode
{
    public List<ValueNode> Elements { get; set; } = [];
}