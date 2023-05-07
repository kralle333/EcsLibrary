using System.Collections.Generic;
using EcsLibrary.Components;
using EcsLibrary.Managers.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NotImplementedException = System.NotImplementedException;

namespace EcsLibrary.Systems
{
    public class RenderSystem2D : System
    {
        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;

        public RenderSystem2D(SpriteBatch batch, OrthographicCamera camera)
        {
            _spriteBatch = batch;
            _camera = camera;
        }

        public RenderSystem2D()
        {
            
        }

        protected override void SetRequiredTypes()
        {
            SetRequiredTypes(typeof(RenderableComponent), typeof(TransformComponent));
        }

        public void Render(GameTime gameTime)
        {
            if (TickTimer(gameTime))
            {
                RenderEntities(_entities);
            }
        }

        private void RenderEntities(List<Entity> updatedEntities)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.GetViewMatrix());
            foreach (var entity in updatedEntities)
            {
                var renderable = GetComponent<RenderableComponent>(entity);
                var trans = GetComponent<TransformComponent>(entity);
                renderable.Draw(_spriteBatch, trans);
            }

            _spriteBatch.End();
        }
    }
}