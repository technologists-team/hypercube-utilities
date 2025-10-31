using System.Collections;
using System.Text;
using System.Text.Json;
using Hypercube.Utilities.Helpers;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value.Primitives;

namespace Hypercube.Utilities.Serialization.Hml.Core;

public static class HmlCompiler
{
    public static StringBuilder Serialize(object? obj, HmlSerializerOptions options)
    {
        var stack = new Stack<object?>();
        var nodeQueue = new Queue<Node>();
        stack.Push(obj);

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if (current is Node currentNode)
            {
                nodeQueue.Enqueue(currentNode);
                continue;
            }
            
            if (current is null)
            {
                nodeQueue.Enqueue(new NullValueNode());
                continue;
            }
            
            if (IsSimpleType(current) || IsStringType(current))
            {
                PrimitiveValueNode node = current switch
                {
                    bool value => new BoolValue() {  Value = value },
                    decimal value => new NumberValueNode() { Value =  value},
                    float value => new NumberValueNode() { Value = (decimal)value},
                    double value => new NumberValueNode() { Value = (decimal)value},
                    string value => new StringValueNode() { Value = value},
                    char value => new StringValueNode() { Value = $"{value}"},
                    _ => new UnknownValueNode() { Value = current! }
                };
                
                nodeQueue.Enqueue(node);
                continue;
            }

            if (current is IList list)
            {
                nodeQueue.Enqueue(new ListNode());
                
                stack.Push(new EndNode());
                
                for (var i = list.Count - 1; i >= 0; i--)
                    stack.Push(list[i]);
                
                continue;
            }
            
            var type = current.GetType();
            var values = ReflectionHelper.GetValueInfos(type);
            nodeQueue.Enqueue(new ObjectNode());
            stack.Push(new EndNode());

            for (var i = values.Count - 1; i >= 0; i--)
            {
                var valueInfo = values[i];
                var value = valueInfo.GetValue(current);
                
                stack.Push(value);
                stack.Push(new IdentifierNode { Name = valueInfo.Name });
                stack.Push(new KeyValuePairNode());
            }
        }
        
        return RenderAst(BuildAst(nodeQueue), options);
    }

    private static RootNode BuildAst(Queue<Node> nodes)
    {
        var stack = new Stack<BuildAstStackFrame>();
        var ast = new RootNode();
        stack.Push(new BuildAstStackFrame { Node = ast, Parent = null! });

        while (stack.Count > 0)
        {
            var frame = stack.Pop();
            
            frame.Node.Parent = frame.Parent;
            frame.Node.OnBuild(stack, nodes, frame);
        }

        return ast;
    }

    private static StringBuilder RenderAst(RootNode ast, HmlSerializerOptions options)
    {
        var state = new RenderAstState() { IndentSize = options.IndentSize};
        var result = new StringBuilder();
        var stack = new Stack<RenderAstStackFrame>();
        stack.Push(new RenderAstStackFrame(ast));

        while (stack.Count > 0)
        {
            var frame = stack.Pop();
            result.Append(frame.Node.Render(stack, result, frame, state, options));
        }
        
        return result;
    }
    
    private static bool IsSimpleType(object obj)
    {
        var type = obj.GetType();
        return type.IsPrimitive || type.IsEnum || type == typeof(decimal) || type == typeof(bool);
    }
    
    private static bool IsStringType(object obj)
    {
        var type = obj.GetType();
        return type == typeof(string) || type == typeof(char);
    }
}
