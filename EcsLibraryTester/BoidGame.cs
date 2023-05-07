using System;
using System.Collections.Generic;
using System.Numerics;
using EcsLibrary.Components;
using EcsLibrary.GameFlow;
using EcsLibrary.Helpers;
using EcsLibrary.Managers.Objects;
using EcsLibrary.Systems;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace EcsLibraryTester;

public class BoidGame : EcsLibrary.GameFlow.EcsGame
{
    protected override void Initialize()
    {
        base.Initialize();
        PushScreen(new BoidGameScreen());
    }

    private class BoidGameScreen : GameScreen
    {
        protected override void Initialize()
        {
            base.Initialize();
            AddSystem(new NeighboursSystem(100));
            AddSystem(new BoidSystem(ScreenWidth,ScreenHeight));

            Random r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var e = NewEntity();
                ComponentManager.AddComponent(e, new TransformComponent(
                    r.Next(0, ScreenWidth),
                    r.Next(0, ScreenHeight)));
                ComponentManager.AddComponent(e, new Physics2DComponent(10, 5));
                ComponentManager.AddComponent(e, new NeighbourComponent());
                ComponentManager.AddComponent(e, new RectangleTexture2DComponent(Texture2DHelper.CreateTexture(GraphicsDevice,Color.Blue,8,8)));
                AddEntity(e);
            }
        }
    }

    private class NeighbourComponent : Component
    {
        public struct Neighbour
        {
            public Vector2 Pos;
            public Vector2 Vel;

            public Neighbour(Vector2 pos, Vector2 vel)
            {
                Pos = pos;
                Vel = vel;
            }
        }

        public readonly List<Neighbour> Neighbours = new List<Neighbour>();

        public void Clear()
        {
            Neighbours.Clear();
        }

        public void AddNeighbour(Vector2 pos, Vector2 vel)
        {
            Neighbours.Add(new Neighbour(pos, vel));
        }
    }

    private class NeighboursSystem : UpdateSystem
    {
        private readonly float _maxDistance;

        public NeighboursSystem(float maxDistance)
        {
            SetProcessPerFrame(30);
            _maxDistance = maxDistance;
        }

        protected override void SetRequiredTypes()
        {
            base.SetRequiredTypes();
            SetRequiredTypes(typeof(TransformComponent), typeof(Physics2DComponent), typeof(NeighbourComponent));
        }

        protected override void UpdateEntities(List<Entity> updatedEntities, GameTime gameTime)
        {
            foreach (var e in updatedEntities)
            {
                GetComponent<NeighbourComponent>(e).Clear();
            }
            for (int i = 0; i < updatedEntities.Count - 1; i++)
            {
                var (t1, n1, p1) = GetComponents(updatedEntities[i]);
                for (int j = i + 1; j < updatedEntities.Count; j++)
                {
                    var (t2, n2, p2) = GetComponents(updatedEntities[j]);
                    var distance = Vector2.Distance(t1.Pos, t2.Pos);
                    if (distance < _maxDistance)
                    {
                        n1.AddNeighbour(t2.Pos,p2.Velocity);
                        n2.AddNeighbour(t1.Pos,p1.Velocity);
                    }
                }
            }
        }

        private (TransformComponent, NeighbourComponent, Physics2DComponent) GetComponents(Entity e)
        {
            return (GetComponent<TransformComponent>(e),
                GetComponent<NeighbourComponent>(e),
                GetComponent<Physics2DComponent>(e));
        }
    }

    private class BoidSystem : UpdateSystem
    {
        private int _width;
        private int _height;

        public BoidSystem(int width, int height)
        {
            _width = width;
            _height = height;
            SetProcessPerFrame(10);
        }
        protected override void SetRequiredTypes()
        {
            base.SetRequiredTypes();
            SetRequiredTypes(typeof(TransformComponent), typeof(Physics2DComponent), typeof(NeighbourComponent));
        }

        protected override void UpdateEntity(Entity entity)
        {
            var n = GetComponent<NeighbourComponent>(entity);
            if (n.Neighbours.Count == 0)
                return;

            var t = GetComponent<TransformComponent>(entity);
            var p = GetComponent<Physics2DComponent>(entity);


            Vector2 cohesion = Vector2.Zero;
            float cWeight = 1;
            Vector2 separation = Vector2.Zero;
            float sWeight = 2f;
            Vector2 alignment = Vector2.Zero;
            float aWeight = 1f;

            var center = Vector2.Zero;
            foreach (var neighbour in n.Neighbours)
            {
                center += neighbour.Pos;
            }

            center /= n.Neighbours.Count;

            cohesion = (t.Pos - center);
            if (cohesion.Length() > 0)
            {
                cohesion.Normalize();
            }

            foreach (var neighbour in n.Neighbours)
            {
                var toNeighbour = t.Pos - neighbour.Pos;
                var distToNeighbour = toNeighbour.Length();
                toNeighbour.Normalize();
                separation += toNeighbour / distToNeighbour;
            }
            if (separation.Length() > 0)
            {
                separation.Normalize();
            }
            
            foreach (var neighbour in n.Neighbours)
            {
                alignment += neighbour.Vel;
            }

            var alignmentLength = alignment.Length();
            if (alignmentLength > 0)
            {
                alignment.Normalize();
            }

            p.SetAcceleration(cohesion*cWeight + separation*sWeight + alignment*aWeight);
            p.Update();
            t.Pos += p.Velocity;
            t.Pos = new Vector2(t.Pos.X % _width, t.Pos.Y % _height);
        }
    }
}