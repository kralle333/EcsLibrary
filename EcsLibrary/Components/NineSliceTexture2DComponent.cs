using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EcsLibrary.Components
{
    [Serializable]
    public class NineSliceTexture2DComponent : RenderableComponent
    {
        public Rectangle middleSlice;
        public int width;
        public int height;
        public string texturePath;

        private Rectangle[] _slices;
        private Texture2D _texture;
        public NineSliceTexture2DComponent(string texturePath, Rectangle middleSlice,int width, int height)
        {
            this.middleSlice = middleSlice;
            this.texturePath = texturePath;
            this.width = width;
            this.height = height;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            _texture = contentManager.Load<Texture2D>(texturePath);
            _slices = new[]
            {
                new Rectangle(0, 0, middleSlice.Left, _texture.Height),
                new Rectangle(0, 0, _texture.Width, middleSlice.Top),
                new Rectangle(middleSlice.Right, 0, _texture.Width - (middleSlice.Right), _texture.Height),
                new Rectangle(0, middleSlice.Bottom, _texture.Width, _texture.Height-middleSlice.Bottom)
            };
        }

        public override void Dispose()
        {
        }

        public override void Draw(SpriteBatch spriteBatch, TransformComponent transform)
        {
            int x = (int) transform.X;
            int y = (int) transform.Y;

            var left = new Rectangle(x, y, _slices[0].Width, height);
            var top = new Rectangle(x, y, width, _slices[1].Height);
            var right = new Rectangle(x + width-_slices[2].Width, y, _slices[2].Width, height);
            var bottom = new Rectangle(x, y + height - _slices[3].Height, width, _slices[3].Height);
            var middle = new Rectangle(
                x + left.Width,
                y + top.Height,
                width - (left.Width + right.Width),
                height - (top.Height + bottom.Height));
            
            spriteBatch.Draw(_texture,left,_slices[0],Color.White);
            spriteBatch.Draw(_texture,top,_slices[1],Color.White);
            spriteBatch.Draw(_texture,right,_slices[2],Color.White);
            spriteBatch.Draw(_texture,bottom,_slices[3],Color.White);
            spriteBatch.Draw(_texture,middle,middleSlice,Color.White);
            
        }
    }
}