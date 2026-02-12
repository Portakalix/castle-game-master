using CastleGame.Core.ECS;
using CastleGame.Core.Components;
using Microsoft.Xna.Framework;

namespace CastleGame.Core.Systems;

public class TurretSystem : GameSystem
{
    private const float GRAVITY = 98.1f;
    private readonly Random _random = new Random();

    public override void Update(float deltaTime)
    {
        foreach (var (entity, turret) in World.GetEntitiesWithComponent<Turret>())
        {
            var pos = World.GetComponent<WorldPosition>(entity);
            var damage = World.GetComponent<ProjectileDamage>(entity);
            var bb = World.GetComponent<AABB>(entity);

            if (pos == null || damage == null || bb == null) continue;

            // Update turret delay
            turret.DelayLeft -= deltaTime;
            if (turret.DelayLeft > 0) continue;

            // Find target
            Vector2? targetPos = FindNearestTarget(entity, pos.Position, turret);
            if (!targetPos.HasValue) continue;

            // Calculate ballistic trajectory
            float variation = 0;
            if (turret.StrengthVariation > 0)
            {
                float sign = targetPos.Value.X > pos.Position.X ? 1 : -1;
                variation = (float)(_random.NextDouble() * turret.StrengthVariation * sign);
            }

            float time = turret.FlightTime;
            float dist = Vector2.Distance(pos.Position, targetPos.Value);
            float vx = (targetPos.Value.X - pos.Position.X + variation * dist) / time;
            float vy = (targetPos.Value.Y + 0.5f * -GRAVITY * time * time - pos.Position.Y) / time;

            float speed = MathF.Sqrt(vx * vx + vy * vy);
            if (speed < turret.MaxStrength)
            {
                // Fire projectile!
                var projectile = World.CreateEntity();
                World.AddComponent(projectile, new Projectile());
                World.AddComponent(projectile, new WorldPosition(pos.Position));
                World.AddComponent(projectile, new Velocity(vx, vy));
                World.AddComponent(projectile, bb);
                World.AddComponent(projectile, damage);

                // Add arrow component if turret has one
                var arrow = World.GetComponent<Arrow>(entity);
                if (arrow != null)
                {
                    World.AddComponent(projectile, arrow);
                    var line = World.GetComponent<Line>(entity);
                    if (line != null)
                    {
                        World.AddComponent(projectile, new Line(line.Color));
                    }
                }

                // Add ignore collision
                var ignore = World.GetComponent<IgnoreCollision>(entity);
                if (ignore != null)
                {
                    World.AddComponent(projectile, ignore);
                }

                turret.DelayLeft = turret.Delay;
            }
        }
    }

    private Vector2? FindNearestTarget(Entity turret, Vector2 turretPos, Turret turretComp)
    {
        bool isAllyTurret = World.HasComponent<Ally>(turret);
        Vector2? closest = null;
        float minDist = float.MaxValue;

        var targets = isAllyTurret 
            ? World.Entities.Where(e => World.HasComponent<Enemy>(e))
            : World.Entities.Where(e => World.HasComponent<Ally>(e));

        foreach (var target in targets)
        {
            var targetPos = World.GetComponent<WorldPosition>(target);
            var targetBB = World.GetComponent<AABB>(target);
            var targetState = World.GetComponent<UnitStateComponent>(target);
            var targetWalk = World.GetComponent<Walk>(target);

            if (targetPos == null || targetBB == null) continue;

            var pos = targetPos.Position;
            pos.X += targetBB.Width / 2;
            pos.Y += targetBB.Height / 2;

            // Predict future position if walking
            if (targetState != null && targetState.State == UnitState.Walk && targetWalk != null)
            {
                var dest = World.GetComponent<Destination>(target);
                if (dest != null)
                {
                    float direction = Math.Sign(dest.X - pos.X);
                    pos.X += targetWalk.Speed * turretComp.FlightTime * direction;
                }
            }

            float dist = Vector2.Distance(turretPos, pos);
            if (dist < minDist && dist > turretComp.MinDistance)
            {
                minDist = dist;
                closest = pos;
            }
        }

        return closest;
    }
}
