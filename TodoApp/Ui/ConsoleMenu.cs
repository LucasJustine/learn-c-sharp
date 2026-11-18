using TodoApp.Services;

namespace TodoApp.UI;

public class ConsoleMenu
{
    private readonly TodoService _todoService;
    private bool _isRunning = true;
    private readonly object _saveLock = new();

    public ConsoleMenu(TodoService todoService)
    {
        _todoService = todoService;
    }

    public void Run()
    {
        Console.CancelKeyPress += OnCancelKeyPress;
        try
        {
            while (_isRunning)
            {
                Console.Clear();
                PrintMenu();

                switch (Console.ReadLine())
                {
                    case "1": Add(); break;
                    case "2": List(); break;
                    case "3": Delete(); break;
                    case "0": _isRunning = false; continue;
                    case "4": Update(); break;
                    default: Console.WriteLine("Choix invalide"); break;
                }

                Pause();
            }
        }
        finally
        {
            Save();
        }
    }


    private void Save()
    {
        lock (_saveLock)
        {
            if (!_todoService.SaveTodos())
                Console.WriteLine("Erreur lors de la sauvegarde des todos");
        }
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        Save();
    }

    private static void Pause()
    {
        Console.WriteLine("\nAppuie sur une touche pour continuer...");
        Console.ReadKey(true);
    }

    private static void PrintMenu()
    {
        Console.WriteLine("""
            1. Ajouter un todo
            2. Lister les todos
            3. Supprimer un todo
            4. Mettre à jour un todo
            0. Quitter
        """);
        Console.Write("> ");
    }

    private void Add()
    {
        Console.Write("Titre : ");
        string? title;
        while (string.IsNullOrWhiteSpace(title = Console.ReadLine()))
        {
            Console.Write("Le titre ne peut pas être vide, réessaie : ");
        }

        if (string.Equals(title, "exit", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Le titre ne peut pas être vide, réessaie : ");
            return;
        }
        Console.WriteLine($"Todo ajouté : {title}");
        _todoService.AddTodo(title);
    }

    private void List()
    {
        foreach (var todo in _todoService.Todos)
            Console.WriteLine($"{todo.Id} - {todo.Title} [{(todo.IsCompleted ? "x" : " ")}]");
    }

    private void Delete()
    {
        Console.Write("Id : ");
        if (int.TryParse(Console.ReadLine(), out var id))
        {
            if (!_todoService.RemoveTodo(id))
                Console.WriteLine("Todo introuvable");
        }
    }

    private bool Update()
    {
        Console.Write("Id : ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Id invalide");
            return false;
        }

        var todo = _todoService.Todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            Console.WriteLine("Todo introuvable");
            return false;
        }

        Console.Write($"Titre ({todo.Title}) : ");
        var title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))
            title = todo.Title;

        Console.Write($"Complété ({(todo.IsCompleted ? "x" : " ")}) [y/n] : ");
        var isCompletedInput = Console.ReadLine();
        bool isCompleted = todo.IsCompleted;
        if (!string.IsNullOrWhiteSpace(isCompletedInput))
            isCompleted = isCompletedInput.Equals("y", StringComparison.OrdinalIgnoreCase);

        if (!_todoService.UpdateTodo(id, title, isCompleted))
        {
            Console.WriteLine("Erreur lors de la mise à jour du todo");
            return false;
        }

        Console.WriteLine("Todo mis à jour");
        return true;
    }

    
}