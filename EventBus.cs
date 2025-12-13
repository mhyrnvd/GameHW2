using System;
using System.Collections.Generic;

public struct AlertStartedEvent { }
public struct AlertStoppedEvent { }

public struct BatSignalToggleEvent { }

public struct OpacityChangedEvent
{
    public float Alpha;

    public OpacityChangedEvent(float alpha)
    {
        Alpha = alpha;
    }
}

public static class EventBus
{
    private static Dictionary<Type, Action<object>> _events = new();

    public static void Subscribe<T>(Action<T> callback)
    {
        var type = typeof(T);

        if (!_events.ContainsKey(type))
            _events[type] = delegate { };

        _events[type] += (obj) => callback((T)obj);
    }

    public static void Unsubscribe<T>(Action<T> callback)
    {
        var type = typeof(T);

        if (_events.ContainsKey(type))
            _events[type] -= (obj) => callback((T)obj);
    }

    public static void Publish<T>(T publishedEvent)
    {
        var type = typeof(T);

        if (_events.ContainsKey(type))
            _events[type]?.Invoke(publishedEvent);
    }
}
