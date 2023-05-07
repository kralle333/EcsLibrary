using EcsLibrary.Components;
using EcsLibrary.Managers;
using EcsLibrary.Managers.Objects;

namespace EcsLibrary.GameMaking;

public abstract class ComponentManagerWrapper
{
    protected readonly ComponentManager ComponentManager;
    protected Entity CurrentEntity;

    protected ComponentManagerWrapper(ComponentManager componentManager)
    {
        ComponentManager = componentManager;
    }

    public void SetCurrentEntity(Entity entity)
    {
        CurrentEntity = entity;
    }

    public abstract T AddComponent<T>(T component) where T : Component;
    public abstract T GetComponent<T>() where T : Component;
}

public class ComponentGetter : ComponentManagerWrapper
{
    public ComponentGetter(ComponentManager componentManager) : base(componentManager)
    {
    }

    public override T AddComponent<T>(T component)
    {
        return null;
    }

    public override T GetComponent<T>()
    {
        return ComponentManager.GetComponent<T>(CurrentEntity);
    }
}

public class ComponentAdder : ComponentManagerWrapper
{
    public ComponentAdder(ComponentManager componentManager) : base(componentManager)
    {
    }

    public T AddComponent<T>() where T : Component, new()
    {
        return ComponentManager.AddComponent(CurrentEntity, new T());
    }

    public override T AddComponent<T>(T component)
    {
        return ComponentManager.AddComponent(CurrentEntity, component);
    }

    public override T GetComponent<T>()
    {
        return null;
    }
}