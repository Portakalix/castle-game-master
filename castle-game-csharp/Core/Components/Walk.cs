namespace CastleGame.Core.Components;

public class Walk : ECS.Component
{
    public AABB Bounds;
    public float Speed;

    public Walk(AABB bounds, float speed)
    {
        Bounds = bounds;
        Speed = speed;
    }
}
