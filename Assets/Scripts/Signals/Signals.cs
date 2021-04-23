using System;
using System.Collections.Generic;


public static class Signals
{
    private static readonly SignalHub hub = new SignalHub();

    public static SType Get<SType>() where SType : ISignal, new()
    {
        return hub.Get<SType>();
    }

    public static void Register<T>() where T : ISignal
    {
        hub.Register<T>();
    }
}

public class SignalHub
{
    private Dictionary<Type, ISignal> signals = new Dictionary<Type, ISignal>();

    public SType Get<SType>() where SType : ISignal, new()
    {
        return (SType)signals[typeof(SType)];   
    }

    public void Register<T>() where T : ISignal
    {
        Type signalType = typeof(T);
        ISignal signal;

        if(signals.TryGetValue(signalType, out signal))
        {
            UnityEngine.Debug.LogError(string.Format("Signal already registered for type {0}", signalType.ToString()));
        }

        signal = (ISignal)Activator.CreateInstance(signalType);
        signals.Add(signalType, signal);
    }
}

public interface ISignal
{
}

public abstract class Signal : ISignal
{
    private Action callback;

    public void AddListener(Action handler)
    {
        #if UNITY_EDITOR
        UnityEngine.Debug.Assert(handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), inherit: false).Length == 0,
            "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
        #endif
        callback += handler;
    }

    public void RemoveListener(Action handler)
    {
        callback -= handler;
    }

    public void Invoke()
    {
        if(callback != null)
        {
            callback();
        }
    }
}

public abstract class Signal<T>: ISignal
{
    private Action<T> callback;

    public void AddListener(Action<T> handler)
    {
        #if UNITY_EDITOR
        UnityEngine.Debug.Assert(handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), inherit: false).Length == 0,
            "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
        #endif
        callback += handler;
    }

    public void RemoveListener(Action<T> handler)
    {
        callback -= handler;
    }

    public void Invoke(T arg1)
    {
        if (callback != null)
        {
            callback(arg1);
        }
    }
}

public abstract class Signal<T, U>: ISignal
{
    private Action<T, U> callback;

    public void AddListener(Action<T, U> handler)
    {
        #if UNITY_EDITOR
        UnityEngine.Debug.Assert(handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), inherit: false).Length == 0,
            "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
        #endif
        callback += handler;
    }

    public void RemoveListener(Action<T, U> handler)
    {
        callback -= handler;
    }

    public void Invoke(T arg1, U arg2)
    {
        if (callback != null)
        {
            callback(arg1, arg2);
        }
    }
}

public abstract class Signal<T, U, V>: ISignal
{
    private Action<T, U, V> callback;

    public void AddListener(Action<T, U, V> handler)
    {
        #if UNITY_EDITOR
        UnityEngine.Debug.Assert(handler.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), inherit: false).Length == 0,
            "Adding anonymous delegates as Signal callbacks is not supported (you wouldn't be able to unregister them later).");
        #endif
        callback += handler;
    }

    public void RemoveListener(Action<T, U, V> handler)
    {
        callback -= handler;
    }

    public void Invoke(T arg1, U arg2, V arg3)
    {
        if (callback != null)
        {
            callback(arg1, arg2, arg3);
        }
    }
}