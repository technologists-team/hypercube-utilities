using Hypercube.Utilities.Serialization.Hml.Core.Nodes;

namespace Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

public record RenderAstStackFrame
{
    public INode Node;
    public int State;
    public int Index;

    public RenderAstStackFrame(INode node)
    {
        Node = node;
    }
}