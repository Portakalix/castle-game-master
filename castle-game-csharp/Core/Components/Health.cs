namespace CastleGame.Core.Components;

public class Health : ECS.Component
{
    public float Value;
    public float MaxHealth;

    public Health(float maxHealth)
    {
        MaxHealth = maxHealth;
        Value = maxHealth;
    }

    public bool IsDead => Value <= 0;

    public void TakeDamage(float damage)
    {
        Value = Math.Max(0, Value - damage);
    }
}
