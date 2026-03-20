using System.Reflection;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value;
using Hypercube.Utilities.Serialization.Hml.Core.Nodes.Value.Primitives;

namespace Hypercube.Utilities.Serialization.Hml.Core;

public static class HmlDeserializer
{
    public static T Compile<T>(RootNode ast, HmlSerializerOptions options)
    {
        return (T) Compile(ast.Child, typeof(T), options);
    }

    private static object Compile(Node node, Type targetType, HmlSerializerOptions options)
    {
        return node switch
        {
            ObjectNode objNode => CompileObject(objNode, targetType, options),
            ListNode listNode => CompileList(listNode, targetType, options),
            PrimitiveValueNode primNode => CompilePrimitive(primNode, targetType),
            IdentifierNode identNode => identNode.Name,
            _ => throw new InvalidOperationException($"Unknown node type: {node.GetType()}")
        };
    }

    private static object CompileObject(ObjectNode node, Type targetType, HmlSerializerOptions options)
    {
        if (typeof(System.Collections.IDictionary).IsAssignableFrom(targetType))
        {
            var valueType = targetType.IsGenericType
                ? targetType.GetGenericArguments()[1] 
                : typeof(object);

            var dict = (System.Collections.IDictionary?)
                Activator.CreateInstance(targetType) ?? throw new InvalidOperationException($"Cannot create instance of {targetType}");

            foreach (var kvp in node.Properties)
            {
                var key = kvp.Key.Name;
                var value = Compile((Node) kvp.Value, valueType, options);
                dict.Add(key, value);
            }

            return dict;
        }
        
        var obj = Activator.CreateInstance(targetType) ?? throw new InvalidOperationException($"Cannot create instance of {targetType}");
        foreach (var kvp in node.Properties)
        {
            var propertyName = kvp.Key.Name;
            var propertyValue = Compile((Node) kvp.Value, GetPropertyType(targetType, propertyName), options);
            SetPropertyOrField(obj, propertyName, propertyValue);
        }

        return obj;
    }

    private static object CompileList(ListNode node, Type targetType, HmlSerializerOptions options)
    {
        var elementType = targetType.IsArray 
            ? targetType.GetElementType()!
            : targetType.GetGenericArguments().FirstOrDefault() ?? typeof(object);

        var elements = node.Elements
            .Select(e => Compile((Node) e, elementType, options))
            .ToList();

        if (targetType.IsArray)
        {
            var array = Array.CreateInstance(elementType, elements.Count);
            for (int i = 0; i < elements.Count; i++)
                array.SetValue(elements[i], i);
            return array;
        }

        var listType = typeof(List<>).MakeGenericType(elementType);
        var list = Activator.CreateInstance(listType)!;
        var addMethod = listType.GetMethod("Add")!;
        
        foreach (var element in elements)
            addMethod.Invoke(list, [element]);

        return list;
    }

    private static object CompilePrimitive(PrimitiveValueNode node, Type targetType)
    {
        return node switch
        {
            BoolValue boolVal => boolVal.Value,
            NumberValueNode numVal => ConvertNumber(numVal.Value, targetType),
            StringValueNode strVal => strVal.Value,
            NullValueNode => null!,
            UnknownValueNode unknownVal => unknownVal.Value,
            _ => throw new InvalidOperationException($"Unknown primitive type: {node.GetType()}")
        };
    }

    private static object ConvertNumber(decimal value, Type targetType)
    {
        if (targetType == typeof(decimal)) return value;
        if (targetType == typeof(double)) return (double)value;
        if (targetType == typeof(float)) return (float)value;
        if (targetType == typeof(long)) return (long)value;
        if (targetType == typeof(ulong)) return (ulong)value;
        if (targetType == typeof(int)) return (int)value;
        if (targetType == typeof(uint)) return (uint)value;
        if (targetType == typeof(short)) return (short)value;
        if (targetType == typeof(ushort)) return (ushort)value;
        if (targetType == typeof(byte)) return (byte)value;
        if (targetType == typeof(sbyte)) return (sbyte)value;
        
        return value;
    }

    private static Type GetPropertyType(object obj, string propertyName)
    {
        var type = obj.GetType();
        var property = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (property != null)
            return property.PropertyType;
        
        var field = type.GetField(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (field != null)
            return field.FieldType;
        
        return typeof(object);
    }

    private static void SetPropertyOrField(object obj, string name, object? value)
    {
        var type = obj.GetType();
        var property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (property != null)
        {
            property.SetValue(obj, value);
            return;
        }
        
        var field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (field != null)
        {
            field.SetValue(obj, value);
            return;
        }
        
        throw new InvalidOperationException($"Property or field '{name}' not found in type {type.Name}");
    }
}
