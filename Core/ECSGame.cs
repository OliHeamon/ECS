using Microsoft.Xna.Framework;

namespace ECS.Core
{
    public abstract partial class ECSGame : Game
    {
        private readonly GraphicsDeviceManager graphics;

        public ECSGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false
            };

            IsFixedTimeStep = false;
        }

        protected sealed override void Initialize()
        {
            InitialiseSystems();

            Load();
        }

        protected sealed override void Update(GameTime gameTime)
            => UpdateSystems(gameTime.ElapsedGameTime.TotalMilliseconds);

        protected sealed override void Draw(GameTime gameTime)
            => DrawSystems();

        protected sealed override void UnloadContent()
            => Unload();

        protected abstract void Load();

        protected abstract void Unload();
    }
}
