using ECS.Components;
using System.Collections;
using System.Collections.Generic;

namespace ECS.Core
{
    public struct With<T> : IEnumerable<int> where T : IComponent
    {
        private readonly World world;

        public With(World world) => this.world = world;

        public IEnumerator<int> GetEnumerator() => world.EnsureComponentCollectionExists<T>().Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public struct With<T, U> : IEnumerable<int> where T : IComponent where U : IComponent
    {
        private readonly World world;

        public With(World world) => this.world = world;

        public IEnumerator<int> GetEnumerator() => new WithEnumerator<T, U>(world);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
