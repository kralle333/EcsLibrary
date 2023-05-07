
using EcsLibrary.Components;
using Microsoft.Xna.Framework;

public class ClickableComponent : Component
{
    public bool isClicked;
    public Rectangle rectangle;

    public ClickableComponent(int width, int height)
    {
        rectangle = new Rectangle(0, 0, width, height);
    }
    public override void Dispose()
    {
    }
}