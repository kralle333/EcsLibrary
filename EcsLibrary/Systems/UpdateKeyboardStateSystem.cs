using EcsLibrary.Components;
using EcsLibrary.Managers.Objects;
using Microsoft.Xna.Framework.Input;

namespace EcsLibrary.Systems
{
    public class UpdateKeyboardStateSystem : UpdateSystem
    {
        protected override void SetRequiredTypes()
        {
            base.SetRequiredTypes();
            SetRequiredTypes(typeof(KeyboardStateComponent));
        }

        protected override void UpdateEntity(Entity entity)
        {
            var comp = GetComponent<KeyboardStateComponent>(entity);
            comp.UpdateState();
        }
    }
}