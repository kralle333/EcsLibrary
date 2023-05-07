using System.Collections.Generic;
using EcsLibrary.Components;
using EcsLibrary.Managers.Objects;
using Microsoft.Xna.Framework;

namespace EcsLibrary.Systems
{
    public class TextureAnimationSystem : UpdateSystem
    {
        Dictionary<Entity, double> _animationTimers = new();

        protected override void OnRegistered(Entity e)
        {
            _animationTimers.Add(e, 0);
        }

        protected override void OnDeregistered(Entity e)
        {
            _animationTimers.Remove(e);
        }

        protected override void SetRequiredTypes()
        {
            SetRequiredTypes(typeof(AnimatedTexture2DComponent));
        }

        protected override void UpdateEntities(List<Entity> updatedEntities, GameTime gameTime)
        {
            foreach (var entity in _entities)
            {
                _animationTimers[entity] -= gameTime.ElapsedGameTime.TotalSeconds;
                if (!(_animationTimers[entity] <= 0)) 
                    continue;
                var atlasTexture = GetComponent<AnimatedTexture2DComponent>(entity);
                atlasTexture.NextStep();
                _animationTimers[entity] = atlasTexture.AnimationSpeed();
            }
        }
    }
}