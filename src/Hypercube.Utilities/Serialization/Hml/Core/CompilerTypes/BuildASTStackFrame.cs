using Hypercube.Utilities.Serialization.Hml.Core.Nodes;

namespace Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

public record BuildAstStackFrame
{
    public readonly INode Node;
    public readonly INode Parent;

    public BuildAstStackFrame(INode node, INode parent)
    {
        Node = node;
        Parent = parent;
    }
}