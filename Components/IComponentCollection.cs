using System.Collections.Generic;

namespace ECS.Components
{
    internal interface IComponentCollection
    {
        void Remove(int entity);

        bool Contains(int entity);

        IEnumerable<int> Entities { get; }

        int Count { get; }
    }
}
