using Snake.Core;

namespace Snake.Input;

public class InputHandler
{
    private readonly Queue<Direction> _pending = new();

    public void Poll(Direction current)
    {
        Direction last = _pending.Count > 0 ? _pending.Last() : current;

        while (Console.KeyAvailable)
        {
            Direction? wanted = Console.ReadKey(true).Key switch
            {
                ConsoleKey.UpArrow    => Direction.Up,
                ConsoleKey.DownArrow  => Direction.Down,
                ConsoleKey.LeftArrow  => Direction.Left,
                ConsoleKey.RightArrow => Direction.Right,
                _                     => null
            };

            if (wanted is null || wanted == last || IsOpposite(last, wanted.Value))
                continue;

            if (_pending.Count < 3)
                _pending.Enqueue(wanted.Value);
            
            last = wanted.Value;
        }
    }

    public Direction Next(Direction current) =>
        _pending.Count > 0 ? _pending.Dequeue() : current;

    private static bool IsOpposite(Direction a, Direction b) =>
        (a, b) is (Direction.Up, Direction.Down)
               or (Direction.Down, Direction.Up)
               or (Direction.Left, Direction.Right)
               or (Direction.Right, Direction.Left);
}