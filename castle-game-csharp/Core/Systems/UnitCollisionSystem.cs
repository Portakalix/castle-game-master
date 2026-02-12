using CastleGame.Core.ECS;
using CastleGame.Core.Components;
using Microsoft.Xna.Framework;

namespace CastleGame.Core.Systems;

public class UnitCollisionSystem : GameSystem
{
    public override void Update(float deltaTime)
    {
        var entities = World.Entities.ToList();

        for (int i = 0; i < entities.Count; i++)
        {
            var e1 = entities[i];
            var state1 = World.GetComponent<UnitStateComponent>(e1);
            var pos1 = World.GetComponent<WorldPosition>(e1);
            var bb1 = World.GetComponent<AABB>(e1);
            var dest1 = World.GetComponent<Destination>(e1);

            if (state1 == null || pos1 == null || bb1 == null || dest1 == null) continue;
            if (state1.State != UnitState.Walk) continue;

            var aabb1 = bb1.Offset(pos1.Position);

            for (int j = 0; j < entities.Count; j++)
            {
                if (i == j) continue;

                var e2 = entities[j];
                var state2 = World.GetComponent<UnitStateComponent>(e2);
                var pos2 = World.GetComponent<WorldPosition>(e2);
                var bb2 = World.GetComponent<AABB>(e2);
                var dest2 = World.GetComponent<Destination>(e2);

                if (pos2 == null || bb2 == null || dest2 == null) continue;

                // Get potentially half bounding box for melee units
                var aabb2 = state2 != null && state2.State == UnitState.Melee
                    ? bb2.ToHalfWidth().Offset(pos2.Position)
                    : bb2.Offset(pos2.Position);

                if (!aabb1.Intersects(aabb2)) continue;

                // Check if same team
                bool isAlly1 = World.HasComponent<Ally>(e1);
                bool isAlly2 = World.HasComponent<Ally>(e2);

                if (isAlly1 == isAlly2)
                {
                    // Same team - make one wait
                    float dist1 = Math.Abs(dest1.X - pos1.Position.X);
                    float dist2 = Math.Abs(dest2.X - pos2.Position.X);

                    if (dist1 > dist2)
                    {
                        state1.State = UnitState.Wait;
                    }
                    else if (state2 != null)
                    {
                        state2.State = UnitState.Wait;
                    }
                }
                else
                {
                    // Different teams - fight!
                    state1.State = UnitState.Melee;
                    if (state2 != null)
                    {
                        state2.State = UnitState.Melee;
                    }
                }
                break;
            }
        }
    }
}
