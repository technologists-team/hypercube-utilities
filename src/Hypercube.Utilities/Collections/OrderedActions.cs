using System.Collections;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Collections;

/// <summary>
/// Provides a subscribable collection of ordered actions with optional priority values.
/// </summary>
/// <typeparam name="T">The type of argument passed to each action when invoked.</typeparam>
public interface ISubscribableOrderedActions<out T>
{
    /// <summary>
    /// Adds a new action to the collection with an optional priority.
    /// </summary>
    /// <param name="action">The action to add.</param>
    /// <param name="priority">
    /// The priority value used to determine execution order. 
    /// Lower values are executed earlier. 
    /// If <c>null</c>, the action is placed after all prioritized actions.
    /// </param>
    void Add(Action<T> action, int? priority = null);
}

/// <summary>
/// Represents an ordered list of actions that can be subscribed to and executed 
/// in a defined order based on optional priority values.
/// </summary>
/// <typeparam name="T">The type of argument passed to each action when invoked.</typeparam>
[PublicAPI]
public sealed class OrderedActions<T> : IEnumerable<Action<T>>, ISubscribableOrderedActions<T>
{
    private readonly List<Entry> _entries = [];

    /// <summary>
    /// Gets the total number of actions stored in the collection.
    /// </summary>
    public int Count => _entries.Count;

    /// <summary>
    /// Adds a new action to the collection with an optional priority.
    /// </summary>
    /// <param name="action">The action to add.</param>
    /// <param name="priority">
    /// The priority value used to determine execution order. 
    /// Lower values are executed earlier. 
    /// If <c>null</c>, the action is placed after all prioritized actions.
    /// </param>
    public void Add(Action<T> action, int? priority = null)
    {
        var newEntry = new Entry(action, priority);

        // Insert with preserving the order in ascending order of priority
        var inserted = false;
        for (var index = 0; index < _entries.Count; index++)
        {
            var current = _entries[index];

            if (current.Priority is null && priority is not null)
            {
                _entries.Insert(index, newEntry);
                inserted = true;
                break;
            }

            if (current.Priority is null || priority is null || priority >= current.Priority)
                continue;
            
            _entries.Insert(index, newEntry);
            inserted = true;
            break;
        }

        if (!inserted)
            _entries.Add(newEntry);
    }

    /// <summary>
    /// Removes the first occurrence of the specified action from the collection.
    /// </summary>
    /// <param name="action">The action to remove.</param>
    /// <returns><c>true</c> if the action was successfully removed; otherwise, <c>false</c>.</returns>
    public bool Remove(Action<T> action)
    {
        for (var i = 0; i < _entries.Count; i++)
        {
            if (_entries[i].Action != action)
                continue;
            
            _entries.RemoveAt(i);
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// Removes all actions from the collection.
    /// </summary>
    public void Clear()
    {
        _entries.Clear();
    }

    /// <summary>
    /// Invokes all registered actions in order, passing the specified argument to each.
    /// </summary>
    /// <param name="arg">The argument passed to each action.</param>
    public void InvokeAll(T arg)
    {
        for (var i = 0; i < _entries.Count; i++)
        {
            _entries[i].Action(arg);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection of actions.
    /// </summary>
    /// <returns>An enumerator of <see cref="Action{T}"/>.</returns>
    public IEnumerator<Action<T>> GetEnumerator()
    {
        var index = 0;
        while (index < _entries.Count)
        {
            yield return _entries[index].Action;
            index++;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Represents an action entry with an optional priority value.
    /// </summary>
    private readonly struct Entry
    {
        /// <summary>
        /// The action to be executed.
        /// </summary>
        public readonly Action<T> Action;

        /// <summary>
        /// The priority of the action. Lower values are executed earlier.
        /// </summary>
        public readonly int? Priority;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entry"/> struct.
        /// </summary>
        /// <param name="action">The action to store.</param>
        /// <param name="priority">The optional priority for execution order.</param>
        public Entry(Action<T> action, int? priority)
        {
            Action = action;
            Priority = priority;
        }
    }
}
