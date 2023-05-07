using System.Collections.Generic;
using EcsLibrary.Components;
using EcsLibrary.Managers.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EcsLibrary.Systems
{
    public class UpdateMouseStateSystem : UpdateSystem
    {
        protected override void SetRequiredTypes()
        {
            base.SetRequiredTypes();
            SetRequiredTypes(typeof(MouseStateComponent));
        }

        protected override void UpdateEntities(List<Entity> updatedEntities, GameTime gameTime)
        {
            foreach (Entity entity in updatedEntities)
            {
                var mouse = GetComponent<MouseStateComponent>(entity);
                mouse.previousMouseState = mouse.currentMouseState;
                mouse.currentMouseState = Mouse.GetState();
            }
        }
    }
}