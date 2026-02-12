using Microsoft.Xna.Framework;

namespace CastleGame.Core.Components;

public class Turret : ECS.Component
{
    public float Delay;
    public float MinDistance;
    public float MaxStrength;
    public float FlightTime;
    public float StrengthVariation;
    public float DelayLeft;

    public Turret(float delay, float minDistance, float maxStrength, float flightTime, float strengthVariation)
    {
        Delay = delay;
        MinDistance = minDistance;
        MaxStrength = maxStrength;
        FlightTime = flightTime;
        StrengthVariation = strengthVariation;
        DelayLeft = 0;
    }
}

public class TurretOffset : ECS.Component
{
    public Vector2 Offset;

    public TurretOffset(float x, float y)
    {
        Offset = new Vector2(x, y);
    }
}

public class Projectile : ECS.Component { }

public class ProjectileDamage : ECS.Component
{
    public float Value;

    public ProjectileDamage(float damage)
    {
        Value = damage;
    }
}

public class Arrow : ECS.Component
{
    public float Length;

    public Arrow(float length)
    {
        Length = length;
    }
}

public enum IgnoreCollisionType
{
    Ally,
    Enemy
}

public class IgnoreCollision : ECS.Component
{
    public IgnoreCollisionType Type;

    public IgnoreCollision(IgnoreCollisionType type)
    {
        Type = type;
    }
}
