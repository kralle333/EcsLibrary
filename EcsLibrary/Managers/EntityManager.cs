using System;
using System.Collections.Generic;
using EcsLibrary.Managers.Objects;

namespace EcsLibrary;

public class EntityManager : IDisposable
{
    private int _entityIdCounter;

    public EntityManager()
    {
        
    }
    
    private int GetEntityId()
    {
        _entityIdCounter++;
        return _entityIdCounter;
    }
    
    public Entity NewEntity()
    { 
        return new Entity(GetEntityId());
    }

    public void Dispose()
    {
        _entityIdCounter = 0;
    }
}