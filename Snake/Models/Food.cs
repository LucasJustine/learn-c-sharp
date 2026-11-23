namespace Snake.Models;

public class Food
{
    private readonly Random _random = new();
    public Position Position { get; private set; }

    public Food(SnakeBody snakeBody, Board board)
    {
        Respawn(snakeBody, board);
    }

    public void Respawn(SnakeBody snakeBody, Board board)
    {
        Position newPosition;
        do
        {
            newPosition = new Position(_random.Next(board.Width), _random.Next(board.Height));
        } while (snakeBody.Segments.Contains(newPosition));

        Position = newPosition;
    }
}