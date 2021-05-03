using ECS.Components;
using ECS.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace ECS.Core
{
    public class World
    {
        private readonly int maxEntities;

        private readonly Dictionary<Type, IComponentCollection> components;

        private readonly Queue<int> freeIds;

        /// <summary>
        /// Initialises a new World, which stores the Components of all Entities.
        /// </summary>
        /// <param name="maxEntities">The maximum amount of Entities permitted to exist at any time.</param>
        public World(int maxEntities)
        {
            this.maxEntities = maxEntities;

            components = new Dictionary<Type, IComponentCollection>();

            freeIds = new Queue<int>();

            for (int i = 0; i < maxEntities; i++)
            {
                freeIds.Enqueue(i);
            }
        }

        /// <summary>
        /// Creates a new Entity by dequeueing the next available ID from the free ID queue.
        /// </summary>
        /// <returns>The ID of the new Entity.</returns>
        public int CreateEntity()
        {
            if (freeIds.Count == 0)
            {
                throw new EntityLimitExceededException(maxEntities);
            }

            return freeIds.Dequeue();
        }

        /// <summary>
        /// Destroys the Entity of the given ID, which also destroys all of its Components.
        /// </summary>
        /// <param name="entity">The ID of the Entity to be destroyed.</param>
        public void DestroyEntity(int entity)
        {
            foreach (IComponentCollection collection in components.Values)
            {
                collection.Remove(entity);
            }

            freeIds.Enqueue(entity);
        }

        /// <summary>
        /// Adds a the given Component to the Entity of the given ID.
        /// </summary>
        /// <typeparam name="T">The type of Component to add.</typeparam>
        /// <param name="entity">The ID of the Entity to receive this Component.</param>
        /// <param name="component">The instance of the Component to be added.</param>
        public void AddComponent<T>(int entity, T component) where T : IComponent
            => EnsureComponentCollectionExists<T>().Add(entity, component);

        /// <summary>
        /// Adds an uninitialised version of the given Component type to the Entity of the given ID.
        /// </summary>
        /// <typeparam name="T">The type of Component to add.</typeparam>
        /// <param name="entity">The ID of the Entity to receive this Component.</param>
        public void AddComponent<T>(int entity) where T : IComponent, new()
            => EnsureComponentCollectionExists<T>().Add(entity, new T());

        /// <summary>
        /// Gets the instance of the Component of the given type for the Entity of the given ID.
        /// </summary>
        /// <typeparam name="T">The type of Component to get.</typeparam>
        /// <param name="entity">The ID of the Entity to get this Component from.</param>
        /// <returns>The fetched Component instance.</returns>
        public ref T GetComponent<T>(int entity) where T : IComponent
        {
            ComponentCollection<T> collection = EnsureComponentCollectionExists<T>();

            if (!collection.Contains(entity))
            {
                throw new ComponentNotFoundException<T>(entity);
            }

            return ref collection.Get(entity);
        }

        /// <summary>
        /// Attempts to get the instance of the Component of the given type for the Entity of the given ID.
        /// </summary>
        /// <typeparam name="T">The type of Component to get.</typeparam>
        /// <param name="entity">The ID of the Entity to get this Component from.</param>
        /// <param name="component">The fetched Component instance, if it exists.</param>
        /// <returns>True if the component exists, otherwise false.</returns>
        public bool TryGetComponent<T>(int entity, ref T component) where T : IComponent
        {
            ComponentCollection<T> collection = EnsureComponentCollectionExists<T>();

            if (collection.Contains(entity))
            {
                component = collection.Get(entity);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the Component instance of the given type from the Entity of the given ID.
        /// </summary>
        /// <typeparam name="T">The type of Component to remove.</typeparam>
        /// <param name="entity">The ID of the Entity to remove this Component from.</param>
        public void RemoveComponent<T>(int entity) where T : IComponent
            => EnsureComponentCollectionExists<T>().Remove(entity);

        /// <summary>
        /// Returns a With instance which can enumerate the IDs of all Entities with the given Component type.
        /// </summary>
        /// <typeparam name="T">The type of Component to get Entity IDs by.</typeparam>
        /// <returns>An enumerable containing all the valid Entities.</returns>
        public With<T> With<T>() where T : IComponent
            => new With<T>(this);

        /// <summary>
        /// Returns a With instance which can enumerate the IDs of all Entities with the given Component types.
        /// </summary>
        /// <typeparam name="T">The types of Components to get Entity IDs by.</typeparam>
        /// <returns>An enumerable containing all the valid Entities.</returns>
        public With<T, U> With<T, U>() where T : IComponent where U : IComponent
            => new With<T, U>(this);

        /// <summary>
        /// Gets a ComponentCollection of the given Component type, or creates one if it doesn't exist.
        /// </summary>
        /// <typeparam name="T">The type of Component that the ComponentCollection should contain.</typeparam>
        /// <returns>The fetched ComponentCollection instance if it exists, otherwise the newly created one.</returns>
        internal ComponentCollection<T> EnsureComponentCollectionExists<T>() where T : IComponent
        {
            Type type = typeof(T);

            if (components.TryGetValue(type, out IComponentCollection collection))
            {
                return (ComponentCollection<T>)collection;
            }

            ComponentCollection<T> newCollection = new ComponentCollection<T>(maxEntities);

            components[type] = newCollection;

            return newCollection;
        }
    }
}
