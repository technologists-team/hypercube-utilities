using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;

public class KeyValuePairNode : Node
{
    public IIdentifierNode Key;
    public IValueNode Value;

    public KeyValuePairNode()
    {
        Key = null!;
        Value = null!;
    }

    public KeyValuePairNode(IIdentifierNode key, IValueNode value)
    {
        Key = key;
        Value = value;
        
        key.SetParent(this);
        Value.SetParent(this);
    }

    public override void OnBuild(Stack<BuildAstStackFrame> stack, Queue<Node> nodes, BuildAstStackFrame frame)
    {
        var node = nodes.Dequeue();
                
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (Key is null)
        {
            if (node is not IIdentifierNode keyNode)
                throw new Exception("");
            
            stack.Push(frame);
            stack.Push(new BuildAstStackFrame(node, this));
            Key = keyNode;
            return;
        }
                
        if (node is not IValueNode valueNode)
            throw new Exception("");
                
        Value = valueNode;
        stack.Push(new BuildAstStackFrame(node, this));
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

            if (options.Indented)
                buffer.Append(" ");
            
            stack.Push(new RenderAstStackFrame(Value));
            return string.Empty;
        }
        
        return string.Empty;
    }
}