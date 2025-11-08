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
        if (frame.State == 0)
        {
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
            
            stack.Push(frame with { State = 2 });
            if (Properties.Count > 1)
                stack.Push(frame with { State = 1, Index = 1 });
            
            stack.Push(new RenderAstStackFrame(Properties[0]));
            return string.Empty;
        }

        if (frame.State == 1)
        {
            if (frame.Index < Properties.Count)
            {
                var index = frame.Index;
                buffer.Append(',');
                
                if (options.Indented)
                {
                    buffer.AppendLine();
                    buffer.Append(state.Indent);
                }
                
                stack.Push(frame with { State = 1,  Index = index + 1 });
                stack.Push(new RenderAstStackFrame(Properties[index]));
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
            buffer.Append('}');
            return string.Empty;
        }
        
        return string.Empty;
    }
}
