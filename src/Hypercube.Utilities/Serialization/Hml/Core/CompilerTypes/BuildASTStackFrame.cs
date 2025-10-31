using Hypercube.Utilities.Serialization.Hml.Core.Nodes;

namespace Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

public record BuildAstStackFrame()
{
    public required Node Node;
    public required Node Parent;
}