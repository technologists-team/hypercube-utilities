using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;

public sealed class ListNode : Node, IValueNode
{
    public List<IValueNode> Elements { get; } = [];

    public override void OnBuild(Stack<BuildAstStackFrame> stack, Queue<Node> nodes, BuildAstStackFrame frame)
    {
        var nextNode = nodes.Dequeue();

        if (nextNode is EndNode)
            return;
                
        if (nextNode is not IValueNode valueNode)
            throw new Exception("");
                
        Elements.Add(valueNode);
        stack.Push(frame);
        stack.Push(new BuildAstStackFrame(valueNode, this));
    }

    public override string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options)
    {
        if (frame.State == 0)
        {
            buffer.Append('[');
            state.PushIndent();
            
            if (Elements.Count == 0)
            {
                buffer.Append(']');
                state.PopIndent();
                return string.Empty;
            }
            
            if (options.Indented)
            {
                buffer.AppendLine();
                buffer.Append(state.Indent);
            }
            
            stack.Push(frame with { State = 2 });
            if (Elements.Count > 1)
                stack.Push(frame with { State = 1, Index = 1});
            
            stack.Push(new RenderAstStackFrame(Elements[0]));
            return string.Empty;
        }

        if (frame.State == 1)
        {
            if (frame.Index < Elements.Count)
            {
                var index = frame.Index;
                buffer.Append(',');
                
                if (options.Indented)
                {
                    buffer.AppendLine();
                    buffer.Append(state.Indent);
                }
                
                stack.Push(frame with { State = 1,  Index = index + 1 });
                stack.Push(new RenderAstStackFrame(Elements[index]));
            }
            
            return string.Empty;
        }

        if (frame.State == 2)
        {
            if (options.Indented)
            {
                buffer.AppendLine();
                state.PopIndent();
                buffer.Append(state.Indent);
            }
            buffer.Append(']');
            return string.Empty;
        }
        
        return string.Empty;
    }
}