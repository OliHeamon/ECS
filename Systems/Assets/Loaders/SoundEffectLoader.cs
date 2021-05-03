using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace ECS.Systems.Assets.Loaders
{
    public class SoundEffectLoader : AssetLoader<SoundEffect>
    {
        public override string FileExtension => "wav";

        public override SoundEffect Load(string path)
        {
            Stream stream = TitleContainer.OpenStream(path);

            return SoundEffect.FromStream(stream);
        }
    }
}
