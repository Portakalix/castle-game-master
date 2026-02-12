using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CastleGame.Core.ECS;
using CastleGame.Core.Systems;
using CastleGame.Core;

namespace CastleGame;

public class Game1 : Game
{
    private const int WIDTH = 1280;
    private const int HEIGHT = 540;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private World _world;
    private RenderingSystem? _renderingSystem;
    private InGameGui? _gui;
    private Texture2D? _pixelTexture;
    private readonly Color _backgroundColor = new Color(135, 206, 235); // Sky blue

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = WIDTH;
        _graphics.PreferredBackBufferHeight = HEIGHT;

        _world = new World();
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Initialize terrain
        var terrain = new Terrain(WIDTH, HEIGHT);
        
        // Draw simple ground
        for (int y = 350; y < HEIGHT; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                uint color = 0xFF8B4513; // Brown
                terrain.DrawPixel(x, y, color);
            }
        }

        // Draw castle walls
        for (int y = 250; y < 350; y++)
        {
            for (int x = 1200; x < WIDTH; x++)
            {
                terrain.DrawPixel(x, y, 0xFF808080); // Gray
            }
        }

        _world.AddResource(terrain);

        // Add all systems in correct order
        _world.AddSystem(new ProjectileSystem());
        _world.AddSystem(new TurretSystem());
        _world.AddSystem(new WalkSystem());
        _world.AddSystem(new UnitFallSystem());
        _world.AddSystem(new UnitResumeWalkingSystem());
        _world.AddSystem(new UnitCollisionSystem());
        _world.AddSystem(new MeleeSystem());
        _world.AddSystem(new PhysicsSystem());
        _world.AddSystem(new ParticleSystem());
        _world.AddSystem(new FloatingTextSystem());

        _renderingSystem = new RenderingSystem();
        _world.AddSystem(_renderingSystem);
        _renderingSystem.Initialize(GraphicsDevice);

        // Initialize GUI
        _gui = new InGameGui(WIDTH, HEIGHT);

        // Place enemies and turrets
        LevelSetup.PlaceTurrets(_world, 1);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Update world
        _world.Update(deltaTime);

        // Update GUI and handle events
        if (_gui != null)
        {
            var guiEvent = _gui.Update();
            
            switch (guiEvent)
            {
                case GuiEvent.BuyArcher:
                    LevelSetup.BuyArcher(_world);
                    break;
                case GuiEvent.BuySoldier:
                    LevelSetup.BuySoldier(_world);
                    break;
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(_backgroundColor);

        // Draw game
        _renderingSystem?.Draw();

        // Draw GUI
        if (_spriteBatch != null && _pixelTexture != null && _gui != null)
        {
            _spriteBatch.Begin();
            _gui.Draw(_spriteBatch, _pixelTexture, null);
            _spriteBatch.End();
        }

        base.Draw(gameTime);
    }
}
