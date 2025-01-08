namespace Hypercube.Utilities.Dependencies.Exceptions;

/// <summary>
/// Represents errors related to invalid registrations in the container.
/// </summary>
public class InvalidRegistrationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidRegistrationException"/> class.
    /// </summary>
    /// <param name="message">The error message explaining the invalid registration.</param>
    public InvalidRegistrationException(string message) : base(message)
    {
    }
}
