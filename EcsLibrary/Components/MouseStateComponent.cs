using Microsoft.Xna.Framework.Input;

namespace EcsLibrary.Components
{
    public class MouseStateComponent : Component
    {
        public MouseState currentMouseState;
        public MouseState previousMouseState;
        
        public MouseStateComponent()
        {
            
        }

        public bool LeftClicked()
        {
            return previousMouseState.LeftButton == ButtonState.Released &&
                   currentMouseState.LeftButton == ButtonState.Pressed;
        }
        public override void Dispose()
        {
        }
    }
}