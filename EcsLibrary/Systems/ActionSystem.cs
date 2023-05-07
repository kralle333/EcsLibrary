using System;
using EcsLibrary.GameMaking;
using EcsLibrary.Managers.Objects;

namespace EcsLibrary.Systems;

public class ActionSystem : UpdateSystem
{
    private Action<ComponentGetter> _updateAction;
    private ComponentGetter _componentGetter;

    public ActionSystem(Type[] types, Action<ComponentGetter> updateAction)
    {
        _updateAction = updateAction;
        SetRequiredTypes(types);
    }
    public ActionSystem(Aspect aspect, Action<ComponentGetter> updateAction)
    {
        _updateAction = updateAction;
        NeededAspect = aspect;
        if (NeededAspect.IsEmpty())
        {
            throw new Exception("Aspect with at least one type must be set!");
        }
    }

    protected override void SetRequiredTypes()
    {
        // done here instead of constructor as ComponentManager is set at this point
        _componentGetter = new ComponentGetter(_componentManager);
    }

    protected override void UpdateEntity(Entity entity)
    {
        _componentGetter.SetCurrentEntity(entity);
        _updateAction(_componentGetter);
    }
}