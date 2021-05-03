using System;

namespace ECS.Systems
{
    public abstract class ECSSystem
    {
        public virtual void OnTypeInspected(Type type)
        {
        }

        public virtual void Load()
        {
        }

        public virtual void Update(double deltaTimeMs)
        {
        }

        public virtual void Draw()
        {
        }

        public virtual void Unload()
        {
        }
    }
}
