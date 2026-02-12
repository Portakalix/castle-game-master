using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CastleGame.Core;

public class InGameGui
{
    private readonly Rectangle _archerButton;
    private readonly Rectangle _soldierButton;
    private readonly int _width;
    private readonly int _height;
    private MouseState _previousMouseState;

    public InGameGui(int width, int height)
    {
        _width = width;
        _height = height;
        _archerButton = new Rectangle(10, height - 50, 120, 40);
        _soldierButton = new Rectangle(140, height - 50, 120, 40);
        _previousMouseState = Mouse.GetState();
    }

    public GuiEvent Update()
    {
        var mouseState = Mouse.GetState();
        bool clicked = mouseState.LeftButton == ButtonState.Pressed && 
                      _previousMouseState.LeftButton == ButtonState.Released;

        _previousMouseState = mouseState;

        if (clicked)
        {
            var mousePos = new Point(mouseState.X, mouseState.Y);

            if (_archerButton.Contains(mousePos))
                return GuiEvent.BuyArcher;

            if (_soldierButton.Contains(mousePos))
                return GuiEvent.BuySoldier;
        }

        return GuiEvent.None;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel, SpriteFont? font)
    {
        // Draw archer button
        spriteBatch.Draw(pixel, _archerButton, new Color(60, 100, 60));
        if (font != null)
        {
            var archerText = "Buy Archer";
            var size = font.MeasureString(archerText);
            spriteBatch.DrawString(font, archerText, 
                new Vector2(_archerButton.X + (_archerButton.Width - size.X) / 2,
                           _archerButton.Y + (_archerButton.Height - size.Y) / 2), 
                Color.White);
        }

        // Draw soldier button
        spriteBatch.Draw(pixel, _soldierButton, new Color(100, 60, 60));
        if (font != null)
        {
            var soldierText = "Buy Soldier";
            var size = font.MeasureString(soldierText);
            spriteBatch.DrawString(font, soldierText,
                new Vector2(_soldierButton.X + (_soldierButton.Width - size.X) / 2,
                           _soldierButton.Y + (_soldierButton.Height - size.Y) / 2),
                Color.White);
        }
    }
}

public enum GuiEvent
{
    None,
    BuyArcher,
    BuySoldier
}
