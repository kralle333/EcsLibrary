using Microsoft.Xna.Framework;
using System.Collections.Generic;
using EcsLibrary.Managers.Objects;

namespace EcsLibrary.Components
{
    public class CollisionComponent : Component
    {
        private readonly int _width = 0;
        private readonly int _height = 0;

        private Rectangle _collisionRectangle;
        private Dictionary<Entity, bool> _collisionData = new Dictionary<Entity, bool>();

        public bool HasCollision = false;

        private CollisionComponent()
        {

        }
        public CollisionComponent(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public override void Dispose()
        {

        }

        public void UpdateRectangle(TransformComponent trans)
        {
            _collisionRectangle = new Rectangle((int)trans.X,(int) trans.Y, _width, _height);
        }

        public bool CheckCollision(CollisionComponent other)
        {
            if (other._collisionRectangle.Intersects(_collisionRectangle))
            {
                HasCollision = true;
                other.HasCollision = true;
            }

            return HasCollision;
        }
    }
}
