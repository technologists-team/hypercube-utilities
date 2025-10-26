namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value.Expression;

public class BinaryExpressionNode : ExpressionNode
{
    public ValueNode Left { get; set; } = default!;
    public string Operator { get; set; } = "";
    public ValueNode Right { get; set; } = default!;
}
