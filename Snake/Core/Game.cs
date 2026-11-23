using Snake.Models;
using Snake.Renderer;
using Snake.Input;
using System.Diagnostics;

namespace Snake.Core;

public class Game
{
    private readonly InputHandler _input = new();
    private readonly ConsoleRenderer _renderer = new();
    private Direction _direction = Direction.Right;
    public Board Board { get; }
    public SnakeBody Snake { get; }
    public Food Food { get; }
    public GameStates State { get; private set; }
    public int Score { get; private set; }
    private const int StartCrossTimeMs = 4500;   // traversée au départ
    private const int MinCrossTimeMs   = 1800;   // traversée à vitesse max
    private const double FullSpeedFill = 0.10;   // vitesse max atteinte quand le serpent remplit 10 % du plateau

    private readonly double _minDelay;
    private readonly double _delayStep;
    private double _gameSpeed;


    public Game(int width = 30, int height = 20)
    {
        Board = new Board { Width = width, Height = height };
        Snake = new SnakeBody(new Position(width / 2, height / 2));
        Food = new Food(Snake, Board);
        State = GameStates.Paused;

        int longSide = Math.Max(width, height);
        double startDelay = StartCrossTimeMs / (double)longSide;
        _minDelay = MinCrossTimeMs / (double)longSide;

        int applesToMaxSpeed = Math.Max(1, (int)(width * height * FullSpeedFill));
        _delayStep = (startDelay - _minDelay) / applesToMaxSpeed;

        _gameSpeed = startDelay;
    }

    public void Run()
    {
        State = GameStates.Playing;
        _renderer.RenderGame(this);
        Console.Clear();
        var sw = Stopwatch.StartNew();
        while (State == GameStates.Playing)
        {
            sw.Restart();
            _input.Poll(_direction);
            _direction = _input.Next(_direction);
            Update();
            _renderer.RenderGame(this);
            while (sw.ElapsedMilliseconds < _gameSpeed)
            {
                _input.Poll(_direction);
                Thread.Sleep(5);
            }
        }

    }

    private void Update()
    {
        var newHead = Snake.Head.Move(_direction);

        if (!Board.IsInside(newHead) || Snake.Segments.Contains(newHead))
        {
            State = GameStates.GameOver;
            return;
        }

        if (newHead == Food.Position)
        {
            Snake.Grow(newHead);
            Score++;
            _gameSpeed = Math.Max(_minDelay, _gameSpeed - _delayStep);

            if (Snake.Segments.Count == Board.Width * Board.Height)
            {
                State = GameStates.Won;
                return;                
            }

            Food.Respawn(Snake, Board);
        }
        else
        {
            Snake.Move(newHead);
        }
    }

    private bool Won() => Snake.Segments.Count == Board.Width * Board.Height;
}