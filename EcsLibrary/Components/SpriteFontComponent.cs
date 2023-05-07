using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EcsLibrary.Components
{
    public class SpriteFontComponent : RenderableComponent
    {
        private string _text;
        private string _spriteFontPath;

        private SpriteFont _spriteFont;

        public SpriteFontComponent(string spriteFontPath)
        {
            _spriteFontPath = spriteFontPath;
        }
        public SpriteFontComponent(string spriteFontPath, string text)
        {
            _spriteFontPath = spriteFontPath;
            _text = text;
        }
        public void SetText(string text)
        {
            _text = text;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _spriteFont = contentManager.Load<SpriteFont>(_spriteFontPath);
        }

        public override void OverrideValues(Component other)
        {
            var converted = (SpriteFontComponent) other;
            _text = string.IsNullOrEmpty(converted._text) ? _text : converted._text;
            _spriteFontPath = string.IsNullOrEmpty(_spriteFontPath) ? converted._spriteFontPath : _spriteFontPath;
        }

        public override void Draw(SpriteBatch spriteBatch, TransformComponent transform)
        {
            spriteBatch.DrawString(_spriteFont, _text, transform.Pos, color,0,Vector2.Zero,new Vector2(2,2),SpriteEffects.None,0);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}