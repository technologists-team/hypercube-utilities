namespace Hypercube.Utilities.Serialization.Hml.Core.CompilerTypes;

public class RenderAstState(int indentSize)
{
    public string Indent { get; private set; } = string.Empty;

    public void PushIndent()
    {
        Indent += new string(' ', indentSize);
    }

    public void PopIndent()
    {
        Indent = Indent.Remove(Indent.Length - indentSize);
    }
}
