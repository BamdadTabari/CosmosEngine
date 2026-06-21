using Cosmos.Domain.Entities;
using Cosmos.Domain.ValueObjects;

namespace Cosmos.Application.Renderers;

public sealed class ConsoleUniverseRenderer
{
    private readonly Camera _camera = new();

    private readonly Dictionary<Guid, BodyRenderInfo> _styles =
        new();

    private readonly Dictionary<Guid, Queue<(int X, int Y)>> _trails =
        new();

    private readonly char[] _symbols =
    [
        'A','B','C','D','E',
        'F','G','H','I','J'
    ];

    private readonly ConsoleColor[] _colors =
    [
        ConsoleColor.Yellow,
        ConsoleColor.Cyan,
        ConsoleColor.Green,
        ConsoleColor.Red,
        ConsoleColor.Magenta,
        ConsoleColor.Blue,
        ConsoleColor.White
    ];

    public void Render(Universe universe)
    {
        const int width = 200;
        const int height = 200;

        Console.SetCursorPosition(0, 0);

        char[,] chars =
            new char[height, width];

        ConsoleColor[,] colors =
            new ConsoleColor[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                chars[y, x] = ' ';
                colors[y, x] = ConsoleColor.Black;
            }
        }

        var bodies =
            universe.Bodies.ToList();

        if (bodies.Count == 0)
        {
            return;
        }

        _camera.X =
            bodies[0].Position.X;

        _camera.Y =
            bodies[0].Position.Y;

        int index = 0;

        foreach (var body in bodies)
        {
            if (!_styles.ContainsKey(body.Id))
            {
                _styles[body.Id] =
                    new BodyRenderInfo(
                        _symbols[index % _symbols.Length],
                        _colors[index % _colors.Length]);

                index++;
            }

            var style =
                _styles[body.Id];

            int screenX =
                (int)(
                    ((body.Position.X - _camera.X)
                    * _camera.Zoom)
                    + width / 2);

            int screenY =
                (int)(
                    ((body.Position.Y - _camera.Y)
                    * _camera.Zoom)
                    + height / 2);

            if (!_trails.ContainsKey(body.Id))
            {
                _trails[body.Id] =
                    new Queue<(int, int)>();
            }

            var trail =
                _trails[body.Id];

            trail.Enqueue((screenX, screenY));

            while (trail.Count > 100)
            {
                trail.Dequeue();
            }

            foreach (var point in trail)
            {
                if (
                    point.X < 0 ||
                    point.X >= width ||
                    point.Y < 0 ||
                    point.Y >= height)
                {
                    continue;
                }

                chars[point.Y, point.X] = '.';
                colors[point.Y, point.X] =
                    ConsoleColor.DarkGray;
            }

            if (
                screenX < 0 ||
                screenX >= width ||
                screenY < 0 ||
                screenY >= height)
            {
                continue;
            }

            chars[screenY, screenX] =
                style.Symbol;

            colors[screenY, screenX] =
                style.Color;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.ForegroundColor =
                    colors[y, x];

                Console.Write(chars[y, x]);
            }

            Console.WriteLine();
        }

        Console.ResetColor();
    }
}