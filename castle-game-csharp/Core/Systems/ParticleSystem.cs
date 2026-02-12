using CastleGame.Core.ECS;
using CastleGame.Core.Components;

namespace CastleGame.Core.Systems;

public class ParticleSystem : GameSystem
{
    private const float GRAVITY = 98.1f;

    public override void Update(float deltaTime)
    {
        var terrain = World.GetResource<Terrain>();
        if (terrain == null) return;

        var particles = World.Entities.Where(e => World.HasComponent<PixelParticle>(e)).ToList();

        foreach (var entity in particles)
        {
            var particle = World.GetComponent<PixelParticle>(entity);
            var pos = World.GetComponent<WorldPosition>(entity);
            var vel = World.GetComponent<Velocity>(entity);

            if (particle == null || pos == null || vel == null) continue;

            var oldPos = particle.Position;

            // Update physics
            pos.Position += vel.Value * deltaTime;
            vel.Value.Y += GRAVITY * deltaTime;
            particle.Life -= deltaTime;

            particle.Position = pos.Position;

            // Check terrain collision
            if (terrain.LineCollides(oldPos.ToPoint(), particle.Position.ToPoint(), out var hitPoint))
            {
                terrain.DrawPixel(hitPoint.X, hitPoint.Y, particle.Color.PackedValue);
                World.DestroyEntity(entity);
            }
            else if (particle.Life <= 0)
            {
                World.DestroyEntity(entity);
            }
        }
    }
}
