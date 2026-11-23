namespace Snake.Models;

public class Board
{
    public int Width { get; init; }
    public int Height { get; init; }

    public bool IsInside(Position position)
    {
        return position.X >= 0 && position.X < Width &&
               position.Y >= 0 && position.Y < Height;
    }
}