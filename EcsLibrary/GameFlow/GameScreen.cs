using System;
using System.Collections.Generic;
using EcsLibrary.GameMaking;
using EcsLibrary.Managers;
using EcsLibrary.Managers.Objects;
using EcsLibrary.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EcsLibrary.GameFlow
{
    public abstract class GameScreen
    {
        public virtual string DebugName => ToString();

        protected ComponentManager ComponentManager { get; private set; }
        private SystemsManager _systemsManager;
        private EntityManager _entityManager;
        private GameIntentManager _gameIntentManager;
        private GameObjectToEcsConverter _gameObjectToEcsConverter;
        private List<IDisposable> _disposableResources = new();

        protected GraphicsDevice GraphicsDevice { get; private set; }
        protected ContentManager ContentManager { get; private set; }

        public int ScreenWidth => GraphicsDevice.Viewport.Width;
        public int ScreenHeight => GraphicsDevice.Viewport.Height;

        public void Init(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            GraphicsDevice = graphicsDevice;
            ContentManager = contentManager;

            // init managers
            ComponentManager = new ComponentManager();
            _systemsManager = new SystemsManager(ComponentManager);
            _entityManager = new EntityManager();
            _gameIntentManager = new GameIntentManager(_entityManager, ComponentManager, _systemsManager);
            _gameObjectToEcsConverter = new GameObjectToEcsConverter(_entityManager, ComponentManager, _systemsManager);
            
            AddDisposableResources(ComponentManager, _systemsManager, _entityManager, _gameIntentManager,_gameObjectToEcsConverter);

            // Init and load gamescreen
            _systemsManager.AddSystem(new RenderSystem2D(new SpriteBatch(graphicsDevice),new OrthographicCamera(graphicsDevice.Viewport)));
            Initialize();
            LoadContent();
        }

        protected virtual void Initialize()
        {
        }

        protected void AddDisposableResource(IDisposable resource)
        {
            _disposableResources.Add(resource);
        }

        protected void AddDisposableResources(params IDisposable[] resources)
        {
            _disposableResources.AddRange(resources);
        }

        public virtual void Dispose()
        {
            foreach (var disposableResource in _disposableResources)
            {
                disposableResource.Dispose();
            }

            _disposableResources.Clear();
            GC.Collect();
        }

        public virtual void Update(GameTime gameTime)
        {
            _gameIntentManager.EmptyInbox();
            _systemsManager.Update(gameTime);
        }

        protected Entity NewEntity()
        {
            return _entityManager.NewEntity();
        }

        public void Add(GameObject gameObject)
        {
            AddEntity(_gameObjectToEcsConverter.ConvertAndAdd(gameObject));
        }

        public void AddEntity(Entity entity)
        {
            _systemsManager.TryAddEntity(entity);
        }

        public void RemoveEntity(Entity entity)
        {
        }

        public void AddSystem(UpdateSystem system)
        {
            _systemsManager.AddSystem(system);
        }

        public void AddSystem(RenderSystem2D system2D)
        {
            _systemsManager.AddSystem(system2D);
        }

        public void RemoveSystem(UpdateSystem system)
        {
            _systemsManager.DeregisterSystem(system);
            system.Dispose();
        }

        public void RemoveSystem(RenderSystem2D system2D)
        {
            _systemsManager.DeregisterSystem(system2D);
            system2D.Dispose();
        }

        private void LoadContent()
        {
            ComponentManager.LoadContent(ContentManager);
        }

        public void Draw(GameTime gameTime)
        {
            _systemsManager.Draw(gameTime);
        }
    }
}