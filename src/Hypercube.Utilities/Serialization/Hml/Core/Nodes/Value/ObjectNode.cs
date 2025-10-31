using System.Text;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

namespace Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;

public class ObjectNode : ValueNode
{
    public Dictionary<string, ValueNode> Properties1 { get; set; } = new();
    public List<KeyValuePairNode> Properties { get; set; } = new();

    public override void OnBuild(Stack<BuildAstStackFrame> stack, Queue<Node> nodes, BuildAstStackFrame frame)
    {
        var nextNode = nodes.Dequeue();

        if (nextNode is EndNode)
            return;
                
        if (nextNode is not KeyValuePairNode keyValuePair)
            throw new Exception("");
                
        Properties.Add(keyValuePair);
        stack.Push(frame);
        stack.Push(new BuildAstStackFrame() { Node = keyValuePair,  Parent = this });
    } 

    public override string Render(Stack<RenderAstStackFrame> stack, StringBuilder buffer, RenderAstStackFrame frame, RenderAstState state, HmlSerializerOptions options)
    {
        if (frame.State == 0)
        {
            buffer.Append('{');
            state.PushIndent();
            
            if (options.WriteIndented && Properties.Count != 0)
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
                
                if (options.WriteIndented)
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
            if (options.WriteIndented)
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
