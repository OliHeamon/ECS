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
        private List<ECSSystem> systems;

        private Dictionary<Type, ECSSystem> systemsByType;

        private void LoadSystems()
        {
            systems = new List<ECSSystem>();

            systemsByType = new Dictionary<Type, ECSSystem>();

            List<Type> allTypes = new List<Type>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    allTypes.Add(type);

                    if (!type.IsAbstract && type.IsSubclassOf(typeof(ECSSystem)))
                    {
                        ECSSystem newSystem = Activator.CreateInstance(type) as ECSSystem;

                        systems.Add(newSystem);

                        systemsByType[type] = newSystem;
                    }
                }
            }

            IEnumerable<ECSSystem> GetDependencies(ECSSystem system)
                => system.GetType()
                .GetCustomAttributes<DependencyAttribute>()
                .Select(attribute => systemsByType[attribute.DependingType]);

            systems = systems.DependencySort(GetDependencies).ToList();

            foreach (ECSSystem system in systems)
            {
                foreach (Type type in allTypes)
                {
                    system.OnTypeInspected(type);
                }

                system.Load();
            }
        }

        private void UpdateSystems(double deltaTimeMs)
        {
            foreach (ECSSystem system in systems)
            {
                system.Update(deltaTimeMs);
            }
        }

        private void DrawSystems()
        {
            foreach (ECSSystem system in systems)
            {
                system.Draw();
            }
        }

        private void UnloadSystems()
        {
            foreach (ECSSystem system in systems)
            {
                system.Unload();
            }
        }

        public ECSSystem GetSystem<T>() where T : ECSSystem
            => systemsByType[typeof(T)];
    }
}
