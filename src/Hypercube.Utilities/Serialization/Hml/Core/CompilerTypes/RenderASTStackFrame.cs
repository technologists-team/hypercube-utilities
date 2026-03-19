using Hypercube.Utilities.Serialization.Hml.Core.Nodes;

namespace Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

public record RenderAstStackFrame(INode Node)
{
    public readonly INode Node = Node;
    
    public RenderState State;
    public int Index;
}
