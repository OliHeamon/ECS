using System;

namespace ECS.Core.Exceptions
{
    internal class RecursiveDependencyException : Exception
    {
        public RecursiveDependencyException(Type type) : base($"Recursive dependency found for type {type}!")
        {
        }
    }
}
