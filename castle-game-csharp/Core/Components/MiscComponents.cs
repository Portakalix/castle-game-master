using Microsoft.Xna.Framework;

namespace CastleGame.Core.Components;

public class TerrainMask : ECS.Component
{
    public int MaskId;
    public Point Position;
    public Point Size;

    public TerrainMask(int maskId, Point position, Point size)
    {
        MaskId = maskId;
        Position = position;
        Size = size;
    }
}

public class FloatingText : ECS.Component
{
    public string Text;
    public Vector2 Position;
    public float TimeAlive;

    public FloatingText(string text, Vector2 position, float timeAlive)
    {
        Text = text;
        Position = position;
        TimeAlive = timeAlive;
    }
}
