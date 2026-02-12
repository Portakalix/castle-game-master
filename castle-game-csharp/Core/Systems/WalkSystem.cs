using CastleGame.Core.ECS;
using CastleGame.Core.Components;
using Microsoft.Xna.Framework;

namespace CastleGame.Core.Systems;

public class WalkSystem : GameSystem
{
    public override void Update(float deltaTime)
    {
        var terrain = World.GetResource<Terrain>();
        if (terrain == null) return;

        foreach (var (entity, walk) in World.GetEntitiesWithComponent<Walk>())
        {
            var state = World.GetComponent<UnitStateComponent>(entity);
            var pos = World.GetComponent<WorldPosition>(entity);
            var dest = World.GetComponent<Destination>(entity);

            if (state == null || pos == null || dest == null) continue;

            // Only walk if in Walk state
            if (state.State != UnitState.Walk) continue;

            // Check terrain collision in front
            var hitbox = walk.Bounds.Offset(pos.Position);
            var rect = hitbox.ToRectangle();
            
            if (terrain.RectCollides(rect, out var hit))
            {
                // If hitting top edge, try to climb
                if (hit.Y == rect.Top)
                {
                    state.State = UnitState.Climb;
                    continue;
                }
            }

            // Move towards destination
            float direction = Math.Sign(dest.X - pos.Position.X);
            pos.Position.X += walk.Speed * deltaTime * direction;
        }
    }
}
