namespace Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

public class RenderAstState
{
    public readonly int IndentSize;
    
    public string Indent { get; private set; } = string.Empty;

    public RenderAstState(int indentSize)
    {
        IndentSize = indentSize;
    }

    public void PushIndent()
    {
        Indent += new string(' ', IndentSize);
    }

    public void PopIndent()
    {
        Indent = Indent.Remove(Indent.Length - IndentSize);
    }
}
