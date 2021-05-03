using Microsoft.Xna.Framework;

namespace ECS.Core
{
    public abstract partial class ECSGame : Game
    {
        public static ECSGame Instance { get; private set; }

        public abstract string AssetFolderRelativePath { get; }

        private readonly GraphicsDeviceManager graphics;

        public ECSGame()
        {
            Instance = this;

            graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false
            };

            IsFixedTimeStep = false;
        }

        protected sealed override void Initialize()
        {
            LoadSystems();

            Load();
        }

        protected sealed override void Update(GameTime gameTime)
        {
            UpdateSystems(gameTime.ElapsedGameTime.TotalMilliseconds);

            Update(gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        protected sealed override void Draw(GameTime gameTime)
            => DrawSystems();

        protected sealed override void UnloadContent()
        {
            UnloadSystems();

            Unload();
        }

        protected virtual void Load()
        {
        }

        protected virtual void Unload()
        {
        }

        protected virtual void Update(double deltaTimeMs)
        {
        }
    }
}
