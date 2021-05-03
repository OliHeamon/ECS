using ECS.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace ECS.Utilities
{
    public static partial class Utils
    {
		public static IEnumerable<T> DependencySort<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies)
		{
			List<T> sorted = new List<T>();
			HashSet<T> visited = new HashSet<T>();

			foreach (T item in source)
			{
				DependencySortRecursive(item, visited, sorted, dependencies);
			}

			return sorted;
		}

		private static void DependencySortRecursive<T>(T item, HashSet<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies)
		{
			if (visited.Contains(item))
			{
				if (!sorted.Contains(item))
				{
					throw new RecursiveDependencyException(item.GetType());
				}

				return;
			}

			visited.Add(item);

			IEnumerable<T> dependenciesList = dependencies.Invoke(item);

			if (dependenciesList != null)
			{
				foreach (T dependency in dependenciesList)
				{
					if (dependency != null)
					{
						DependencySortRecursive(dependency, visited, sorted, dependencies);
					}
				}
			}

			sorted.Add(item);
		}
	}
}
