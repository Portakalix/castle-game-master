using CastleGame.Core.ECS;
using CastleGame.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CastleGame.Core.Systems;

public class RenderingSystem : GameSystem
{
    private GraphicsDevice? _graphics;
    private SpriteBatch? _spriteBatch;
    private Texture2D? _pixelTexture;
    private Texture2D? _terrainTexture;
    private SpriteFont? _font;

    private static readonly Color GREEN_BAR = new Color(106, 190, 48);
    private static readonly Color RED_BAR = new Color(172, 50, 51);

    public void Initialize(GraphicsDevice graphics)
    {
        _graphics = graphics;
        _spriteBatch = new SpriteBatch(graphics);
        _pixelTexture = new Texture2D(graphics, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });

        var terrain = World.GetResource<Terrain>();
        if (terrain != null)
        {
            _terrainTexture = new Texture2D(graphics, terrain.Width, terrain.Height);
        }
    }

    public void SetFont(SpriteFont font)
    {
        _font = font;
    }

    public void Draw()
    {
        if (_graphics == null || _spriteBatch == null || _pixelTexture == null) return;

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

        // Draw terrain
        DrawTerrain();

        // Draw sprites
        DrawSprites();

        // Draw lines (arrows)
        DrawLines();

        // Draw particles
        DrawParticles();

        // Draw health bars
        DrawHealthBars();

        // Draw floating text
        DrawFloatingText();

        _spriteBatch.End();
    }

    private void DrawTerrain()
    {
        var terrain = World.GetResource<Terrain>();
        if (terrain == null || _terrainTexture == null) return;

        // Convert terrain buffer to Color array
        var colors = new Color[terrain.Buffer.Length];
        for (int i = 0; i < terrain.Buffer.Length; i++)
        {
            uint pixel = terrain.Buffer[i];
            colors[i] = new Color(pixel);
        }

        _terrainTexture.SetData(colors);
        _spriteBatch!.Draw(_terrainTexture, Vector2.Zero, Color.White);
    }

    private void DrawSprites()
    {
        foreach (var (entity, sprite) in World.GetEntitiesWithComponent<SpriteComponent>())
        {
            var pos = World.GetComponent<WorldPosition>(entity);
            if (pos == null) continue;

            _spriteBatch!.Draw(
                _pixelTexture!,
                new Rectangle((int)pos.Position.X, (int)pos.Position.Y, (int)sprite.Size.X, (int)sprite.Size.Y),
                sprite.Color
            );
        }
    }

    private void DrawLines()
    {
        foreach (var (_, line) in World.GetEntitiesWithComponent<Line>())
        {
            DrawLine(line.P1, line.P2, line.Color);
        }
    }

    private void DrawLine(Vector2 start, Vector2 end, Color color)
    {
        float distance = Vector2.Distance(start, end);
        float angle = MathF.Atan2(end.Y - start.Y, end.X - start.X);

        _spriteBatch!.Draw(
            _pixelTexture!,
            start,
            null,
            color,
            angle,
            Vector2.Zero,
            new Vector2(distance, 1),
            SpriteEffects.None,
            0
        );
    }

    private void DrawParticles()
    {
        foreach (var (entity, particle) in World.GetEntitiesWithComponent<PixelParticle>())
        {
            _spriteBatch!.Draw(
                _pixelTexture!,
                particle.Position,
                null,
                particle.Color,
                0,
                Vector2.Zero,
                1,
                SpriteEffects.None,
                0
            );
        }
    }

    private void DrawHealthBars()
    {
        foreach (var (entity, health) in World.GetEntitiesWithComponent<Health>())
        {
            var pos = World.GetComponent<WorldPosition>(entity);
            var healthBar = World.GetComponent<HealthBar>(entity);

            if (pos == null || healthBar == null) continue;

            var barPos = pos.Position + healthBar.Offset;
            int barWidth = healthBar.Width;
            float healthRatio = health.Value / health.MaxHealth;

            int greenWidth = (int)(healthRatio * barWidth);
            int redWidth = barWidth - greenWidth;

            // Green part
            if (greenWidth > 0)
            {
                _spriteBatch!.Draw(
                    _pixelTexture!,
                    new Rectangle((int)barPos.X, (int)barPos.Y, greenWidth, 2),
                    GREEN_BAR
                );
            }

            // Red part
            if (redWidth > 0)
            {
                _spriteBatch!.Draw(
                    _pixelTexture!,
                    new Rectangle((int)barPos.X + greenWidth, (int)barPos.Y, redWidth, 2),
                    RED_BAR
                );
            }
        }
    }

    private void DrawFloatingText()
    {
        if (_font == null) return;

        foreach (var (_, text) in World.GetEntitiesWithComponent<FloatingText>())
        {
            _spriteBatch!.DrawString(_font, text.Text, text.Position, Color.White);
        }
    }

    public override void Update(float deltaTime)
    {
        // Rendering happens in Draw(), not Update()
    }
}
