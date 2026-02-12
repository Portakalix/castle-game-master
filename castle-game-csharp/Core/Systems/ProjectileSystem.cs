using CastleGame.Core.ECS;
using CastleGame.Core.Components;
using Microsoft.Xna.Framework;

namespace CastleGame.Core.Systems;

public class ProjectileSystem : GameSystem
{
    private const float GRAVITY = 98.1f;

    public override void Update(float deltaTime)
    {
        var terrain = World.GetResource<Terrain>();
        if (terrain == null) return;

        var projectiles = World.Entities.Where(e => World.HasComponent<Projectile>(e)).ToList();

        foreach (var proj in projectiles)
        {
            var pos = World.GetComponent<WorldPosition>(proj);
            var vel = World.GetComponent<Velocity>(proj);

            if (pos == null || vel == null) continue;

            // Apply gravity
            vel.Value.Y += GRAVITY * deltaTime;

            // Calculate next position
            var nextPos = pos.Position + vel.Value * deltaTime;

            // Check terrain collision
            if (terrain.LineCollides(pos.Position.ToPoint(), nextPos.ToPoint(), out var hitPoint))
            {
                HandleTerrainHit(proj, hitPoint);
                continue;
            }

            // Check unit collision
            if (CheckUnitCollision(proj, pos))
            {
                continue;
            }

            // Update position
            pos.Position = nextPos;
        }
    }

    private void HandleTerrainHit(Entity projectile, Point hitPoint)
    {
        // Create crater if this projectile has a mask
        var arrow = World.GetComponent<Arrow>(projectile);
        if (arrow != null)
        {
            // Stick arrow in terrain
            var line = World.GetComponent<Line>(projectile);
            var pos = World.GetComponent<WorldPosition>(projectile);
            if (line != null && pos != null)
            {
                // Create stuck arrow entity
                var stuckArrow = World.CreateEntity();
                var newLine = new Line(line.Color);
                newLine.P1 = hitPoint.ToVector2();
                
                var vel = World.GetComponent<Velocity>(projectile);
                if (vel != null)
                {
                    var angle = MathF.Atan2(vel.Value.Y, vel.Value.X);
                    newLine.P2 = newLine.P1 - new Vector2(MathF.Cos(angle) * arrow.Length, MathF.Sin(angle) * arrow.Length);
                }
                
                World.AddComponent(stuckArrow, newLine);
            }
        }
        else
        {
            // Create crater for heavy projectiles
            var terrain = World.GetResource<Terrain>();
            terrain?.DrawCircleMask(hitPoint, 5);
        }

        World.DestroyEntity(projectile);
    }

    private bool CheckUnitCollision(Entity projectile, WorldPosition projPos)
    {
        var projBB = World.GetComponent<AABB>(projectile);
        var projDmg = World.GetComponent<ProjectileDamage>(projectile);
        var ignore = World.GetComponent<IgnoreCollision>(projectile);

        if (projBB == null || projDmg == null) return false;

        var projAABB = projBB.Offset(projPos.Position);

        foreach (var (target, targetHealth) in World.GetEntitiesWithComponent<Health>())
        {
            // Check ignore collision
            if (ignore != null)
            {
                if (ignore.Type == IgnoreCollisionType.Ally && World.HasComponent<Ally>(target)) continue;
                if (ignore.Type == IgnoreCollisionType.Enemy && World.HasComponent<Enemy>(target)) continue;
            }

            var targetPos = World.GetComponent<WorldPosition>(target);
            var targetBB = World.GetComponent<AABB>(target);

            if (targetPos == null || targetBB == null) continue;

            var targetAABB = targetBB.Offset(targetPos.Position);

            if (projAABB.Intersects(targetAABB))
            {
                // Hit!
                targetHealth.TakeDamage(projDmg.Value);

                // Spawn blood particles
                for (int i = 0; i < 4; i++)
                {
                    var particle = World.CreateEntity();
                    var random = new Random();
                    World.AddComponent(particle, new PixelParticle(new Color(172, 50, 51), 10f));
                    World.AddComponent(particle, new WorldPosition(targetPos.Position));
                    World.AddComponent(particle, new Velocity(
                        (float)(random.NextDouble() * 40 - 20),
                        (float)(random.NextDouble() * 40 - 20)
                    ));
                }

                if (targetHealth.IsDead)
                {
                    var text = World.CreateEntity();
                    World.AddComponent(text, new FloatingText("x", targetPos.Position, 2f));
                    World.DestroyEntity(target);
                }

                World.DestroyEntity(projectile);
                return true;
            }
        }

        return false;
    }
}
