using Microsoft.Xna.Framework;

namespace CastleGame.Core.Components;

public class WorldPosition : ECS.Component
{
    public Vector2 Position;

    public WorldPosition(float x, float y)
    {
        Position = new Vector2(x, y);
    }

    public WorldPosition(Vector2 position)
    {
        Position = position;
    }
}
