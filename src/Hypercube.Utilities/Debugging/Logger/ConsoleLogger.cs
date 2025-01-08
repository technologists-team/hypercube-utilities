using Hypercube.Utilities.Constants;

namespace Hypercube.Utilities.Debugging.Logger;

public class ConsoleLogger : ILogger
{
    public LogLevel LogLevel { get; set; } = LogLevel.Trace;
    
    public void Log(LogLevel level, string message)
    {
        if (level < LogLevel)
            return;
        
        Console.WriteLine($"{GetColor(level)}[{level}] {message}{Ansi.Reset}");
    }

    public void Log(LogLevel level, string template, params object[] args)
    {
        if (level < LogLevel)
            return;
        
        var formattedMessage = string.Format(template, args);
        
        Console.WriteLine($"{GetColor(level)}[{level}] {formattedMessage}{Ansi.Reset}");
    }

    public void Log(LogLevel level, Exception exception, string message = "")
    {
        if (level < LogLevel)
            return;
        
        var fullMessage = message != string.Empty
            ? $"{message}\nException: {exception}"
            : $"Exception: {exception}";
        
        Console.WriteLine($"{GetColor(level)}[{level}] {fullMessage}{Ansi.Reset}");
    }

    public void Echo(string message)
    {
        Console.WriteLine(message);
    }

    public void Trace(string message)
    {
        Log(LogLevel.Trace, message);
    }

    public void Debug(string message)
    {
        Log(LogLevel.Debug, message);
    }

    public void Info(string message)
    {
        Log(LogLevel.Info, message);
    }

    public void Warning(string message)
    {
        Log(LogLevel.Warning, message);
    }

    public void Error(Exception exception, string message = "")
    {
        Log(LogLevel.Error, exception, message);
    }

    public void Critical(string message)
    {
        Log(LogLevel.Critical, message);
    }

    private static string GetColor(LogLevel level)
    {
        return level switch
        {
            LogLevel.Trace => Ansi.BrightBlack,
            LogLevel.Debug => Ansi.Cyan,
            LogLevel.Info => Ansi.White,
            LogLevel.Warning => Ansi.Yellow,
            LogLevel.Error => Ansi.Red,
            LogLevel.Critical => $"{Ansi.BackgroundRed}{Ansi.Black}",
            _ => Ansi.Reset
        };
    }
}