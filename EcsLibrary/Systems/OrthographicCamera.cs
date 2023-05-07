using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EcsLibrary.Systems
{
    public class OrthographicCamera : System
    {
        private readonly Viewport _viewport;
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; }
        public float Rotation { get; }

        public float Zoom { get; } = 1;

        public void Translate(float x, float y)
        {
            Position = new Vector2(Position.X + x, Position.Y + y);
        }

        public OrthographicCamera(Viewport viewport)
        {
            _viewport = viewport;
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-this.Position, 0.0f)) *
                   Matrix.CreateTranslation(new Vector3(-this.Origin, 0.0f)) * Matrix.CreateRotationZ(this.Rotation) *
                   Matrix.CreateScale(this.Zoom, this.Zoom, 1f) *
                   Matrix.CreateTranslation(new Vector3(this.Origin, 0.0f));
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition - new Vector2((float) _viewport.X, (float) _viewport.Y),
                Matrix.Invert(this.GetViewMatrix()));
        }
    }
}