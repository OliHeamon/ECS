using System;

namespace ECS.Core.Exceptions
{
    internal class EntityLimitExceededException : Exception
    {
        public EntityLimitExceededException(int maxEntities) : base($"Maximum amount of entities exceeded! (Maximum is {maxEntities}).")
        {
        }
    }
}
