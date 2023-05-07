using System;
using System.Collections.Generic;
using EcsLibrary.Components;
using EcsLibrary.Managers.Objects;
using Microsoft.Xna.Framework.Content;

namespace EcsLibrary.Managers
{
    public class ComponentManager : IDisposable
    {
        private List<Dictionary<int, Component>> _components = new(30);
        private AspectManager _aspectManager;
        private const int DefaultCapacity = 100;
        private int _renderableTypeId = 0;

        public ComponentManager()
        {
            _aspectManager = new AspectManager();
            SetupLibraryTypes();
        }

        private void SetupLibraryTypes()
        {
            EnsureType<AnimatedTexture2DComponent>();
            EnsureType<ClickableComponent>();
            EnsureType<CollisionComponent>();
            EnsureType<KeyboardStateComponent>();
            EnsureType<MouseStateComponent>();
            EnsureType<NineSliceTexture2DComponent>();
            EnsureType<RectangleTexture2DComponent>();
            _renderableTypeId = EnsureType<RenderableComponent>();
            EnsureType<SpriteFontComponent>();
            EnsureType<Texture2DComponent>();
            EnsureType<TileSheetTexture2D>();
            EnsureType<TransformComponent>();
        }

        public Aspect ConstructAspect(Type[] neededTypes)
        {
            return _aspectManager.ConstructAspect(neededTypes);
        }

        private int EnsureType<T>() where T : Component
        {
            var typeId = _aspectManager.EnsureType<T>();
            while (typeId >= _components.Count)
            {
                _components.Add(new Dictionary<int, Component>());
            }

            return typeId;
        }


        public T AddComponent<T>(Entity entity, T component) where T : Component
        {
            var typeIndex = EnsureType<T>();
            _components[typeIndex][entity.Id] = component;
            entity.Aspect.Add(typeIndex);

            if (typeof(RenderableComponent).IsAssignableFrom(typeof(T)))
            {
                _components[_renderableTypeId][entity.Id] = component;
                entity.Aspect.Add(_renderableTypeId);
            }


            return component;
        }

        public T GetComponent<T>(Entity entity) where T : Component
        {
            var typeIndex = EnsureType<T>();
            var collection = _components[typeIndex];
            if (collection.TryGetValue(entity.Id,out var component))
            {
                return component as T;
            }
            return null;
        }

        public bool HasComponent<T>(Entity entity) where T : Component
        {
            return GetComponent<T>(entity) != null;
        }

        public void LoadContent(ContentManager contentManager)
        {
            foreach (var keyValuePair in _components)
            {
                foreach (var kvp in keyValuePair)
                {
                    kvp.Value.LoadContent(contentManager);
                }
            }
        }

        public void Dispose()
        {
            _components.Clear();
            _aspectManager.Dispose();
        }
    }
}