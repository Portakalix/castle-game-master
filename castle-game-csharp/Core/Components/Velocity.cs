using Microsoft.Xna.Framework;

namespace CastleGame.Core.Components;

public class Velocity : ECS.Component
{
    public Vector2 Value;

    public Velocity(float x, float y)
    {
        Value = new Vector2(x, y);
    }

    public Velocity(Vector2 velocity)
    {
        Value = velocity;
    }
}
