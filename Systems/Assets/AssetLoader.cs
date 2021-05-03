using ECS.Core.Exceptions;
using System.Collections.Generic;
using System.IO;

namespace ECS.Systems.Assets
{
    public abstract class AssetLoader<T> : IAssetLoader
    {
        internal readonly Dictionary<string, T> assets;

        public abstract string FileExtension { get; }

        public AssetLoader()
        {
            assets = new Dictionary<string, T>();
        }

        void IAssetLoader.LoadAsset(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);

            assets[name] = Load(path);
        }

        public abstract T Load(string path);

        internal T Get(string name)
        {
            if (!assets.ContainsKey(name))
            {
                throw new AssetNotFoundException<T>(name);
            }

            return assets[name];
        }
    }
}
