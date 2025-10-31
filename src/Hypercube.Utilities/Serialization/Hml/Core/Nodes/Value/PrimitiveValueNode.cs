using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;

public abstract class PrimitiveValueNode : ValueNode
{
    public override void OnBuild(Stack<BuildAstStackFrame> stack, Queue<Node> nodes, BuildAstStackFrame frame)
    {
        // Do nothing
    }
}