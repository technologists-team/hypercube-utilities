using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value.Primitives;

public class UnknownValueNode : PrimitiveValueNode
{
    public readonly object Value;

    public UnknownValueNode(object value)
    {
        Value = value;
    }

    public override string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options)
    {
        return Value.ToString() ?? string.Empty;
    }
}