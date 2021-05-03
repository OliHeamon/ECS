using ECS.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace ECS.Systems.Assets.Loaders
{
    public class EffectLoader : AssetLoader<Effect>
    {
        public override string FileExtension => "fxc";

        public override Effect Load(string path)
        {
            Stream stream = TitleContainer.OpenStream(path);

            byte[] effectCode = new byte[stream.Length];

            stream.Read(effectCode, 0, (int)stream.Length);

            return new Effect(ECSGame.Instance.GraphicsDevice, effectCode);
        }
    }
}
