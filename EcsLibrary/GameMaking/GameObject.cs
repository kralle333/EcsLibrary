using System;
using EcsLibrary.Components;
using EcsLibrary.Managers;

namespace EcsLibrary.GameMaking;

public class GameObject
{
    private Action<ComponentAdder> _initAction;
    private Action<ComponentGetter> _updateAction;
    private string _name;

    protected GameObject(string name)
    {
        _name = name;
    }

    public GameObject(string name, Action<ComponentAdder> initAction, Action<ComponentGetter> updateAction)
    {
        _name = name;
        _initAction = initAction;
        _updateAction = updateAction;
    }
    
    public virtual void Init(ComponentAdder componentAdder)
    {
        _initAction?.Invoke(componentAdder);
    }

    public virtual void Update(ComponentGetter componentGetter)
    {
        _updateAction?.Invoke(componentGetter);
    }
}