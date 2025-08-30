using Hypercube.Utilities.Constants;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Debugging.Logger;

/// <inheritdoc/>
public abstract class Logger : ILogger
{
    /// <inheritdoc/>
    public LogLevel LogLevel { get; set; } = LogLevel.Trace;

    /// <inheritdoc/>
    public abstract void Echo(string message);

    /// <inheritdoc/>
    public virtual void Log(LogLevel level, string message)
    {
        if (level < LogLevel)
            return;
        
        Echo($"{GetColor(level)}[{level}] {message}{Ansi.Reset}");
    }

    /// <inheritdoc/>
    public virtual void Log(LogLevel level, string template, params object[] args)
    {
        Log(level, string.Format(template, args));
    }

    /// <inheritdoc/>
    public virtual void Log(LogLevel level, Exception exception, string message = "")
    {
        var fullMessage = message != string.Empty
            ? $"{message}\nException: {exception}"
            : $"Exception: {exception}";
        
        Log(level, fullMessage);
    }

    /// <inheritdoc/>
    public void Trace(string message)
    {
        Log(LogLevel.Trace, message);
    }

    /// <inheritdoc/>
    public void Debug(string message)
    {
        Log(LogLevel.Debug, message);
    }

    /// <inheritdoc/>
    public void Info(string message)
    {
        Log(LogLevel.Info, message);
    }

    /// <inheritdoc/>
    public void Warning(string message)
    {
        Log(LogLevel.Warning, message);
    }

    /// <inheritdoc/>
    public void Error(string message)
    {
        Log(LogLevel.Error, message);
    }
    
    /// <inheritdoc/>
    public void Error(Exception exception, string message = "")
    {
        Log(LogLevel.Error, exception, message);
    }

    /// <inheritdoc/>
    public void Critical(string message)
    {
        Log(LogLevel.Critical, message);
    }
    
    /// <inheritdoc/>
    public void Critical(Exception exception, string message = "")
    {
        Log(LogLevel.Critical, exception, message);
    }

    [PublicAPI]
    protected static string GetColor(LogLevel level)
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
