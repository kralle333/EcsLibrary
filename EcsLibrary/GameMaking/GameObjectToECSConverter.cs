using System;
using EcsLibrary.Components;
using EcsLibrary.Managers;
using EcsLibrary.Managers.Objects;
using EcsLibrary.Systems;

namespace EcsLibrary.GameMaking;

public class GameObjectToEcsConverter : IDisposable
{
    private readonly EntityManager _entityManager;
    private readonly SystemsManager _systemsManager;
    private readonly ComponentManager _componentManager;

    private readonly ComponentAdder _componentAdder;

    public GameObjectToEcsConverter(EntityManager entityManager, ComponentManager componentManager,
        SystemsManager systemsManager)
    {
        _entityManager = entityManager;
        _componentManager = componentManager;
        _systemsManager = systemsManager;
        
        _componentAdder = new ComponentAdder(_componentManager);
    }

    public Entity ConvertAndAdd<T>(T gameObject) where T : GameObject
    {
        // Construct Entity:
        var entity = _entityManager.NewEntity();
        
        // Add Components
        _componentAdder.SetCurrentEntity(entity);
        gameObject.Init(_componentAdder);
        _componentManager.AddComponent(entity, new TaggedComponent<T>());

        // Construct System and Add Systems
        var s = new ActionSystem(entity.Aspect, gameObject.Update); // TODO: Add type T to actionsystem or call it GameObjectSystem instead
        _systemsManager.AddSystem(s);
        SetupRelatedSystems(entity);
        return entity;
    }

    private void SetupRelatedSystems(Entity entity)
    {
        if(_componentManager.HasComponent<KeyboardStateComponent>(entity))
            _systemsManager.EnsureUpdateSystem<UpdateKeyboardStateSystem>();
        
        if(_componentManager.HasComponent<CollisionComponent>(entity))
            _systemsManager.EnsureUpdateSystem<CollisionSystem>();
    }

    public void Dispose()
    {
    }
}