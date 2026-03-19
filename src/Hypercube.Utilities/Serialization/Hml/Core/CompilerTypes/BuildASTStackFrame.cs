using System.Diagnostics;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes;

namespace Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

[DebuggerDisplay("{Node} ({Parent})")]
public record BuildAstStackFrame(INode Node, INode Parent)
{
    public readonly INode Node = Node;
    public readonly INode Parent = Parent;
}
