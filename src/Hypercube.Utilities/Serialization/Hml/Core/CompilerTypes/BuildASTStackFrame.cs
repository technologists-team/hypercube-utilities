using Hypercube.Utilities.Serialization.Hml.Core.Nodes;

namespace Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

public record BuildAstStackFrame
{
    public readonly Node Node;
    public readonly Node Parent;

    public BuildAstStackFrame(Node node, Node parent)
    {
        Node = node;
        Parent = parent;
    }
}