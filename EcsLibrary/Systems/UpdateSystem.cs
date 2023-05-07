using System;
using System.Collections.Generic;
using System.Diagnostics;
using EcsLibrary.Managers.Objects;
using Microsoft.Xna.Framework;

namespace EcsLibrary.Systems
{
    public class UpdateSystem : System
    {
        public void Update(GameTime gameTime)
        {
            if (TickTimer(gameTime))
            {
                UpdateEntities(_entities, gameTime);
            }
        }

        protected virtual void UpdateEntities(List<Entity> updatedEntities, GameTime gameTime)
        {
            foreach (var updated in updatedEntities)
            {
                UpdateEntity(updated);
            }
        }


        protected virtual void UpdateEntity(Entity entity)
        {
        }
    }
}