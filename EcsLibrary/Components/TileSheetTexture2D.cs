using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcsLibrary.Components
{
    public class TileSheetTexture2D : RenderableComponent
    {
        public override void Draw(SpriteBatch spriteBatch, TransformComponent transform)
        {
            base.Draw(spriteBatch, transform);
        }
    }
}
