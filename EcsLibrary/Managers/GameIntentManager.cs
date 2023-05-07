using System;
using System.Collections.Generic;
using System.Diagnostics;
using EcsLibrary.Managers.Objects;

namespace EcsLibrary.Managers;

public class GameIntentManager : IDisposable
{
    private List<GameIntent> _intents = new List<GameIntent>();
    private readonly EntityManager _entityManager;
    private readonly ComponentManager _componentManager;
    private readonly SystemsManager _systemsManager;

    public GameIntentManager(EntityManager entityManager, ComponentManager componentManager,
        SystemsManager systemsManager)
    {
        _entityManager = entityManager;
        _componentManager = componentManager;
        _systemsManager = systemsManager;
    }

    public void RegisterEntityIntent(Components.Component[] components,
        Systems.UpdateSystem entityCreatorSystem,
        params Components.Component[][] attachedEntities)
    {
        GameIntent intent = new GameIntent();
        intent.components = components;
        intent.IntentType = GameIntent.Type.CreateEntity;
        intent.entityCreatorSystem = entityCreatorSystem;
        intent.attachedEntities = attachedEntities;
        Debug.Assert(entityCreatorSystem != null, "empty entityCreatorSystem");

        _intents.Add(intent);
    }

    // public void RegisterEntityToBuild(string entityPath, Systems.UpdateSystem entityCreatorSystem)
    // {
    //     GameIntent intent = new GameIntent();
    //     intent.entityFactoryPath = entityPath;
    //     intent.entityCreatorSystem = entityCreatorSystem;
    //     intent.IntentType = GameIntent.Type.CreateFactoryEntity;
    //     Debug.Assert(entityCreatorSystem != null, "empty entityCreatorSystem");
    //
    //     _intents.Add(intent);
    // }

    // public void RegisterSubEntityIntent(Components.Component[] components,
    //     Systems.UpdateSystem entityCreatorSystem,
    //     int attachedToEntity = 0)
    // {
    //     GameIntent intent = new GameIntent
    //     {
    //         subEntityComponents = components,
    //         attachedToEntity = attachedToEntity,
    //         IntentType = GameIntent.Type.CreateSubEntity,
    //         subEntityCreatorSystem = entityCreatorSystem
    //     };
    //     Debug.Assert(entityCreatorSystem != null, "empty entityCreatorSystem");
    //
    //     _intents.Add(intent);
    // }

    public void DeregisterEntity(Entity entity)
    {
        GameIntent gameIntent = new GameIntent()
        {
            deregistedEntity = entity,
            IntentType = GameIntent.Type.RemoveEntity
        };
        _intents.Add(gameIntent);
    }

    public void AddResumeIntent()
    {
        GameIntent intent = new GameIntent
        {
            IntentType = GameIntent.Type.Resume
        };
        _intents.Add(intent);
    }

    public void AddPauseIntent(params Systems.UpdateSystem[] allowedSystems)
    {
        GameIntent intent = new GameIntent
        {
            allowedSystems = allowedSystems,
            IntentType = GameIntent.Type.Pause
        };
        _intents.Add(intent);
    }


    public void EmptyInbox()
    {
        ProcessGameIntents(_intents);
        _intents.Clear();
    }


    public void ProcessGameIntents(List<GameIntent> gameIntents)
    {
        foreach (var gameIntent in gameIntents)
        {
            switch (gameIntent.IntentType)
            {
                case GameIntent.Type.None:
                    break;
                case GameIntent.Type.CreateEntity:
                {
                    var newEntity = _entityManager.NewEntity();
                    foreach (var gameIntentComponent in gameIntent.components)
                    {
                        _componentManager.AddComponent(newEntity, gameIntentComponent);
                    }
                    _systemsManager.TryAddEntity(newEntity);
                    gameIntent.entityCreatorSystem.OnEntityIntentFinished(newEntity);
                    break;
                }
                // case GameIntent.Type.CreateSubEntity:
                    // var subEntity = _entities[gameIntent.attachedToEntity].AddSubEntity()
                    //     .AddComponents(gameIntent.subEntityComponents);
                    // gameIntent.subEntityCreatorSystem.OnEntityIntentFinished(subEntity);
                    // AddEntity(subEntity);
                    // break;
                case GameIntent.Type.RemoveEntity:
                    _systemsManager.RemoveEntity(gameIntent.deregistedEntity);
                    break;
                // case GameIntent.Type.CreateFactoryEntity:
                // {
                //     var newEntity = _entityManager.NewEntity();
                //     // EntityFactory.BuildEntity(gameIntent.entityFactoryPath,ref newEntity);
                //     _systemsManager.AddEntity(newEntity);
                //     gameIntent.entityCreatorSystem.OnEntityIntentFinished(newEntity);
                //     break;
                // }
                case GameIntent.Type.Pause:
                    _systemsManager.Pause(gameIntent.allowedSystems);
                    break;
                case GameIntent.Type.Resume:
                    _systemsManager.Resume();
                    break;
            }
        }
    }

    public void Dispose()
    {
        
    }
}