using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;
using Hypercube.Utilities.Serialization.Hml.Exceptions;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes;

public abstract class Node : INode
{
    public INode Parent { get; private set; } = null!;

    public void SetParent(INode parent)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        // if (Parent is not null && Parent != this)
        //    throw new HmlException();
        
        Parent = parent;
    }

    public abstract void OnBuild(Stack<BuildAstStackFrame> stack, Queue<Node> nodes, BuildAstStackFrame frame);
    public abstract string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options);
}
