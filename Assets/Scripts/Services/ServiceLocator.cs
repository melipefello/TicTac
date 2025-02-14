using System;
using System.Collections.Generic;

public class ServiceLocator
{
    readonly Dictionary<Type, Lazy<object>> _lazyInstances = new();

    public void Register<T>(Func<T> singletonFactory) where T : class
    {
        if (_lazyInstances.ContainsKey(typeof(T)))
            throw new InvalidOperationException($"{typeof(T)} is already installed.");

        _lazyInstances[typeof(T)] = new Lazy<object>(singletonFactory);
    }

    public T Get<T>() where T : class
    {
        var key = typeof(T);
        if (!_lazyInstances.TryGetValue(key, out var lazyInstance))
            throw new InvalidOperationException($"No singleton registered for {key}.");

        return (T) lazyInstance.Value;
    }
}