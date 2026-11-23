namespace Snake.Models;

public sealed class SnakeBody
{
    private readonly Queue<Position> _segments = new();

    public SnakeBody(Position start)
    {
        Head = start;
        _segments.Enqueue(start);
    }

    public Position Head { get; private set; }

    public IReadOnlyCollection<Position> Segments => _segments;

    public Position Move(Position newHead)
    {
        _segments.Enqueue(newHead);
        Head = newHead;
        return _segments.Dequeue();
    }

    public void Grow(Position newHead)
    {
        _segments.Enqueue(newHead);
        Head = newHead;
    }

    public bool CollidesWithSelf() => _segments.Count(s => s == Head) > 1;
}