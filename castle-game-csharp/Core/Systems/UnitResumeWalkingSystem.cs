using CastleGame.Core.ECS;
using CastleGame.Core.Components;
using Microsoft.Xna.Framework;

namespace CastleGame.Core.Systems;

public class UnitResumeWalkingSystem : GameSystem
{
    public override void Update(float deltaTime)
    {
        var entities = World.Entities.ToList();

        foreach (var e1 in entities)
        {
            var state1 = World.GetComponent<UnitStateComponent>(e1);
            var pos1 = World.GetComponent<WorldPosition>(e1);
            var bb1 = World.GetComponent<AABB>(e1);
            var dest1 = World.GetComponent<Destination>(e1);

            if (state1 == null || pos1 == null || bb1 == null || dest1 == null) continue;

            // Only check waiting or fighting units
            if (state1.State != UnitState.Wait && state1.State != UnitState.Melee) continue;

            var aabb1 = bb1.Offset(pos1.Position);
            bool intersects = false;

            foreach (var e2 in entities)
            {
                if (e1 == e2) continue;

                var pos2 = World.GetComponent<WorldPosition>(e2);
                var bb2 = World.GetComponent<AABB>(e2);
                var dest2 = World.GetComponent<Destination>(e2);

                if (pos2 == null || bb2 == null || dest2 == null) continue;

                var aabb2 = bb2.Offset(pos2.Position);

                if (aabb1.Intersects(aabb2))
                {
                    float dist1 = Math.Abs(dest1.X - pos1.Position.X);
                    float dist2 = Math.Abs(dest2.X - pos2.Position.X);

                    // If this unit is farther from destination, stay waiting
                    if (dist1 > dist2)
                    {
                        intersects = true;
                        break;
                    }
                }
            }

            // No longer colliding, resume walking
            if (!intersects)
            {
                state1.State = UnitState.Walk;
            }
        }
    }
}
