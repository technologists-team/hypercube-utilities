using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;

public class ObjectNode : Node, IValueNode
{
    public List<KeyValuePairNode> Properties { get; } = [];

    public void Add(KeyValuePairNode node)
    {
        Properties.Add(node);
        node.SetParent(this);
    }
    
    public override void OnBuild(Stack<BuildAstStackFrame> stack, Queue<Node> nodes, BuildAstStackFrame frame)
    {
        var nextNode = nodes.Dequeue();

        if (nextNode is EndNode)
            return;
                
        if (nextNode is not KeyValuePairNode keyValuePair)
            throw new Exception("");
                
        Properties.Add(keyValuePair);
        stack.Push(frame);
        stack.Push(new BuildAstStackFrame(keyValuePair, this));
    } 

    public override string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options)
    {
        switch (frame.State)
        {
            case RenderState.Start:
                buffer.Append('{');
                state.PushIndent();
            
                if (options.Indented && Properties.Count != 0)
                {
                    buffer.AppendLine();
                    buffer.Append(state.Indent);
                }

                if (Properties.Count == 0)
                {
                    buffer.Append('}');
                    state.PopIndent();
                    return string.Empty;
                }
            
                stack.Push(frame with { State = RenderState.End });
                
                if (Properties.Count > 1)
                    stack.Push(frame with { State = RenderState.Body, Index = 1 });
            
                stack.Push(new RenderAstStackFrame(Properties[0]));
                break;
            
            case RenderState.Body:
                if (frame.Index >= Properties.Count)
                    return string.Empty;
                
                var index = frame.Index;
                if (options.ObjectEol || !options.Indented)
                    buffer.Append(',');
                
                if (options.Indented)
                {
                    buffer.AppendLine();
                    buffer.Append(state.Indent);
                }
                
                stack.Push(frame with { State = RenderState.Body,  Index = index + 1 });
                stack.Push(new RenderAstStackFrame(Properties[index]));
                break;
            
            case RenderState.End:
                if (options.Indented)
                {
                    buffer.AppendLine();
                    state.PopIndent();
                    buffer.Append(state.Indent);
                }
            
                buffer.Append('}');
                break;
        }

        return string.Empty;
    }
}
