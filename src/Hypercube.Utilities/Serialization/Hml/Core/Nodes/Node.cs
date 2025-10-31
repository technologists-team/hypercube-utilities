using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes;

public abstract class Node : INode
{
    public INode Parent { get; set;  } = null!;

    public abstract void OnBuild(Stack<BuildAstStackFrame> stack, Queue<Node> nodes, BuildAstStackFrame frame);
    public abstract string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options);
}
