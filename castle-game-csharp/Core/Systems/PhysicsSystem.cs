using CastleGame.Core.ECS;
using CastleGame.Core.Components;
using Microsoft.Xna.Framework;

namespace CastleGame.Core.Systems;

public class PhysicsSystem : GameSystem
{
    private const float GRAVITY = 98.1f;

    public override void Update(float deltaTime)
    {
        var terrain = World.GetResource<Terrain>();
        if (terrain == null) return;

        // Apply gravity to all entities with velocity
        foreach (var (entity, velocity) in World.GetEntitiesWithComponent<Velocity>())
        {
            var pos = World.GetComponent<WorldPosition>(entity);
            if (pos == null) continue;

            // Apply gravity
            velocity.Value.Y += GRAVITY * deltaTime;

            // Update position
            pos.Position += velocity.Value * deltaTime;
        }
    }
}
