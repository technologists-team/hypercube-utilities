using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes;

public class RootNode : Node
{
    public Node Child = null!;

    public override void OnBuild(Stack<BuildAstStackFrame> stack, Queue<Node> nodes, BuildAstStackFrame frame)
    {
        Child = nodes.Dequeue();
        stack.Push(new BuildAstStackFrame(Child, this));
    }

    public override string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options)
    {
        stack.Push(new RenderAstStackFrame(Child));
        return string.Empty;
    }
}