using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EcsLibrary.Components
{
    
    [Serializable]
    public abstract class RenderableComponent : Component
    {
        public Color color = Color.White;
        public int depth;

        public override void Dispose()
        {

        }


        public virtual void Draw(SpriteBatch spriteBatch, TransformComponent transform){}

    }
}
