using System.Collections;
using System.Text;
using Hypercube.Utilities.Helpers;

namespace Hypercube.Utilities.Serialization.Hml.Core;

public static class HmlCompiler
{
    private record StackItem(object? Obj, int Indent, string? Name, bool IsClosing = false, bool Enumerated = false);

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
