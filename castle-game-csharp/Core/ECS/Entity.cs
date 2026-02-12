namespace CastleGame.Core.ECS;

/// <summary>
/// Represents a unique entity in the game world
/// </summary>
public readonly struct Entity : IEquatable<Entity>
{
    public readonly int Id;
    private static int _nextId = 0;

    private Entity(int id)
    {
        Id = id;
    }

    public static Entity Create()
    {
        return new Entity(_nextId++);
    }

    public bool Equals(Entity other) => Id == other.Id;
    public override bool Equals(object? obj) => obj is Entity other && Equals(other);
    public override int GetHashCode() => Id.GetHashCode();
    public static bool operator ==(Entity left, Entity right) => left.Equals(right);
    public static bool operator !=(Entity left, Entity right) => !left.Equals(right);
}
