using ECS.Systems;
using ECS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ECS.Core
{
    public partial class ECSGame
    {
        private List<ISystem> systems;

        private Dictionary<Type, ISystem> systemsByType;

        private void InitialiseSystems()
        {
            systems = new List<ISystem>();

            systemsByType = new Dictionary<Type, ISystem>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsInterface && typeof(ISystem).IsAssignableFrom(type))
                    {
                        ISystem newSystem = Activator.CreateInstance(type) as ISystem;

                        systems.Add(newSystem);
                        systemsByType.Add(type, newSystem);
                    }
                }
            }

            IEnumerable<ISystem> GetDependencies(ISystem system)
                => system.GetType()
                .GetCustomAttributes<DependencyAttribute>()
                .Select(attribute => systemsByType[attribute.DependingType]);

            systems = systems.DependencySort(GetDependencies).ToList();
        }

        private void UpdateSystems(double deltaTimeMs)
        {
            foreach (ISystem system in systems)
            {
                system.Update(deltaTimeMs);
            }
        }

        private void DrawSystems()
        {
            foreach (ISystem system in systems)
            {
                system.Draw();
            }
        }
    }
}
