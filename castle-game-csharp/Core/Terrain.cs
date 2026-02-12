using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CastleGame.Core;

/// <summary>
/// Destructible terrain system using pixel buffer
/// </summary>
public class Terrain
{
    private readonly uint[] _buffer;
    private readonly int _width;
    private readonly int _height;
    private const uint TRANSPARENT_COLOR = 0xFFFF00FF; // Magenta = transparent

    public uint[] Buffer => _buffer;
    public int Width => _width;
    public int Height => _height;

    public Terrain(int width, int height)
    {
        _width = width;
        _height = height;
        _buffer = new uint[width * height];
        
        // Initialize with transparent color
        for (int i = 0; i < _buffer.Length; i++)
        {
            _buffer[i] = TRANSPARENT_COLOR;
        }
    }

    public bool LineCollides(Point start, Point end, out Point hitPoint)
    {
        // Bresenham line algorithm
        int x0 = start.X, y0 = start.Y;
        int x1 = end.X, y1 = end.Y;
        
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            if (x0 >= 0 && x0 < _width && y0 >= 0 && y0 < _height)
            {
                int index = x0 + y0 * _width;
                if ((_buffer[index] & 0xFFFFFF) != (TRANSPARENT_COLOR & 0xFFFFFF))
                {
                    hitPoint = new Point(x0, y0);
                    return true;
                }
            }

            if (x0 == x1 && y0 == y1) break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        hitPoint = Point.Zero;
        return false;
    }

    public bool RectCollides(Rectangle rect, out Point hitPoint)
    {
        int startX = Math.Max(0, rect.X);
        int startY = Math.Max(0, rect.Y);
        int endX = Math.Min(_width, rect.X + rect.Width);
        int endY = Math.Min(_height, rect.Y + rect.Height);

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                int index = x + y * _width;
                if (index >= 0 && index < _buffer.Length)
                {
                    if ((_buffer[index] & 0xFFFFFF) != (TRANSPARENT_COLOR & 0xFFFFFF))
                    {
                        hitPoint = new Point(x, y);
                        return true;
                    }
                }
            }
        }

        hitPoint = Point.Zero;
        return false;
    }

    public void DrawPixel(int x, int y, uint color)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
        {
            _buffer[x + y * _width] = color;
        }
    }

    public void DrawCircleMask(Point center, int radius)
    {
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (x * x + y * y <= radius * radius)
                {
                    DrawPixel(center.X + x, center.Y + y, TRANSPARENT_COLOR);
                }
            }
        }
    }

    public void DrawLine(Point start, Point end, uint color)
    {
        // Bresenham line algorithm
        int x0 = start.X, y0 = start.Y;
        int x1 = end.X, y1 = end.Y;
        
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            DrawPixel(x0, y0, color);

            if (x0 == x1 && y0 == y1) break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }
}
