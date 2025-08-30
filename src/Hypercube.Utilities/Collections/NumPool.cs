using System.Numerics;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Collections;

/// <summary>
/// A generic pool for numeric values that provides sequential numbers with the ability
/// to release numbers back into the pool for reuse.
/// </summary>
/// <typeparam name="T">
/// The numeric type used by the pool. Must be a value type implementing <see cref="INumber{T}"/>.
/// </typeparam>
[PublicAPI]
public class NumPool<T> where T : struct, INumber<T>
{
    private readonly Stack<T> _released = new();
    private T _counter;

    /// <summary>
    /// Gets the next available number from the pool.
    /// If previously released numbers exist, one of them is reused;
    /// otherwise, a new sequential number is generated.
    /// </summary>
    public T Next => _released.Count > 0
        ? _released.Pop()
        : GetNext();

    /// <summary>
    /// Releases a number back into the pool, making it available for reuse.
    /// </summary>
    /// <param name="value">The number to release.</param>
    /// <exception cref="ArgumentException">
    /// Thrown if the value is invalid, i.e. less than zero,
    /// greater than the current counter,
    /// or already present in the released stack.
    /// </exception>
    public void Release(T value)
    {
        if (value < T.Zero || value > _counter || _released.Contains(value))
            throw new ArgumentException("Invalid number to release.", nameof(value));

        _released.Push(value);
    }

    /// <summary>
    /// Generates a new sequential number.
    /// </summary>
    /// <returns>The next unused number.</returns>
    private T GetNext()
    {
        var current = _counter;
        _counter += T.One;
        return current;
    }
}
