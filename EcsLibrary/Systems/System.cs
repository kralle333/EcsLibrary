using System;
using System.Collections.Generic;
using EcsLibrary.Components;
using EcsLibrary.Managers;
using EcsLibrary.Managers.Objects;
using Microsoft.Xna.Framework;

namespace EcsLibrary.Systems
{
    public abstract class System // TODO: Figure out if I can use Type system System<T> for figuring out entities easily
    {
        protected bool _showUpdateTime = false;
        private Aspect _neededAspect;
        protected Aspect NeededAspect
        {
            get => _neededAspect;
            set => _neededAspect = value;
        }

        protected List<Entity> _entities = new List<Entity>();
        private double _processTimer;
        protected ComponentManager _componentManager;
        private AspectManager _aspectManager;

        public void RegisterEntity(Entity e)
        {
            _entities.Add(e);
            OnRegistered(e);
        }

        public void DeregisterEntity(Entity e)
        {
            _entities.Remove(e);
            OnDeregistered(e);
        }

        public T GetComponent<T>(Entity entity) where T : Component
        {
            return _componentManager.GetComponent<T>(entity);
        }

        protected virtual void OnRegistered(Entity e)
        {
        }

        protected virtual void OnDeregistered(Entity e)
        {
        }

        private const double TargetFramesPerSecond = 60;
        private double _processPerFrame;

        public void SetManager(ComponentManager componentManager)
        {
            _componentManager = componentManager;
            SetRequiredTypes();
        }

        protected virtual void SetRequiredTypes()
        {
        }

        protected void SetRequiredTypes(params Type[] neededTypes)
        {
            _neededAspect = _componentManager.ConstructAspect(neededTypes);
        }

        public void SetProcessPerFrame(double processPerFrame)
        {
            Console.WriteLine($"[{this}] Changing processing per frame {_processPerFrame}->{processPerFrame}");
            _processPerFrame = processPerFrame;
        }


        public bool ShouldProcess(Entity entity)
        {
            return _neededAspect.Overlaps(entity.Aspect);
        }


        public virtual void OnEntityIntentFinished(Entity entity)
        {
        }

        protected bool TickTimer(GameTime gameTime)
        {
            _processTimer += gameTime.ElapsedGameTime.TotalSeconds;
            bool isReady = _processTimer >= _processPerFrame / TargetFramesPerSecond;
            if (isReady)
            {
                _processTimer = 0;
            }

            return isReady;
        }

        public void Dispose()
        {
            _entities.Clear();
            _entities = null;
        }
    }
}