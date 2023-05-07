using System;
using System.Text;
using EcsLibrary.Components;
using EcsLibrary.Managers.Objects;

namespace EcsLibrary
{
    public struct GameIntent
    {
        public enum Type
        {
            None,
            CreateEntity,
            // CreateFactoryEntity,
            // CreateSubEntity,
            RemoveEntity,
            Pause,
            Resume,
        }

        public Type IntentType { get; set; }

        // CreateEntity
        public Components.Component[] components;
        public Systems.UpdateSystem entityCreatorSystem;
        public Component[][] attachedEntities;
        
        // CreateSubEntity
        public Components.Component[] subEntityComponents;
        public int attachedToEntity;
        public Systems.UpdateSystem subEntityCreatorSystem;
        

        // DeregisterEntity
        public Entity deregistedEntity;

        // Set Paused
        public Systems.UpdateSystem[] allowedSystems;
        
        // Factory build
        public string entityFactoryPath;
    }
}
