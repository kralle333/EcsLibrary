using EcsLibrary.Components;
using EcsLibrary.GameFlow;
using EcsLibrary.Helpers;
using EcsLibrary.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EcsLibraryTester;

public class TestTransitioningGame : EcsGame
{
    private class TransitioningFromGameScreen : GameScreen
    {
        protected virtual Color BoxColor => Color.Red;
        protected virtual string Text => "whjat";
        private SpriteBatch _spriteBatch;

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            AddSystem(new ActionSystem(new[] { typeof(TransformComponent), typeof(RectangleTexture2DComponent) },
                (getter) =>
                {
                    var t = getter.GetComponent<TransformComponent>();
                    t.X += 4;
                    if(t.X % 10 == 0)
                    {
                        t.Y += 5;
                    }
                }));

            var e = NewEntity();
            ComponentManager.AddComponent(e, new RectangleTexture2DComponent(Texture2DHelper.CreateTexture(GraphicsDevice, BoxColor, 32, 32)));
            ComponentManager.AddComponent(e, new TransformComponent(200, 100));
            AddEntity(e);

            var e2 = NewEntity();
            var c = new SpriteFontComponent("textfont");
            c.SetText(Text);
            c.color = BoxColor;
            ComponentManager.AddComponent(e2, c);
            ComponentManager.AddComponent(e2, new TransformComponent(300, 50));
            AddEntity(e2);
            AddDisposableResource(_spriteBatch);
        }

        public override void Dispose()
        {
            base.Dispose();
            _spriteBatch.Dispose();
        }
    }

    private class FromGameScreen : TransitioningFromGameScreen
    {
        protected override Color BoxColor => Color.Red;
        protected override string Text => "Hello";
    }

    private class ToGameScreen : TransitioningFromGameScreen
    {
        protected override Color BoxColor => Color.Green;
        protected override string Text => "World!";
    }

    private float timer = 0;

    protected override void Initialize()
    {
        base.Initialize();
        PushScreen(new FromGameScreen());
        PushScreen(new ToGameScreen());
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer > 5)
        {
            timer = 0;
            PopScreen();
        }
    }
}