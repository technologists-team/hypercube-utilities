namespace Hypercube.Utilities.Debugging.Logger;

/// <summary>
/// Interface for an advanced logger.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Gets or sets the current log level.
    /// Messages below this level will be ignored.
    /// </summary>
    LogLevel LogLevel { get; set; }

    /// <summary>
    /// Logs a simple text message.
    /// </summary>
    /// <param name="level">The log level of the message.</param>
    /// <param name="message">The message to log.</param>
    void Log(LogLevel level, string message);

    /// <summary>
    /// Logs a structured message using a template and arguments.
    /// </summary>
    /// <param name="level">The log level of the message.</param>
    /// <param name="template">The template for the message, e.g., "User {UserId} logged in".</param>
    /// <param name="args">The arguments to format the template with.</param>
    void Log(LogLevel level, string template, params object[] args);

    /// <summary>
    /// Logs an exception along with an optional message.
    /// </summary>
    /// <param name="level">The log level of the message.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Optional additional message.</param>
    void Log(LogLevel level, Exception exception, string message = "");

    void Echo(string message);
    
    /// <summary>
    /// Logs a trace-level message.
    /// </summary>
    /// <param name="message">The trace message to log.</param>
    void Trace(string message);

    /// <summary>
    /// Logs a debug-level message.
    /// </summary>
    /// <param name="message">The debug message to log.</param>
    void Debug(string message);

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message">The informational message to log.</param>
    void Info(string message);

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The warning message to log.</param>
    void Warning(string message);

    /// <summary>
    /// Logs an error along with an optional message and exception.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Optional additional message.</param>
    void Error(Exception exception, string message = "");

    /// <summary>
    /// Logs a critical-level message.
    /// </summary>
    /// <param name="message">The critical message to log.</param>
    void Critical(string message);
}