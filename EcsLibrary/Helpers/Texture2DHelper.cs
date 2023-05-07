using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EcsLibrary.Helpers;

public static class Texture2DHelper
{
    public static Texture2D CreateTexture(GraphicsDevice graphics, Color color, int width, int height)
    {
        var bgColorData = new Color[width * height];
        for (int i = 0; i < bgColorData.Length; i++)
        {
            bgColorData[i] = color;
        }

        var texture2D = new Texture2D(graphics, width, height);
        texture2D.SetData(bgColorData);
        return texture2D;
    }
}