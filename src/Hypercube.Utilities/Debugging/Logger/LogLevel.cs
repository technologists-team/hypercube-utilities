namespace Hypercube.Utilities.Debugging.Logger;

/// <summary>
/// Represents the available log levels for logging messages.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Trace level logging.
    /// 
    /// Used for very detailed and low-level diagnostic messages.
    /// Typically used for debugging fine-grained application behavior 
    /// or tracing the execution flow during development. 
    /// These logs are verbose and often disabled in production environments.
    /// </summary>
    Trace,

    /// <summary>
    /// Debug level logging.
    /// 
    /// Used for debugging application logic and capturing additional details 
    /// about the state of the application. These logs are useful for 
    /// troubleshooting and should be enabled in staging or testing environments 
    /// but generally disabled in production.
    /// </summary>
    Debug,

    /// <summary>
    /// Info level logging.
    /// 
    /// Used for general informational messages that indicate normal application 
    /// operations. These logs are useful for tracking the flow of execution 
    /// and recording significant events such as application startup or shutdown.
    /// Commonly enabled in production environments.
    /// </summary>
    Info,

    /// <summary>
    /// Warning level logging.
    /// 
    /// Used for events that may indicate a potential problem or situation 
    /// requiring attention but do not disrupt the normal operation of the application. 
    /// Examples include deprecated API usage, low disk space, or unusual behavior.
    /// </summary>
    Warning,

    /// <summary>
    /// Error level logging.
    /// 
    /// Used for logging errors that prevent the application or a particular 
    /// functionality from working correctly. These logs are essential for 
    /// identifying and resolving critical issues and should be reviewed regularly.
    /// </summary>
    Error,

    /// <summary>
    /// Critical level logging.
    /// 
    /// Used for severe errors that cause the application to terminate 
    /// or lead to data loss or corruption. These logs should be used 
    /// sparingly and indicate issues that require immediate attention.
    /// </summary>
    Critical
}