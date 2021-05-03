namespace ECS.Systems.Assets
{
    internal interface IAssetLoader
    {
        string FileExtension { get; }

        internal void LoadAsset(string path);
    }
}
