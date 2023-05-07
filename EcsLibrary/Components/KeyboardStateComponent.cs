using Microsoft.Xna.Framework.Input;

namespace EcsLibrary.Components
{
    public class KeyboardStateComponent : Component
    {
        private KeyboardState _state;
        private KeyboardState _prevState;

        public KeyboardStateComponent()
        {
            UpdateState();
        }

        public bool IsKeyDown(Keys key)
        {
            return _state.IsKeyDown(key);
        }

        public bool IsKeyNewDown(Keys key)
        {
            return _state.IsKeyDown(key) && _prevState.IsKeyUp(key);
        }

        public override void Dispose()
        {
            //Only structs here
        }

        public void UpdateState()
        {
            _state = Keyboard.GetState();
            _prevState = _state;
        }
    }
}
