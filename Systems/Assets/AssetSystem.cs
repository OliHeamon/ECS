using ECS.Core;
using ECS.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;

namespace ECS.Systems.Assets
{
    public class AssetSystem : ECSSystem
    {
        private readonly Dictionary<Type, IAssetLoader> loaders;

        public AssetSystem()
        {
            loaders = new Dictionary<Type, IAssetLoader>();
        }

        public override void OnTypeInspected(Type type)
        {
            if (!type.IsAbstract && !type.IsInterface && typeof(IAssetLoader).IsAssignableFrom(type))
            {
                IAssetLoader newLoader = Activator.CreateInstance(type) as IAssetLoader;

                loaders[type] = newLoader;
            }
        }

        public override void Load()
        {
            WalkDirectory(ECSGame.Instance.AssetFolderRelativePath, path => 
            {
                string fileExtension = Path.GetExtension(path);

                foreach (IAssetLoader loader in loaders.Values)
                {
                    if ($".{loader.FileExtension}" == fileExtension)
                    {
                        loader.LoadAsset(path);
                    }
                }
            });
        }

        private void WalkDirectory(string source, Action<string> onFileFound)
        {
            foreach (string path in Directory.GetFiles(source))
            {
                onFileFound.Invoke(path);
            }

            foreach (string subDirectory in Directory.GetDirectories(source))
            {
                WalkDirectory(subDirectory, onFileFound);
            }
        }

        public T Get<T>(string name)
        {
            Type assetType = typeof(T);

            if (!loaders.ContainsKey(assetType))
            {
                throw new AssetLoaderNotFoundException<T>();
            }

            AssetLoader<T> loader = (AssetLoader<T>)loaders[assetType];

            return loader.Get(name);
        }
    }
}
