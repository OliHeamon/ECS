using System;

namespace ECS.Core.Exceptions
{
    internal class AssetNotFoundException<T> : Exception
    {
        public AssetNotFoundException(string name) : base($"There is no asset of type {typeof(T)} with name {name}!")
        {

        }
    }
}
