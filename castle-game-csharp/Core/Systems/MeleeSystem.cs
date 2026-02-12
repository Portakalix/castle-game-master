using CastleGame.Core.ECS;
using CastleGame.Core.Components;
using Microsoft.Xna.Framework;

namespace CastleGame.Core.Systems;

public class MeleeSystem : GameSystem
{
    private static readonly Color BLOOD_COLOR = new Color(172, 50, 51);

    public override void Update(float deltaTime)
    {
        var allies = World.Entities.Where(e => World.HasComponent<Ally>(e)).ToList();
        var enemies = World.Entities.Where(e => World.HasComponent<Enemy>(e)).ToList();

        foreach (var ally in allies)
        {
            var allyState = World.GetComponent<UnitStateComponent>(ally);
            var allyPos = World.GetComponent<WorldPosition>(ally);
            var allyBB = World.GetComponent<AABB>(ally);
            var allyMelee = World.GetComponent<Melee>(ally);

            if (allyState == null || allyPos == null || allyBB == null) continue;
            if (allyState.State != UnitState.Melee) continue;

            var allyAABB = allyBB.Offset(allyPos.Position);

            foreach (var enemy in enemies)
            {
                var enemyPos = World.GetComponent<WorldPosition>(enemy);
                var enemyBB = World.GetComponent<AABB>(enemy);
                var enemyHealth = World.GetComponent<Health>(enemy);
                var enemyMelee = World.GetComponent<Melee>(enemy);

                if (enemyPos == null || enemyBB == null) continue;

                var enemyAABB = enemyBB.Offset(enemyPos.Position);

                if (!allyAABB.Intersects(enemyAABB)) continue;

                // Ally attacks enemy
                if (allyMelee != null)
                {
                    allyMelee.Cooldown -= deltaTime;
                    if (allyMelee.Cooldown <= 0 && enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(allyMelee.Damage);
                        allyMelee.Cooldown = allyMelee.HitRate;

                        // Spawn blood particle
                        SpawnBloodParticle(enemyPos.Position);

                        if (enemyHealth.IsDead)
                        {
                            SpawnDeathText(enemyPos.Position);
                            World.DestroyEntity(enemy);
                        }
                    }
                }

                // Enemy attacks ally
                if (enemyMelee != null)
                {
                    var allyHealth = World.GetComponent<Health>(ally);
                    enemyMelee.Cooldown -= deltaTime;
                    if (enemyMelee.Cooldown <= 0 && allyHealth != null)
                    {
                        allyHealth.TakeDamage(enemyMelee.Damage);
                        enemyMelee.Cooldown = enemyMelee.HitRate;

                        // Spawn blood particle
                        SpawnBloodParticle(allyPos.Position);

                        if (allyHealth.IsDead)
                        {
                            SpawnDeathText(allyPos.Position);
                            World.DestroyEntity(ally);
                        }
                    }
                }
            }
        }
    }

    private void SpawnBloodParticle(Vector2 position)
    {
        var particle = World.CreateEntity();
        World.AddComponent(particle, new PixelParticle(BLOOD_COLOR, 10f));
        World.AddComponent(particle, new WorldPosition(position));
        World.AddComponent(particle, new Velocity(-10, -10));
    }

    private void SpawnDeathText(Vector2 position)
    {
        var text = World.CreateEntity();
        World.AddComponent(text, new FloatingText("x", position, 2f));
    }
}
