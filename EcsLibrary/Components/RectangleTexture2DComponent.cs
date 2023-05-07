using System;
using EcsLibrary.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EcsLibrary.Components
{
    [Serializable]
    public class RectangleTexture2DComponent : RenderableComponent
    {
        private Texture2D _texture2D;
        public RectangleTexture2DComponent(Texture2D texture2D)
        {
            _texture2D = texture2D;
        }
        public override void Draw(SpriteBatch spriteBatch, TransformComponent transform)
        {
            spriteBatch.Draw(_texture2D, transform.Pos,new Rectangle(0,0,_texture2D.Width,_texture2D.Height), Color.White, transform.Rotation, transform.Origin, transform.Scale,SpriteEffects.None, 1);

        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(_texture2D, position, color);
        }

        public override void Dispose()
        {
            _texture2D?.Dispose();
            _texture2D = null;
        }
    }
}
