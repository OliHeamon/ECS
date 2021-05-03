using System;

namespace ECS.Systems
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DependencyAttribute : Attribute
    {
        public Type DependingType { get; private set; }

        public DependencyAttribute(Type dependingType)
        {
            DependingType = dependingType;
        }
    }
}
