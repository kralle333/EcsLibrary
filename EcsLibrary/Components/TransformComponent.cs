using System;
using Microsoft.Xna.Framework;

namespace EcsLibrary.Components
{
    [Serializable]
    public class TransformComponent : Component
    {
        public Vector2 Pos
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vector2 Scale => new Vector2(ScaleX, ScaleY);

        public float X;
        public float Y;
        public float ScaleX = 1;
        public float ScaleY = 1;
        public float Rotation;
        public Vector2 Origin;

        public TransformComponent(float x, float y)
        {
            X = x;
            Y = y;
        }

        public TransformComponent()
        {
        }

        public override void OverrideValues(Component other)
        {
            var casted = (TransformComponent)other;
            X = casted.X != 0 ? casted.X : X;
            Y = casted.Y != 0 ? casted.Y : Y;
            ScaleX = casted.ScaleX != 1 ? casted.ScaleX : ScaleX;
            ScaleY = casted.ScaleY != 1 ? casted.ScaleY : ScaleY;
            Rotation = casted.Rotation != 0 ? casted.Rotation : Rotation;
            Origin = casted.Origin != Vector2.Zero ? casted.Origin : Origin;
        }

        public override void Dispose()
        {
            // nothing to dispose of
        }
    }
}