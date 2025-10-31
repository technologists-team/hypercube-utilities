namespace Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

public class RenderAstState
{
    public string Indent { get; private set; } = string.Empty;
    public int IndentSize { get; init; }

    public void PushIndent()
    {
        Indent += new string(' ', IndentSize);
    }

    public void PopIndent()
    {
        Indent = Indent.Remove(Indent.Length - IndentSize);
    }
}