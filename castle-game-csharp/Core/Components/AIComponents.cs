namespace CastleGame.Core.Components;

// Tag components for team identification
public class Ally : ECS.Component { }
public class Enemy : ECS.Component { }

public class Destination : ECS.Component
{
    public float X;

    public Destination(float x)
    {
        X = x;
    }
}

public class Melee : ECS.Component
{
    public float Damage;
    public float HitRate;
    public float Cooldown;

    public Melee(float damage, float hitRate)
    {
        Damage = damage;
        HitRate = hitRate;
        Cooldown = 0;
    }
}
