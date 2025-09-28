using Hypercube.Utilities.Collections;

namespace Hypercube.Utilities.UnitTests.Collections;

[TestFixture]
public sealed class OrderedActionsTests
{
    [Test]
    public void Add()
    {
        var actions = new OrderedActions<int>();
        var called = false;

        actions.Add(_ => called = true);

        Assert.That(actions.Count, Is.EqualTo(1));
        
        actions.InvokeAll(42);

        Assert.That(called, Is.True);
    }

    [Test]
    public void InvokeAll()
    {
        var executionOrder = new List<string>();
        var actions = new OrderedActions<int>
        {
            _ => executionOrder.Add("third"),
            { _ => executionOrder.Add("second"), 10 },
            { _ => executionOrder.Add("first"), 1 }
        };

        actions.InvokeAll(0);

        Assert.That(executionOrder, Is.EqualTo(new[] { "first", "second", "third" }));
    }

    [Test]
    public void InvokeAllWithArguments()
    {
        var executionOrder = new List<int>();
        var actions = new OrderedActions<int>
        {
            { x => executionOrder.Add(x + 1), 1 },
            { x => executionOrder.Add(x + 2), 2 }
        };

        actions.InvokeAll(10);

        Assert.That(executionOrder, Is.EqualTo(new[] { 11, 12 }));
    }

    [Test]
    public void AddWithSamePriority()
    {
        var executionOrder = new List<string>();
        var actions = new OrderedActions<int>
        {
            { _ => executionOrder.Add("first"), 1 },
            { _ => executionOrder.Add("second"), 1 }
        };

        actions.InvokeAll(0);

        Assert.That(executionOrder, Is.EqualTo(new[] { "first", "second" }));
    }

    [Test]
    public void RemoveExistingAction()
    {
        var actions = new OrderedActions<int>();
        Action<int> target = _ => { };

        actions.Add(target);

        var result = actions.Remove(target);

        Assert.That(result, Is.True);
        Assert.That(actions.Count, Is.EqualTo(0));
    }

    [Test]
    public void RemoveNonExistingAction()
    {
        var actions = new OrderedActions<int>();

        var result = actions.Remove(_ => { });

        Assert.That(result, Is.False);
    }

    [Test]
    public void Clear()
    {
        var actions = new OrderedActions<int>();
        actions.Add(_ => { });
        actions.Add(_ => { });

        actions.Clear();

        Assert.That(actions.Count, Is.EqualTo(0));
    }

    [Test]
    public void Enumerator()
    {
        var actions = new OrderedActions<int>();
        Action<int>[] targets = [_ => { }, _ => { }];

        foreach (var target in targets)
            actions.Add(target);

        var index = 0;
        foreach (var action in actions)
        {
            Assert.That(action, Is.SameAs(targets[index]));
            index++;
        }


        Assert.That(index, Is.EqualTo(targets.Length));
    }
}
