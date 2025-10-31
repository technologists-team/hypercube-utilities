using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;

public class EndNode : Node
{
    public override void OnBuild(Stack<BuildAstStackFrame> stack, Queue<Node> nodes, BuildAstStackFrame frame)
    {
        // Do nothing
    }

    public override string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options)
    {
        return string.Empty;
    }
}