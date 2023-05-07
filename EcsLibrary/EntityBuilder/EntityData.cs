using System;
using System.Linq;
using EcsLibrary.Components;

namespace EcsLibrary
{
    [Serializable]
    public class SubEntityData
    {
        public string entityRef;
        public ComponentOneOf[] values;
    }

    [Serializable]
    public class EntityData
    {
        public ComponentOneOf[] components;
        public SubEntityData[] subEntities;
    }

    [Serializable]
    public class ComponentOneOf
    {
        public AnimatedTexture2DComponent AnimatedTexture2DComponent;
        public ClickableComponent ClickableComponent;
        public CollisionComponent CollisionComponent;
        public TransformComponent TransformComponent;
        public KeyboardStateComponent KeyboardStateComponent;
        public MouseStateComponent MouseStateComponent;
        public NineSliceTexture2DComponent NineSliceTexture2DComponent;
        public SpriteFontComponent SpriteFontComponent;
        public Texture2DComponent Texture2DComponent;

        public virtual Component GetComponent()
        {
            if (AnimatedTexture2DComponent != null)
                return AnimatedTexture2DComponent;
            if (ClickableComponent != null)
                return ClickableComponent;
            if (CollisionComponent != null)
                return CollisionComponent;
            if (TransformComponent != null)
                return TransformComponent;
            if (KeyboardStateComponent != null)
                return KeyboardStateComponent;
            if (MouseStateComponent != null)
                return MouseStateComponent;
            if (NineSliceTexture2DComponent != null)
                return NineSliceTexture2DComponent;
            if (SpriteFontComponent != null)
                return SpriteFontComponent;
            if (Texture2DComponent != null)
                return Texture2DComponent;
            
            return null;
        }
        
    }
}