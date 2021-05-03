namespace ECS.Systems
{
    public interface ISystem
    {
        void Load();

        void Update(double deltaTimeMs);

        void Draw();

        void Unload();
    }
}
