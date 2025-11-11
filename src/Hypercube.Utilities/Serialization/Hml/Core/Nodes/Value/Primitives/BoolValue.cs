using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value.Primitives;

public class BoolValue : PrimitiveValueNode
{
    private readonly bool _value;

    public BoolValue(bool value)
    {
        _value = value;
    }

    public override string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options)
    {
        return _value.ToString();
    }
}