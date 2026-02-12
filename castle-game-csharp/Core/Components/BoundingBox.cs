using Microsoft.Xna.Framework;

namespace CastleGame.Core.Components;

public class AABB : ECS.Component
{
    public Vector2 Min;
    public Vector2 Max;

    public AABB(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
    }

    public AABB(float minX, float minY, float maxX, float maxY)
    {
        Min = new Vector2(minX, minY);
        Max = new Vector2(maxX, maxY);
    }

    public float Width => Max.X - Min.X;
    public float Height => Max.Y - Min.Y;

    public AABB Offset(Vector2 position)
    {
        return new AABB(Min + position, Max + position);
    }

    public bool Intersects(AABB other)
    {
        return !(Max.X < other.Min.X || Min.X > other.Max.X ||
                 Max.Y < other.Min.Y || Min.Y > other.Max.Y);
    }

    public AABB ToHalfWidth()
    {
        var halfWidth = Width / 2;
        return new AABB(Min.X + halfWidth, Min.Y, Max.X, Max.Y);
    }

    public Rectangle ToRectangle()
    {
        return new Rectangle((int)Min.X, (int)Min.Y, (int)Width, (int)Height);
    }
}
