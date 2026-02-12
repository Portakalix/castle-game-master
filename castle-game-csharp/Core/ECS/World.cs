using System.Collections.Generic;

namespace CastleGame.Core.ECS;

/// <summary>
/// The World contains all entities, components, systems, and resources
/// </summary>
public class World
{
    private readonly List<Entity> _entities = new();
    private readonly Dictionary<Type, Dictionary<Entity, Component>> _components = new();
    private readonly List<GameSystem> _systems = new();
    private readonly Dictionary<Type, object> _resources = new();
    private readonly Queue<Entity> _entitiesToDestroy = new();

    public IReadOnlyList<Entity> Entities => _entities;

    public Entity CreateEntity()
    {
        var entity = Entity.Create();
        _entities.Add(entity);
        return entity;
    }

    public void DestroyEntity(Entity entity)
    {
        _entitiesToDestroy.Enqueue(entity);
    }

    public void AddComponent<T>(Entity entity, T component) where T : Component
    {
        var type = typeof(T);
        if (!_components.ContainsKey(type))
        {
            _components[type] = new Dictionary<Entity, Component>();
        }
        _components[type][entity] = component;
    }

    public T? GetComponent<T>(Entity entity) where T : Component
    {
        var type = typeof(T);
        if (_components.TryGetValue(type, out var storage))
        {
            if (storage.TryGetValue(entity, out var component))
            {
                return (T)component;
            }
        }
        return null;
    }

    public bool HasComponent<T>(Entity entity) where T : Component
    {
        var type = typeof(T);
        return _components.ContainsKey(type) && _components[type].ContainsKey(entity);
    }

    public bool RemoveComponent<T>(Entity entity) where T : Component
    {
        var type = typeof(T);
        if (_components.TryGetValue(type, out var storage))
        {
            return storage.Remove(entity);
        }
        return false;
    }

    public IEnumerable<(Entity, T)> GetEntitiesWithComponent<T>() where T : Component
    {
        var type = typeof(T);
        if (_components.TryGetValue(type, out var storage))
        {
            foreach (var kvp in storage)
            {
                yield return (kvp.Key, (T)kvp.Value);
            }
        }
    }

    public void AddResource<T>(T resource) where T : class
    {
        _resources[typeof(T)] = resource;
    }

    public T? GetResource<T>() where T : class
    {
        if (_resources.TryGetValue(typeof(T), out var resource))
        {
            return (T)resource;
        }
        return null;
    }

    public void AddSystem(GameSystem system)
    {
        system.Initialize(this);
        _systems.Add(system);
    }

    public void Update(float deltaTime)
    {
        foreach (var system in _systems)
        {
            system.Update(deltaTime);
        }

        // Process entity deletions
        Maintain();
    }

    public void Maintain()
    {
        while (_entitiesToDestroy.Count > 0)
        {
            var entity = _entitiesToDestroy.Dequeue();
            
            // Remove all components from this entity
            foreach (var storage in _components.Values)
            {
                storage.Remove(entity);
            }

            _entities.Remove(entity);
        }
    }
}
