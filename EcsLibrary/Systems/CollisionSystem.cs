using System.Collections.Generic;
using EcsLibrary.Components;
using EcsLibrary.Managers.Objects;
using Microsoft.Xna.Framework;

namespace EcsLibrary.Systems
{
    class CollisionSystem : UpdateSystem
    {
        // Add specific collision requirements?
        protected override void SetRequiredTypes()
        {
            SetRequiredTypes(typeof(CollisionComponent), typeof(TransformComponent));
        }

        protected override void UpdateEntities(List<Entity> entities, GameTime gameTime)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var col1 = GetComponent<CollisionComponent>(entities[i]);
                col1.HasCollision = false;
            }

            for (int i = 0; i < entities.Count; i++)
            {
                for (int j = i + 1; j < entities.Count; j++)
                {
                    var trans1 = GetComponent<TransformComponent>(entities[i]);
                    var col1 = GetComponent<CollisionComponent>(entities[i]);
                    col1.UpdateRectangle(trans1);
                    var trans2 = GetComponent<TransformComponent>(entities[j]);
                    var col2 = GetComponent<CollisionComponent>(entities[j]);
                    col2.UpdateRectangle(trans2);
                   
                    col1.CheckCollision(col2);
                }
            }
        }
    }
}