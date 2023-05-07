using System;
using EcsLibrary.Components;
using EcsLibrary.GameFlow;
using EcsLibrary.GameMaking;
using EcsLibrary.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EcsLibraryTester;

public class TestGameObjectsGame : EcsGame
{
    private class TestGameObjectsScreen : GameScreen
    {
        private class PlayerGameObject : GameObject
        {
            private Texture2D _texture2D;

            public PlayerGameObject(Texture2D texture2D):base("player")
            {
                _texture2D = texture2D;
            }

            public override void Init(ComponentAdder componentAdder)
            {
                componentAdder.AddComponent(new TransformComponent(250, 250));
                componentAdder.AddComponent(new RectangleTexture2DComponent(_texture2D));
                componentAdder.AddComponent<KeyboardStateComponent>();
                componentAdder.AddComponent(new CollisionComponent(100, 100));
            }

            public override void Update(ComponentGetter componentGetter)
            {
                const float speed = 2;
                var t = componentGetter.GetComponent<TransformComponent>();
                var keys = componentGetter.GetComponent<KeyboardStateComponent>();
                if (keys.IsKeyDown(Keys.Up))
                    t.Y -= speed;
                if (keys.IsKeyDown(Keys.Down))
                    t.Y += speed;
                if (keys.IsKeyDown(Keys.Left))
                    t.X -= speed;
                if (keys.IsKeyDown(Keys.Right))
                    t.X += speed;
                
                var collision = componentGetter.GetComponent<CollisionComponent>();
                componentGetter.GetComponent<RectangleTexture2DComponent>().color =
                    collision.HasCollision ? Color.Red : Color.Blue;
            }
        }
        private class TextGameObject : GameObject
        {
            private string _text;

            public TextGameObject(string text):base("text")
            {
                _text = text;
            }

            public void SetText(string text)
            {
                _text = text;
            }

            public override void Init(ComponentAdder componentAdder)
            {
                componentAdder.AddComponent(new SpriteFontComponent("textfont", _text));
                componentAdder.AddComponent(new TransformComponent(300, 10));
            }
        }

        protected override void Initialize()
        {
            Add(new PlayerGameObject(Texture2DHelper.CreateTexture(GraphicsDevice, Color.White, 100, 100)));
            Add(new TextGameObject("GameObjects Test!"));
            Add(new GameObject("ground",componentAdder =>
                {
                    componentAdder.AddComponent(new TransformComponent(0, ScreenHeight - 100));
                    componentAdder.AddComponent(new RectangleTexture2DComponent(
                        Texture2DHelper.CreateTexture(GraphicsDevice, Color.Green, ScreenWidth, 100)));
                    componentAdder.AddComponent(new CollisionComponent(ScreenWidth, 100));
                },
                null));
        }
    }

    protected override void Initialize()
    {
        base.Initialize();
        PushScreen(new TestGameObjectsScreen());
    }
}