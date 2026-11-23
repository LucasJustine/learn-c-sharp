using Snake.Core;
namespace Snake;

public class Program
{
    public static void Main(string[] args)
    {
        if (Console.WindowHeight < 20 + 3 || Console.WindowWidth < 30 * 2 + 2)
        {
            Console.WriteLine("Agrandis la fenêtre du terminal (au moins 60 x 20).");
            return;
        }
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();

        var game = new Game();
        game.Run();

        if(game.State == GameStates.GameOver)
        {
            Console.SetCursorPosition(0, game.Board.Height + 3);
            Console.WriteLine($"Game Over ! Score : {game.Score}");
        }
        else if(game.State == GameStates.Won)
        {
            Console.SetCursorPosition(0, game.Board.Height + 3);
            Console.WriteLine($"You won ! Score : {game.Score}");
        }
        Console.CursorVisible = true;
    }
}