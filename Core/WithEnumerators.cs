using ECS.Components;
using System.Collections;
using System.Collections.Generic;

namespace ECS.Core
{
    internal struct WithEnumerator<T, U> : IEnumerator<int> where T : IComponent where U : IComponent
    {
        private readonly IComponentCollection collection;

        private readonly IEnumerator<int> setEnumerator;

        public WithEnumerator(World world)
        {
            IComponentCollection collection1 = world.EnsureComponentCollectionExists<T>();
            IComponentCollection collection2 = world.EnsureComponentCollectionExists<U>();

            if (collection1.Count > collection2.Count)
            {
                setEnumerator = collection2.Entities.GetEnumerator();

                collection = collection1;
            }
            else
            {
                setEnumerator = collection1.Entities.GetEnumerator();

                collection = collection2;
            }
        }

        public int Current => setEnumerator.Current;

        object IEnumerator.Current => setEnumerator.Current;

        public bool MoveNext()
        {
            while (setEnumerator.MoveNext())
            {
                int entity = setEnumerator.Current;

                if (!collection.Contains(entity))
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        public void Reset() => setEnumerator.Reset();

        public void Dispose()
        {
        }
    }
}
