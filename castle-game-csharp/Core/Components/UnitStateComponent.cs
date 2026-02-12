namespace CastleGame.Core.Components;

public enum UnitState
{
    Walk,
    Climb,
    Wait,
    Melee,
    Shoot
}

public class UnitStateComponent : ECS.Component
{
    public UnitState State = UnitState.Walk;
}
