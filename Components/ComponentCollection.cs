using System.Collections;
using System.Collections.Generic;

namespace ECS.Components
{
    internal class ComponentCollection<T> : IComponentCollection where T : IComponent
    {
        private readonly T[] components;

        private readonly BitArray contains;

        private readonly List<int> entities;

        public IEnumerable<int> Entities => entities;

        public int Count => entities.Count;

        public ComponentCollection(int maxEntities)
        {
            components = new T[maxEntities];

            contains = new BitArray(maxEntities);

            entities = new List<int>();
        }

        public void Add(int entity, T component)
        {
            components[entity] = component;

            contains.Set(entity, true);

            entities.Add(entity);
        }

        public ref T Get(int entity)
            => ref components[entity];

        public bool Contains(int entity)
            => contains[entity];

        public void Remove(int entity)
        {
            contains.Set(entity, false);

            entities.Remove(entity);
        }
    }
}
