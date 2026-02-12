using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CastleGame.Core.Components;

public class SpriteComponent : ECS.Component
{
    public Color Color;
    public Vector2 Size;

    public SpriteComponent(Color color, Vector2 size)
    {
        Color = color;
        Size = size;
    }
}

public class HealthBar : ECS.Component
{
    public int Width;
    public Vector2 Offset;

    public HealthBar(int width, Vector2 offset)
    {
        Width = width;
        Offset = offset;
    }
}

public class PixelParticle : ECS.Component
{
    public Color Color;
    public float Life;
    public Vector2 Position;

    public PixelParticle(Color color, float life)
    {
        Color = color;
        Life = life;
        Position = Vector2.Zero;
    }
}

public class Line : ECS.Component
{
    public Vector2 P1;
    public Vector2 P2;
    public Color Color;

    public Line(Color color)
    {
        Color = color;
        P1 = Vector2.Zero;
        P2 = Vector2.Zero;
    }
}
