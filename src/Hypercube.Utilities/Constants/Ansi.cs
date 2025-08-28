using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Constants;

/// <summary>
/// Provides ANSI escape codes for text colors, background colors, and styles.
/// Supports 16, 256, and RGB true-color formatting.
/// </summary>
[PublicAPI]
public static class Ansi
{
    // Reset all formatting
    public const string Reset = "\u001b[0m";

    // Standard text colors
    public const string Black = "\u001b[30m";
    public const string Red = "\u001b[31m";
    public const string Green = "\u001b[32m";
    public const string Yellow = "\u001b[33m";
    public const string Blue = "\u001b[34m";
    public const string Magenta = "\u001b[35m";
    public const string Cyan = "\u001b[36m";
    public const string White = "\u001b[37m";
    
    // Extended palette (commonly used colors from 256 ANSI palette)
    public const string SeroburoMalinovy256 = "\u001b[38;5;132m";
    public const string Orange256 = "\u001b[38;5;208m";
    public const string Pink256 = "\u001b[38;5;213m";
    public const string Purple256 = "\u001b[38;5;129m";
    public const string Violet256 = "\u001b[38;5;177m";
    public const string Teal256 = "\u001b[38;5;37m";
    public const string Aqua256 = "\u001b[38;5;51m";
    public const string Lime256 = "\u001b[38;5;118m";
    public const string Olive256 = "\u001b[38;5;64m";
    public const string Brown256 = "\u001b[38;5;94m";
    public const string Gold256 = "\u001b[38;5;220m";
    public const string Silver256 = "\u001b[38;5;7m";
    public const string Gray256 = "\u001b[38;5;244m";

    // Pastel shades
    public const string PastelPink256 = "\u001b[38;5;218m";
    public const string PastelBlue256 = "\u001b[38;5;153m";
    public const string PastelGreen256 = "\u001b[38;5;151m";
    public const string PastelYellow256 = "\u001b[38;5;229m";
    public const string PastelPurple256 = "\u001b[38;5;183m";

    // Neon-like shades
    public const string NeonPink256 = "\u001b[38;5;199m";
    public const string NeonBlue256 = "\u001b[38;5;27m";
    public const string NeonGreen256 = "\u001b[38;5;46m";
    public const string NeonCyan256 = "\u001b[38;5;50m";
    public const string NeonOrange256 = "\u001b[38;5;202m";
    
    // Dark text colors
    public const string DarkSeroburoMalinovy256 = "\u001b[38;5;95m";
    public const string DarkGray256 = "\u001b[38;5;240m";
    
    // Muted text colors
    public const string MutedSeroburoMalinovy256 = "\u001b[38;5;139m";
    
    // Light text colors
    public const string LightSeroburoMalinovy256 = "\u001b[38;5;174m";
    
    // Bright text colors
    public const string BrightBlack = "\u001b[90m";
    public const string BrightRed = "\u001b[91m";
    public const string BrightGreen = "\u001b[92m";
    public const string BrightYellow = "\u001b[93m";
    public const string BrightBlue = "\u001b[94m";
    public const string BrightMagenta = "\u001b[95m";
    public const string BrightCyan = "\u001b[96m";
    public const string BrightWhite = "\u001b[97m";
    public const string BrightSeroburoMalinovy256 = "\u001b[38;5;217m";
    
    // Background colors
    public const string BackgroundBlack = "\u001b[40m";
    public const string BackgroundRed = "\u001b[41m";
    public const string BackgroundGreen = "\u001b[42m";
    public const string BackgroundYellow = "\u001b[43m";
    public const string BackgroundBlue = "\u001b[44m";
    public const string BackgroundMagenta = "\u001b[45m";
    public const string BackgroundCyan = "\u001b[46m";
    public const string BackgroundWhite = "\u001b[47m";
    public const string BackgroundSeroburoMalinovy256 = "\u001b[48;5;132m";

    // Backgrounds for extended palette
    public const string BackgroundOrange256 = "\u001b[48;5;208m";
    public const string BackgroundPink256 = "\u001b[48;5;213m";
    public const string BackgroundPurple256 = "\u001b[48;5;129m";
    public const string BackgroundViolet256 = "\u001b[48;5;177m";
    public const string BackgroundTeal256 = "\u001b[48;5;37m";
    public const string BackgroundAqua256 = "\u001b[48;5;51m";
    public const string BackgroundLime256 = "\u001b[48;5;118m";
    public const string BackgroundOlive256 = "\u001b[48;5;64m";
    public const string BackgroundBrown256 = "\u001b[48;5;94m";
    public const string BackgroundGold256 = "\u001b[48;5;220m";
    public const string BackgroundSilver256 = "\u001b[48;5;7m";
    public const string BackgroundGray256 = "\u001b[48;5;244m";
    
    // Background colors (custom dark shades using 256-color palette)
    public const string BackgroundDarkRed256 = "\u001b[48;5;52m";
    public const string BackgroundDarkGreen256 = "\u001b[48;5;22m";
    public const string BackgroundDarkYellow256 = "\u001b[48;5;58m";
    public const string BackgroundDarkBlue256 = "\u001b[48;5;17m";
    public const string BackgroundDarkMagenta256 = "\u001b[48;5;53m";
    public const string BackgroundDarkCyan256 = "\u001b[48;5;23m";
    public const string BackgroundDarkGray256 = "\u001b[48;5;232m";
    public const string BackgroundDarkSeroburoMalinovy256 = "\u001b[48;5;95m";

    // Pastel backgrounds
    public const string BackgroundPastelPink256 = "\u001b[48;5;218m";
    public const string BackgroundPastelBlue256 = "\u001b[48;5;153m";
    public const string BackgroundPastelGreen256 = "\u001b[48;5;151m";
    public const string BackgroundPastelYellow256 = "\u001b[48;5;229m";
    public const string BackgroundPastelPurple256 = "\u001b[48;5;183m";

    // Neon backgrounds
    public const string BackgroundNeonPink256 = "\u001b[48;5;199m";
    public const string BackgroundNeonBlue256 = "\u001b[48;5;27m";
    public const string BackgroundNeonGreen256 = "\u001b[48;5;46m";
    public const string BackgroundNeonCyan256 = "\u001b[48;5;50m";
    public const string BackgroundNeonOrange256 = "\u001b[48;5;202m";
    
    // Bright background colors
    public const string BrightBackgroundBlack = "\u001b[100m";
    public const string BrightBackgroundRed = "\u001b[101m";
    public const string BrightBackgroundGreen = "\u001b[102m";
    public const string BrightBackgroundYellow = "\u001b[103m";
    public const string BrightBackgroundBlue = "\u001b[104m";
    public const string BrightBackgroundMagenta = "\u001b[105m";
    public const string BrightBackgroundCyan = "\u001b[106m";
    public const string BrightBackgroundWhite = "\u001b[107m";
    public const string BackgroundBrightSeroburoMalinovy256 = "\u001b[48;5;217m";

    // Muted background colors
    public const string BackgroundMutedSeroburoMalinovy256 = "\u001b[48;5;139m";
    
    // Light background colors
    public const string BackgroundLightSeroburoMalinovy256 = "\u001b[48;5;174m";
    
    // Text styles
    public const string Bold = "\u001b[1m";
    public const string Dim = "\u001b[2m";
    public const string Italic = "\u001b[3m";
    public const string Underline = "\u001b[4m";
    public const string Blink = "\u001b[5m";
    public const string Reversed = "\u001b[7m";

    // 256-color support (standard ANSI palette)
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Text256(int colorCode) => $"\u001b[38;5;{colorCode}m";
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Background256(int colorCode) => $"\u001b[48;5;{colorCode}m";

    // True-color (24-bit RGB support)
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string TextRgb(int r, int g, int b) => $"\u001b[38;2;{r};{g};{b}m";
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string BackgroundRgb(int r, int g, int b) => $"\u001b[48;2;{r};{g};{b}m";

    // Utility methods for gradients and patterns (optional extension)
    public static string GradientText(string text, Func<int, (int R, int G, int B)> colorFunction)
    {
        var result = string.Empty;
        
        for (var i = 0; i < text.Length; i++)
        {
            var (r, g, b) = colorFunction(i);
            result += $"{TextRgb(r, g, b)}{text[i]}";
        }
        
        return result + Reset;
    }
    
    // Standard gradient functions
    public static string GradientLinear(string text, (int R, int G, int B) start, (int R, int G, int B) end)
    {
        return GradientText(text, i =>
        {
            var t = (float) i / Math.Max(1, text.Length - 1);

            var r = (int) (start.R + (end.R - start.R) * t);
            var g = (int) (start.G + (end.G - start.G) * t);
            var b = (int) (start.B + (end.B - start.B) * t);

            return (r, g, b);
        });
    }

    public static string GradientRainbow(string text)
    {
        return GradientText(text, i =>
        {
            var hue = i * 360.0 / Math.Max(1, text.Length) % 360;
            return HsvToRgb(hue, 1.0, 1.0);
        });
    }

    public static string GradientMulti(string text, params (int R, int G, int B)[] colors)
    {
        if (colors.Length < 2)
            return text;

        return GradientText(text, i =>
        {
            var pos = (float) i / Math.Max(1, text.Length - 1);
            var scaled = pos * (colors.Length - 1);
            var idx = (int) Math.Floor(scaled);
            var t = scaled - idx;

            var c1 = colors[idx];
            var c2 = colors[Math.Min(idx + 1, colors.Length - 1)];

            var r = (int) (c1.R + (c2.R - c1.R) * t);
            var g = (int) (c1.G + (c2.G - c1.G) * t);
            var b = (int) (c1.B + (c2.B - c1.B) * t);

            return (r, g, b);
        });
    }

    // Helper: HSV → RGB
    private static (int R, int G, int B) HsvToRgb(double h, double s, double v)
    {
        var c = v * s;
        var x = c * (1 - Math.Abs((h / 60) % 2 - 1));
        var m = v - c;

        double r, g, b;

        switch (h)
        {
            case < 60:
                (r, g, b) = (c, x, 0);
                break;
            
            case < 120:
                (r, g, b) = (x, c, 0);
                break;
            
            case < 180:
                (r, g, b) = (0, c, x);
                break;
            
            case < 240:
                (r, g, b) = (0, x, c);
                break;
            
            case < 300:
                (r, g, b) = (x, 0, c);
                break;
            
            default:
                (r, g, b) = (c, 0, x);
                break;
        }

        return ((int) ((r + m) * 255), (int) ((g + m) * 255), (int) ((b + m) * 255));
    }
}