using System;
using System.Collections.Generic;
using EcsLibrary.Managers.Objects;
using EcsLibrary.Systems;
using Microsoft.Xna.Framework;

namespace EcsLibrary.Managers;

public class SystemsManager : IDisposable
{
    private List<UpdateSystem> _updateSystems = new();
    private List<RenderSystem2D> _renderSystems = new();
    private readonly List<UpdateSystem> _allowedSystemWhenPaused = new();
    private readonly ComponentManager _componentManager;

    public SystemsManager(ComponentManager componentManager)
    {
        _componentManager = componentManager;
    }

    public void Resume()
    {
        _allowedSystemWhenPaused.Clear();
    }

    public void Pause(params UpdateSystem[] allowedSystems)
    {
        _allowedSystemWhenPaused.AddRange(allowedSystems);
    }

    public void EnsureUpdateSystem<T>() where T : UpdateSystem, new()
    {
        foreach (var updateSystem in _updateSystems)
        {
            if (updateSystem is T)
            {
                return;
            }
        }

        AddSystem(new T());
    }
    public bool HasRenderSystem2D<T>() where T : RenderSystem2D
    {
        foreach (var updateSystem in _renderSystems)
        {
            if (updateSystem is T)
            {
                return true;
            }
        }

        return false;
    }

    public void AddSystem(UpdateSystem system)
    {
        _updateSystems.Add(system);
        system.SetManager(_componentManager);
    }

    public void AddSystem(RenderSystem2D system2D)
    {
        _renderSystems.Add(system2D);
        system2D.SetManager(_componentManager);
    }

    public void DeregisterSystem(UpdateSystem system)
    {
        _updateSystems.Remove(system);
    }

    public void DeregisterSystem(RenderSystem2D system2D)
    {
        _renderSystems.Remove(system2D);
    }

    public void TryAddEntity(Entity entity)
    {
        foreach (var system in _updateSystems)
        {
            if (system.ShouldProcess(entity))
                system.RegisterEntity(entity);
        }

        foreach (var system in _renderSystems)
        {
            if (system.ShouldProcess(entity))
                system.RegisterEntity(entity);
        }
    }

    public void RemoveEntity(Entity entity)
    {
        foreach (var system in _updateSystems)
        {
            system.DeregisterEntity(entity);
        }

        foreach (var system in _renderSystems)
        {
            system.DeregisterEntity(entity);
        }
    }

    private bool UpdateAllowedSystem(GameTime gameTime)
    {
        if (_allowedSystemWhenPaused.Count == 0)
        {
            return false;
        }

        foreach (var system in _allowedSystemWhenPaused)
        {
            system.Update(gameTime);
        }

        return true;
    }


    public void Update(GameTime gameTime)
    {
        if (UpdateAllowedSystem(gameTime))
            return;
        foreach (var system in _updateSystems)
        {
            system.Update(gameTime);
        }
    }

    public void Draw(GameTime gameTime)
    {
        foreach (var system in _renderSystems)
        {
            system.Render(gameTime);
        }
    }

    public void Dispose()
    {
        foreach (var updateSystem in _updateSystems)
        {
            updateSystem.Dispose();
        }

        foreach (var renderSystem in _renderSystems)
        {
            renderSystem.Dispose();
        }

        _updateSystems = null;
        _renderSystems = null;
    }
}