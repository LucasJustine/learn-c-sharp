using System.Text;
using Snake.Models;
using Snake.Core;

namespace Snake.Renderer;

public sealed class ConsoleRenderer : IRenderer
{
    private const string Reset = "\x1b[0m";

    private readonly StringBuilder _sb = new();
    private Cell[]? _grid;

    public void RenderGame(Game game)
    {
        int w = game.Board.Width;
        int h = game.Board.Height;

        if (_grid is null || _grid.Length != w * h)
            _grid = new Cell[w * h];

        Array.Clear(_grid);
        foreach (var s in game.Snake.Segments)
            _grid[s.Y * w + s.X] = Cell.Body;
        _grid[game.Snake.Head.Y * w + game.Snake.Head.X] = Cell.Head;
        _grid[game.Food.Position.Y * w + game.Food.Position.X] = Cell.Food;

        _sb.Clear();
        _sb.Append("\x1b[H");

        _sb.Append('┌').Append('─', w * 2).Append("┐\n");

        for (int y = 0; y < h; y++)
        {
            _sb.Append('│');
            for (int x = 0; x < w; x++)
                _sb.Append(Render(_grid[y * w + x]));
            _sb.Append("│\n");
        }

        _sb.Append('└').Append('─', w * 2).Append('┘'); 
        _sb.Append($"\nScore : {game.Score}\x1b[K");

        Console.Write(_sb.ToString());
    }

    private static string Render(Cell cell) => cell switch
    {
        Cell.Body => $"\x1b[32m██{Reset}",   // vert
        Cell.Head => $"\x1b[92m██{Reset}",   // vert clair
        Cell.Food => $"\x1b[31mX{Reset} ",   // rouge
        _         => $"\x1b[90m·{Reset} "    // gris
    };
}