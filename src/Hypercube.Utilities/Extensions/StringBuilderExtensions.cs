using System.Text;
using Hypercube.Utilities.Constants;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Extensions;

[PublicAPI]
public static class StringBuilderExtensions
{
    extension(StringBuilder builder)
    {
        public void Append(string line, string color)
        {
            builder.Append(color);
            builder.Append(line);
            builder.Append(Ansi.Reset);
        }
        
        public void AppendLine(string line, string color)
        {
            builder.Append(color);
            builder.Append(line);
            builder.AppendLine(Ansi.Reset);
        }
        
        public void AppendLineWrapped(string line, string color)
        {
            builder.AppendLine(Ansi.Wrap(line, color));
        }
    }
}