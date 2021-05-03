using System;

namespace ECS.Core.Exceptions
{
    internal class ComponentNotFoundException<T> : Exception
    {
        public ComponentNotFoundException(int entity) : base($"Entity ID {entity} does not have a component of type {typeof(T)} assigned to it!")
        {
        }
    }
}
