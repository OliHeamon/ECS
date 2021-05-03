using System;

namespace ECS.Systems.Assets
{
    internal interface IAssetLoader : IDisposable
    {
        string FileExtension { get; }

        Type AssetType { get; }

        void LoadAsset(string path);
    }
}
