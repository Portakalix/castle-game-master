namespace CastleGame.Core.ECS;

/// <summary>
/// Base class for all systems in the ECS architecture
/// </summary>
public abstract class GameSystem
{
    protected World World { get; private set; } = null!;

    public void Initialize(World world)
    {
        World = world;
        OnInitialize();
    }

    protected virtual void OnInitialize() { }

    public abstract void Update(float deltaTime);
}
