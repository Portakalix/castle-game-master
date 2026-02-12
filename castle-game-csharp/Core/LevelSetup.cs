using CastleGame.Core.ECS;
using CastleGame.Core.Components;
using Microsoft.Xna.Framework;

namespace CastleGame.Core;

public class LevelSetup
{
    public static void PlaceTurrets(World world, int level)
    {
        if (level == 1)
        {
            // Enemy turret (cannon)
            var turret1 = world.CreateEntity();
            world.AddComponent(turret1, new Enemy());
            world.AddComponent(turret1, new WorldPosition(1270, 295));
            world.AddComponent(turret1, new Turret(3f, 50f, 310f, 5f, 0.05f));
            world.AddComponent(turret1, new AABB(0, 0, 5, 5));
            world.AddComponent(turret1, new ProjectileDamage(30f));

            // Enemy turret (archer)
            var turret2 = world.CreateEntity();
            world.AddComponent(turret2, new Enemy());
            world.AddComponent(turret2, new WorldPosition(1255, 315));
            world.AddComponent(turret2, new Turret(1f, 50f, 290f, 4f, 0.05f));
            world.AddComponent(turret2, new AABB(0, 0, 1, 1));
            world.AddComponent(turret2, new ProjectileDamage(10f));
            world.AddComponent(turret2, new Arrow(7f));
            world.AddComponent(turret2, new Line(new Color(102, 57, 49)));

            // Enemy soldiers
            for (int i = 0; i < 5; i++)
            {
                float health = 50f;
                var soldier = world.CreateEntity();
                world.AddComponent(soldier, new Enemy());
                world.AddComponent(soldier, new SpriteComponent(new Color(200, 50, 50), new Vector2(5, 10)));
                world.AddComponent(soldier, new WorldPosition(1130 - 20 * i, 320));
                world.AddComponent(soldier, new Walk(new AABB(2, 5, 5, 10), 15f));
                world.AddComponent(soldier, new AABB(1, 0, 6, 10));
                world.AddComponent(soldier, new Destination(10f));
                world.AddComponent(soldier, new Health(health));
                world.AddComponent(soldier, new HealthBar(10, new Vector2(-2, -3)));
                world.AddComponent(soldier, new Melee(10f, 1f));
                world.AddComponent(soldier, new UnitStateComponent());
            }

            // Enemy archers
            for (int i = 0; i < 20; i++)
            {
                float health = 20f;
                var archer = world.CreateEntity();
                world.AddComponent(archer, new Enemy());
                world.AddComponent(archer, new SpriteComponent(new Color(150, 100, 100), new Vector2(5, 10)));
                world.AddComponent(archer, new WorldPosition(1140 - 20 * i, 320));
                world.AddComponent(archer, new Walk(new AABB(1, 5, 4, 10), 20f));
                world.AddComponent(archer, new AABB(1, 0, 5, 10));
                world.AddComponent(archer, new Destination(10f));
                world.AddComponent(archer, new Health(health));
                world.AddComponent(archer, new HealthBar(5, new Vector2(1, -3)));
                world.AddComponent(archer, new Melee(5f, 1f));
                world.AddComponent(archer, new Turret(3f, 20f, 150f, 2f, 0.1f));
                world.AddComponent(archer, new TurretOffset(2, 2));
                world.AddComponent(archer, new ProjectileDamage(5f));
                world.AddComponent(archer, new Arrow(3f));
                world.AddComponent(archer, new Line(new Color(102, 57, 49)));
                world.AddComponent(archer, new IgnoreCollision(IgnoreCollisionType.Enemy));
                world.AddComponent(archer, new UnitStateComponent());
            }
        }
    }

    public static void BuyArcher(World world)
    {
        float health = 20f;
        var archer = world.CreateEntity();
        world.AddComponent(archer, new Ally());
        world.AddComponent(archer, new SpriteComponent(new Color(50, 150, 200), new Vector2(5, 10)));
        world.AddComponent(archer, new WorldPosition(1, 340));
        world.AddComponent(archer, new Walk(new AABB(1, 5, 4, 10), 20f));
        world.AddComponent(archer, new AABB(0, 0, 5, 10));
        world.AddComponent(archer, new Destination(1280f));
        world.AddComponent(archer, new Health(health));
        world.AddComponent(archer, new HealthBar(5, new Vector2(1, -3)));
        world.AddComponent(archer, new Melee(5f, 1f));
        world.AddComponent(archer, new Turret(3f, 20f, 150f, 2f, 0.1f));
        world.AddComponent(archer, new TurretOffset(2, 2));
        world.AddComponent(archer, new ProjectileDamage(5f));
        world.AddComponent(archer, new Arrow(3f));
        world.AddComponent(archer, new Line(new Color(102, 57, 49)));
        world.AddComponent(archer, new IgnoreCollision(IgnoreCollisionType.Ally));
        world.AddComponent(archer, new UnitStateComponent());
    }

    public static void BuySoldier(World world)
    {
        float health = 50f;
        var soldier = world.CreateEntity();
        world.AddComponent(soldier, new Ally());
        world.AddComponent(soldier, new SpriteComponent(new Color(50, 100, 200), new Vector2(5, 10)));
        world.AddComponent(soldier, new WorldPosition(1, 340));
        world.AddComponent(soldier, new Walk(new AABB(1, 5, 4, 10), 15f));
        world.AddComponent(soldier, new AABB(0, 0, 5, 10));
        world.AddComponent(soldier, new Destination(1280f));
        world.AddComponent(soldier, new Health(health));
        world.AddComponent(soldier, new HealthBar(10, new Vector2(-2, -3)));
        world.AddComponent(soldier, new Melee(10f, 1f));
        world.AddComponent(soldier, new UnitStateComponent());
    }
}
