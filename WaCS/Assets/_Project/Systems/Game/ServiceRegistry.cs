using System.Collections.Generic;
using System;

public static class ServiceRegistry
{
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static void Register<TService>(TService service)
    {
        _services[typeof(TService)] = service;
    }

    public static TService Get<TService>()
    {
        if(_services.TryGetValue(typeof(TService), out var service))
        {
            return (TService)service;
        }
        throw new InvalidOperationException($"Service {typeof(TService).Name} not registered");
    }

    public static bool TryGet<TService>(out TService service)
    {
        if(_services.TryGetValue(typeof(TService), out var s))
        {
            service = (TService)s;
            return true;
        }
        service = default(TService);
        return false;
    }
}
