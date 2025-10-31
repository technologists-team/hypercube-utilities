using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value.Primitives;

public class StringValueNode : PrimitiveValueNode, IIdentifierNode
{
    public required string Value;
    public string Name => Value;
        
    public override string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options)
    {
        return $"'{Value}'";
    }

}