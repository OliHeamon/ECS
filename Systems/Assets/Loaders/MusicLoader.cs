using ECS.Systems.Assets.CustomAssetTypes;

namespace ECS.Systems.Assets.Loaders
{
    public class MusicLoader : AssetLoader<Music>
    {
        public override string FileExtension => "ogg";

        public override Music Load(string path)
            => new Music(path);
    }
}
