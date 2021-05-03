using System;

namespace ECS.Core.Exceptions
{
    internal class AssetLoaderNotFoundException<T> : Exception
    {
        public AssetLoaderNotFoundException() : base($"There is no asset loader for the type {typeof(T)}!")
        {

        }
    }
}
