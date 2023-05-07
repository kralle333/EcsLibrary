using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EcsLibrary.GameFlow
{
    // For managing transitions between GameScreens and updating and drawing them
    public class EcsGame : Game
    {
        private readonly Stack<GameScreen> _screenStack = new();
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _transitionSpriteBatch;
        protected bool Initialized { get; private set; }

        private GameScreenTransition _fadeInTransition;
        private GameScreenTransition _fadeOutTransition;
        private Queue<GameScreenTransition> _transitions = new Queue<GameScreenTransition>();

        protected EcsGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            Initialized = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            var w = _graphics.PreferredBackBufferHeight;
            var h = _graphics.PreferredBackBufferHeight;
            var tex = Helpers.Texture2DHelper.CreateTexture(_graphics.GraphicsDevice, Color.White, w, h);
            _fadeInTransition = new FadeInGameScreenTransition(tex, null);
            _fadeOutTransition = new FadeOutGameScreenTransition(tex, null);
            _transitionSpriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                foreach (var screen in _screenStack)
                {
                    screen.Dispose();
                }

                _screenStack.Clear();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (_transitions.TryPeek(out var activeTransition))
            {
                if (activeTransition.Update(gameTime))
                    _transitions.Dequeue();
            }
            else if (_screenStack.TryPeek(out var topScreen))
            {
                topScreen.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _graphics.GraphicsDevice.Clear(Color.Black);

            if (_transitions.TryPeek(out var activeTransition))
            {
                _transitionSpriteBatch.Begin();
                activeTransition.Draw(_transitionSpriteBatch, gameTime);
                _transitionSpriteBatch.End();
            }
            else if (_screenStack.TryPeek(out var topScreen))
            {
                topScreen.Draw(gameTime);
            }
        }

        public void PushScreen(GameScreen screen)
        {
            if (!Initialized)
                throw new Exception(
                    "Attempted to push screen before game was initialized. Push initial screen from Initialize or use SetInitialScreen");
            if (_screenStack.TryPeek(out GameScreen from))
            {
                SetupTransition(from, screen);
            }

            _screenStack.Push(screen);
            screen.Init(_graphics.GraphicsDevice,Content);
        }

        public void PopScreen()
        {
            if (!Initialized)
                throw new Exception("Game not ready");
            if (_screenStack.Count == 0)
                return;
            var from = _screenStack.Pop();
            if (!_screenStack.TryPeek(out GameScreen to))
                return;
            SetupTransition(from, to);
        }

        private void SetupTransition(GameScreen from, GameScreen to)
        {
            _fadeOutTransition.StartTransition(from, 1);
            _transitions.Enqueue(_fadeOutTransition);
            _fadeInTransition.StartTransition(to, 1);
            _transitions.Enqueue(_fadeInTransition);
        }
    }
}