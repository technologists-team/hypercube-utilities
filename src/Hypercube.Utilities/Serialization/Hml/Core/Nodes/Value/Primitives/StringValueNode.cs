using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value.Primitives;

public class StringValueNode : PrimitiveValueNode, IIdentifierNode
{
    public readonly string Value;

    public string Name => Value;

    public StringValueNode(string value)
    {
        Value = value;
    }

    public StringValueNode(char value)
    {
        Value = value.ToString();
    }

    public override string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options)
    {
        return $"'{Value}'";
    }
}
