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
        switch (frame.State)
        {
            case RenderState.Start:
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
            
                stack.Push(frame with { State = RenderState.End });
                
                if (Elements.Count > 1)
                    stack.Push(frame with { State = RenderState.Body, Index = 1});
            
                stack.Push(new RenderAstStackFrame(Elements[0]));
                break;
            
            case RenderState.Body:
                if (frame.Index >= Elements.Count)
                    return string.Empty;
                
                var index = frame.Index;
                if (options.Eol || !options.Indented)
                    buffer.Append(';');
                
                if (options.Indented)
                {
                    buffer.AppendLine();
                    buffer.Append(state.Indent);
                }
                
                stack.Push(frame with { State = RenderState.Body,  Index = index + 1 });
                stack.Push(new RenderAstStackFrame(Elements[index]));
                break;
            
            case RenderState.End:
                if (options.Indented)
                {
                    buffer.AppendLine();
                    state.PopIndent();
                    buffer.Append(state.Indent);
                }
                
                buffer.Append(']');
                break;
        }
        
        return string.Empty;
    }
}