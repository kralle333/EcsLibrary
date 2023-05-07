using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EcsLibrary.Components
{
    public class Texture2DComponent : RenderableComponent
    {
        private string _texturePath;
        private Rectangle _textureRectangle;
        private Texture2D _texture;

        public Texture2DComponent(string texturePath, Rectangle textureRectangle)
        {
            _textureRectangle = textureRectangle;
            _texturePath = texturePath;
        }
        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            _texture = contentManager.Load<Texture2D>(_texturePath);
        }

        public override void Draw(SpriteBatch spriteBatch, TransformComponent transform)
        {
            base.Draw(spriteBatch, transform);
            spriteBatch.Draw(_texture, transform.Pos,_textureRectangle, Color.White, transform.Rotation, transform.Origin, transform.Scale,SpriteEffects.None, 1);
        }
    }
}