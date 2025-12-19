using System;
using System.Collections.Generic;

/// <summary>Event fired when alert starts.</summary>
public struct AlertStartedEvent { }

/// <summary>Event fired when alert stops.</summary>
public struct AlertStoppedEvent { }

/// <summary>Event fired to toggle Bat-Signal.</summary>
public struct BatSignalToggleEvent { }

/// <summary>Event for changing sprite opacity.</summary>
public struct OpacityChangedEvent
{
    public float Alpha;

    /// <summary>Creates opacity change event.</summary>
    public OpacityChangedEvent(float alpha)
    {
        Alpha = alpha;
    }
}

/// <summary>
/// Simple generic event bus for decoupled communication.
/// </summary>
public static class EventBus
{
    private static Dictionary<Type, Action<object>> _events = new();

    /// <summary>Subscribes to an event type.</summary>
    public static void Subscribe<T>(Action<T> callback)
    {
        var type = typeof(T);

        if (!_events.ContainsKey(type))
            _events[type] = delegate { };

        _events[type] += (obj) => callback((T)obj);
    }

    /// <summary>Unsubscribes from an event type.</summary>
    public static void Unsubscribe<T>(Action<T> callback)
    {
        var type = typeof(T);

        if (_events.ContainsKey(type))
            _events[type] -= (obj) => callback((T)obj);
    }

    /// <summary>Publishes an event to all listeners.</summary>
    public static void Publish<T>(T publishedEvent)
    {
        var type = typeof(T);

        if (_events.ContainsKey(type))
            _events[type]?.Invoke(publishedEvent);
    }
}
