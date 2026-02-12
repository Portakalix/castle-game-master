using CastleGame.Core.ECS;
using CastleGame.Core.Components;

namespace CastleGame.Core.Systems;

public class FloatingTextSystem : GameSystem
{
    public override void Update(float deltaTime)
    {
        var texts = World.Entities.Where(e => World.HasComponent<FloatingText>(e)).ToList();

        foreach (var entity in texts)
        {
            var text = World.GetComponent<FloatingText>(entity);
            if (text == null) continue;

            text.TimeAlive -= deltaTime;
            if (text.TimeAlive <= 0)
            {
                World.DestroyEntity(entity);
            }
        }
    }
}
