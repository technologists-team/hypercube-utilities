using System.Collections;
using System.Text;
using Hypercube.Utilities.Helpers;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value.Primitives;

namespace Hypercube.Utilities.Serialization.Hml.Core;

public static class HmlCompiler
{
    private record StackItem(object? Obj, int Indent, string? Name, bool IsClosing = false, bool Enumerated = false);

    private record StackRecord(object? Obj, List<Node> AdditionalNodes);
    
    private record Frame
    {
        public Node Node;
        public Node Parent;
        public int ChildIndex;

        public Frame(Node node, Node parent)
        {
            Node = node;
            Parent = parent;
            ChildIndex = 0;
        }
    }
    
    public static StringBuilder SerializeNew(object? obj, HmlSerializerOptions options)
    {
        var stack = new Stack<StackRecord>();
        var nodeQueue = new Queue<Node>();
        stack.Push(new StackRecord(obj, []));

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if (current.Obj is EndNode endNode)
            {
                nodeQueue.Enqueue(endNode);
                continue;
            }
            
            foreach (var node in current.AdditionalNodes)
                nodeQueue.Enqueue(node);
            
            if (current.Obj is null)
            {
                nodeQueue.Enqueue(new NullValueNode());
                continue;
            }
            
            if (IsSimpleType(current.Obj) || IsStringType(current.Obj))
            {
                PrimitiveValueNode node = current.Obj switch
                {
                    bool value => new BoolValue() {  Value = value },
                    decimal value => new NumberValueNode() { Value =  value},
                    float value => new NumberValueNode() { Value = (decimal)value},
                    double value => new NumberValueNode() { Value = (decimal)value},
                    string value => new StringValueNode() { Value = value},
                    char value => new StringValueNode() { Value = $"{value}"},
                    _ => new UnknownValueNode() { Value = current.Obj! }
                };
                
                nodeQueue.Enqueue(node);
                continue;
            }

            if (current.Obj is IList list)
            {
                nodeQueue.Enqueue(new ListNode());
                
                stack.Push(new StackRecord(new EndNode(), []));
                
                for (var i = list.Count - 1; i >= 0; i--)
                    stack.Push(new StackRecord(list[i], []));
                
                continue;
            }
            
            var type = current.Obj.GetType();
            var values = ReflectionHelper.GetValueInfos(type);
            nodeQueue.Enqueue(new ObjectNode());
            stack.Push(new StackRecord(new EndNode(), []));

            for (var i = values.Count - 1; i >= 0; i--)
            {
                var valueInfo = values[i];
                var value = valueInfo.GetValue(current.Obj);
                
                stack.Push(new StackRecord(value, []));
                stack.Push(new StackRecord(valueInfo.Name, [new KeyValuePairNode()]));
            }
        }
        
        var ast = BuildAst(nodeQueue);
        
        return new StringBuilder();
    }

    private static RootNode BuildAst(Queue<Node> nodes)
    {
        var stack = new Stack<Frame>();
        var ast = new RootNode();
        var child = nodes.Dequeue();

        ast.Child = child;
        stack.Push(new Frame(child, ast));

        while (stack.Count > 0)
        {
            var frame = stack.Peek();
            frame.Node.Parent = frame.Parent;

            if (frame.Node is PrimitiveValueNode)
            {
                stack.Pop();
                continue;
            }

            if (frame.Node is KeyValuePairNode pair)
            {
                var node = nodes.Dequeue();
                if (node is not ValueNode valueNode)
                    throw new Exception("");
                
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (pair.Key is null)
                {
                    stack.Push(new Frame(node, pair));
                    pair.Key = valueNode;
                    continue;
                }
                
                pair.Value = valueNode;
                stack.Pop();
                stack.Push(new Frame(node, pair));
                continue;
            }

            if (frame.Node is ListNode list)
            {
                var nextNode = nodes.Dequeue();

                if (nextNode is EndNode)
                {
                    stack.Pop();
                    continue;
                }
                
                if (nextNode is not ValueNode valueNode)
                    throw new Exception("");
                
                list.Elements.Add(valueNode);
                stack.Push(new Frame(valueNode, list));
                continue;
            }

            if (frame.Node is ObjectNode objectNode)
            {
                var nextNode = nodes.Dequeue();

                if (nextNode is EndNode)
                {
                    stack.Pop();
                    continue;
                }
                
                if (nextNode is not KeyValuePairNode keyValuePair)
                    throw new Exception("");
                
                
                objectNode.Properties.Add(keyValuePair);
                stack.Push(new Frame(keyValuePair, objectNode));
                continue;
            }
        }

        return ast;
    }
    
    public static StringBuilder Serialize(object? obj, HmlSerializerOptions options)
    {
        var builder = new StringBuilder();
        var stack = new Stack<StackItem>();

        stack.Push(new StackItem(obj, 0, null));

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            var indent = new string(' ', current.Indent * options.IndentSize);

            if (current.IsClosing)
            {
                builder.Append(indent);
                
                if (current.Obj is not null)
                    builder.Append(current.Obj);

                if (current.Enumerated)
                    builder.Append(',');

                builder.AppendLine();
                continue;
            }

            if (current.Obj is null)
            {
                builder.Append(indent);

                if (current.Name is not null)
                {
                    builder.Append(current.Name);
                    builder.Append(": ");
                }
                
                builder.AppendLine(current.Enumerated ? "null," : "null");
                continue;
            }

            var type = current.Obj.GetType();
            if (IsSimpleType(current.Obj) || IsStringType(current.Obj))
            {
                builder.Append(indent);

                if (current.Name is not null)
                {
                    builder.Append(current.Name);
                    builder.Append(": ");
                }
                
                builder.Append(FormatValue(current.Obj, in options));

                if (current.Enumerated)
                    builder.Append(',');
                
                builder.AppendLine();
                continue;
            }
            
            if (current.Obj is IEnumerable enumerable)
            {
                builder.Append(indent);
                
                if (current.Name is not null)
                {
                    builder.Append(current.Name);
                    builder.Append(": ");
                }

                builder.Append('[');
                builder.AppendLine();
                
                var items = new List<object>();
                foreach (var e in enumerable)
                    items.Add(e!);
                
                stack.Push(new StackItem("]", current.Indent, null, true, Enumerated: current.Enumerated));
                for (var i = items.Count - 1; i >= 0; i--)
                    stack.Push(new StackItem(items[i], current.Indent + 1, null, Enumerated: i != items.Count - 1 || options.TrailingComma));
                
                continue;
            }

            builder.Append(indent);
            
            if (current.Name is not null)
            {
                builder.Append(current.Name);
                builder.Append(": ");
            }
            
            builder.Append('{');
            builder.AppendLine();
            
            var values = ReflectionHelper.GetValueInfos(type);
            
            stack.Push(new StackItem("}", current.Indent, null, true, Enumerated: current.Enumerated));

            for (var i = values.Count - 1; i >= 0; i--)
            {
                var valueInfo = values[i];
                var value = valueInfo.GetValue(current.Obj);
                stack.Push(new StackItem(value!, current.Indent + 1, valueInfo.Name));
            }
        }

        return builder;
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

    private static string FormatValue(object obj, in HmlSerializerOptions options)
    {
        return obj switch
        {
            bool value => value ? "true" : "false",
            decimal value => value.ToString(options.CultureInfo),
            float value => value.ToString(options.CultureInfo),
            double value => value.ToString(options.CultureInfo),
            string value => $"'{value}'",
            char value => $"'{value}'",
            _ => obj.ToString()!
        };
    }
}
