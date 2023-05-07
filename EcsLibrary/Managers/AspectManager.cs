using System;
using System.Collections.Generic;
using EcsLibrary.Components;
using EcsLibrary.Managers.Objects;

namespace EcsLibrary.Managers;

public class AspectManager : IDisposable
{
    private int _registeredTypesCount = 0;
    private readonly Dictionary<Type, int> _registeredTypes = new();
    
    public int EnsureType<T>() where T : Component
    {
        var componentType = typeof(T);
        return EnsureType(componentType);
    }

    public int EnsureType(Type componentType)
    {
        if (_registeredTypes.TryGetValue(componentType, out var typeId))
            return typeId;

        _registeredTypesCount++;
        typeId = _registeredTypesCount;
        _registeredTypes.Add(componentType, typeId);
        return typeId;
    }

    public Aspect ConstructAspect(params Type[] types)
    {
        Aspect a = new Aspect();
        foreach (var type in types)
        {
            a.Add(EnsureType(type));
        }

        return a;
    }

    public void Dispose()
    {
        _registeredTypes.Clear();
        _registeredTypesCount = 0;
    }
}