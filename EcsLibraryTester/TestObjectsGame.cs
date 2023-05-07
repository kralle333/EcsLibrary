using System;
using EcsLibrary.Components;
using EcsLibrary.GameFlow;
using EcsLibrary.GameMaking;
using EcsLibrary.Helpers;
using EcsLibrary.Managers.Objects;
using EcsLibrary.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EcsLibraryTester;

public class TestObjectsGame : EcsGame
{
    class RotatorSystem : UpdateSystem
    {
        private Random random = new Random();

        protected override void SetRequiredTypes()
        {
            SetRequiredTypes(typeof(TransformComponent));
        }

        protected override void UpdateEntity(Entity entity)
        {
            base.UpdateEntity(entity);
            var trans = GetComponent<TransformComponent>(entity);
            trans.Rotation += (float)(random.NextDouble()*Math.PI*2)/12;
            if (trans.Rotation >= MathF.PI*2)
            {
                trans.Rotation = 0;
            }
        }
    }

    private class TestObjectsScreen : GameScreen
    {
        protected override void Initialize()
        {
            base.Initialize();
            AddSystem(new RotatorSystem());
            Random r = new Random();
            for (int i = 0; i < 2000; i++)
            {
                var randomColor = new Color((uint)r.NextInt64(0, 0xFFFFFF));
                var texture = Texture2DHelper.CreateTexture(GraphicsDevice, randomColor, 10, 5);
                var randomX = r.Next(0, 720);
                var randomY = r.Next(0, 480);
                Add(new TestObject(texture, new Vector2(randomX, randomY)));
            }
        }
    }

    private class TestObject : GameObject
    {
        private Texture2D _texture2D;
        private Vector2 _startPosition;

        public TestObject(Texture2D texture2D, Vector2 startPosition) : base("player")
        {
            _texture2D = texture2D;
            _startPosition = startPosition;
        }

        public override void Init(ComponentAdder componentAdder)
        {
            componentAdder.AddComponent(new TransformComponent(_startPosition.X, _startPosition.Y));
            componentAdder.AddComponent(new RectangleTexture2DComponent(_texture2D));
        }
    }

    protected override void Initialize()
    {
        base.Initialize();
        PushScreen(new TestObjectsScreen());
    }
}