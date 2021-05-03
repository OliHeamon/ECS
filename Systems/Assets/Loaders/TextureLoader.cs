using ECS.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace ECS.Systems.Assets.Loaders
{
    public class TextureLoader : AssetLoader<Texture2D>
    {
        public override string FileExtension => "png";

        public override Texture2D Load(string path)
        {
            Stream stream = TitleContainer.OpenStream(path);

            return Texture2D.FromStream(ECSGame.Instance.GraphicsDevice, stream);
        }
    }
}
