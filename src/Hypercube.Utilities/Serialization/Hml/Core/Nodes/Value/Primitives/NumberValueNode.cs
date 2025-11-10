using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value.Primitives;

public class NumberValueNode : PrimitiveValueNode
{
    public readonly decimal Value;

    public NumberValueNode(decimal value)
    {
        Value = value;
    }

    public override string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options)
    {
        return Value.ToString(options.CultureInfo);
    }
}
