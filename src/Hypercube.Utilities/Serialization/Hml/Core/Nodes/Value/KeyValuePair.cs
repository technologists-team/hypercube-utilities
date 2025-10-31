using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;

public class KeyValuePairNode : Node
{
    public IIdentifierNode Key = null!;
    public ValueNode Value = null!;

    public override void OnBuild(Stack<BuildAstStackFrame> stack, Queue<Node> nodes, BuildAstStackFrame frame)
    {
        var node = nodes.Dequeue();
                
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (Key is null)
        {
            if (node is not IIdentifierNode keyNode)
                throw new Exception("");
            
            stack.Push(frame);
            stack.Push(new BuildAstStackFrame { Node = node, Parent = this});
            Key = keyNode;
            return;
        }
                
        if (node is not ValueNode valueNode)
            throw new Exception("");
                
        Value = valueNode;
        stack.Push(new BuildAstStackFrame { Node = node, Parent = this});
    }

    public override string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options)
    {
        if (frame.State == 0)
        {
            stack.Push(frame with { State = 1 });
            stack.Push(new RenderAstStackFrame(Key));
            return string.Empty;
        }

        if (frame.State == 1)
        {
            buffer.Append(":");

            if (options.WriteIndented)
                buffer.Append(" ");
            
            stack.Push(new RenderAstStackFrame(Value));
            return string.Empty;
        }
        
        return string.Empty;
    }
}