using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes;

public class IdentifierNode : Node, IIdentifierNode
{
    public required string Name { get; init; }
    
    public override void OnBuild(Stack<BuildAstStackFrame> stack, Queue<Node> nodes, BuildAstStackFrame frame)
    {
        // Do noting
    }

    public override string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options)
    {
        return Name;
    }
}