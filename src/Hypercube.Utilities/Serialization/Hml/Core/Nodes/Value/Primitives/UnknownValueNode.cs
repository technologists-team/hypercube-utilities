using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value.Primitives;

public class UnknownValueNode : PrimitiveValueNode
{
    public required object Value;
    
    public override string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options)
    {
        return Value.ToString() ?? string.Empty;
    }
}