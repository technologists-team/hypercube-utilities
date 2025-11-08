using System.Collections;
using System.Text;
using Hypercube.Utilities.Helpers;
using Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value.Primitives;

namespace Hypercube.Utilities.Serialization.Hml.Core;

public static class HmlCompiler
{
    public static string Serialize(object? obj, HmlSerializerOptions options)
    {
        return SerializeToBuilder(obj, options).ToString();
    }
    
    public static StringBuilder SerializeToBuilder(object? obj, HmlSerializerOptions options)
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

            // Null
            if (current is null)
            {
                nodeQueue.Enqueue(new NullValueNode());
                continue;
            }

            // Booleans, numbers, strings
            if (IsPrimitiveType(current))
            {
                nodeQueue.Enqueue(ToPrimitiveValueNode(current));
                continue;
            }

            // Arrays, lists
            if (current is IList list)
            {
                nodeQueue.Enqueue(new ListNode());
                stack.Push(new EndNode());
                
                for (var i = list.Count - 1; i >= 0; i--)
                    stack.Push(list[i]);
                
                continue;
            }
            
            // Object
            var values = ReflectionHelper.GetValueInfos(current);
            
            nodeQueue.Enqueue(new ObjectNode());
            stack.Push(new EndNode());

            for (var i = values.Count - 1; i >= 0; i--)
            {
                var valueInfo = values[i];
                var value = valueInfo.GetValue(current);
                
                stack.Push(value);
                stack.Push(new IdentifierNode(valueInfo.Name));
                stack.Push(new KeyValuePairNode());
            }
        }
        
        return RenderAst(BuildAst(nodeQueue), options);
    }

    private static RootNode BuildAst(Queue<Node> nodes)
    {
        var stack = new Stack<BuildAstStackFrame>();
        var ast = new RootNode();
        
        stack.Push(new BuildAstStackFrame(ast, null!));

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
        var state = new RenderAstState { IndentSize = options.IndentSize};
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

    private static PrimitiveValueNode ToPrimitiveValueNode(object obj)
    {
        return obj switch
        {
            bool value => new BoolValue(value),
            decimal value => new NumberValueNode(value),
            float value => new NumberValueNode((decimal) value),
            double value => new NumberValueNode((decimal) value),
            string value => new StringValueNode(value),
            char value => new StringValueNode(value),
            _ => new UnknownValueNode(obj)
        };
    }

    private static bool IsPrimitiveType(object obj)
    {
        var type = obj.GetType();
        return type.IsPrimitive || type.IsEnum || type == typeof(string);
    }
}
