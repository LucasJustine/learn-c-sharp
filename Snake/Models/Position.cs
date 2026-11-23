using Snake.Core;

namespace Snake.Models;
public record struct Position(int X, int Y)
{
    public readonly Position Up() => new(X, Y - 1);
    public readonly Position Down() => new(X, Y + 1);
    public readonly Position Left() => new(X - 1, Y);
    public readonly Position Right() => new(X + 1, Y);

    public readonly Position Move(Direction d) => d switch
    {
        Direction.Up    => Up(),
        Direction.Down  => Down(),
        Direction.Left  => Left(),
        Direction.Right => Right(),
        _ => this
    };
}