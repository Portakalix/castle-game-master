using CastleGame.Core.ECS;
using CastleGame.Core.Components;
using Microsoft.Xna.Framework;

namespace CastleGame.Core.Systems;

public class UnitFallSystem : GameSystem
{
    private const float GRAVITY = 98.1f;

    public override void Update(float deltaTime)
    {
        var terrain = World.GetResource<Terrain>();
        if (terrain == null) return;

        foreach (var (entity, walk) in World.GetEntitiesWithComponent<Walk>())
        {
            var pos = World.GetComponent<WorldPosition>(entity);
            if (pos == null) continue;

            // Apply gravity
            pos.Position.Y += GRAVITY * deltaTime;

            // Collision detection with terrain
            var hitbox = walk.Bounds.Offset(pos.Position);
            var rect = hitbox.ToRectangle();

            // Move unit up until no collision
            int maxIterations = 100;
            int iteration = 0;
            while (terrain.RectCollides(rect, out _) && iteration < maxIterations)
            {
                pos.Position.Y -= 1;
                hitbox = walk.Bounds.Offset(pos.Position);
                rect = hitbox.ToRectangle();
                iteration++;
            }
        }
    }
}
