using TodoApp.Services;
using TodoApp.UI;

namespace TodoApp;

public class Program
{
    public static void Main(string[] args)
    {
        var storagePath = args.Length > 0 ? args[0] : "todos.json";

        storagePath = InitializeStorage(storagePath);

        var storageService = new StorageService(storagePath);
        var service = new TodoService(storageService);
        var menu = new ConsoleMenu(service);

        menu.Run();
    }

    static string InitializeStorage(string defaultPath)
    {
        Console.WriteLine($"Utiliser le fichier {defaultPath} ? (y/n) : ");
        Console.Write("> ");
        if (string.Equals(Console.ReadLine()?.Trim(), "y", StringComparison.OrdinalIgnoreCase))
            return defaultPath;

        string path = AskPath();

        return NormalizeStoragePath(path.Trim());
    }

    static string AskPath()
    {
        string? path;
        do
        {
            Console.Write("Chemin du fichier : ");
            path = Console.ReadLine();

            if (path is null && Console.IsInputRedirected)
            {
                Console.WriteLine();
                Console.Write("Entrée fermée, arrêt du programme.");
                Environment.Exit(0);
            }

            if (string.IsNullOrWhiteSpace(path))
                Console.WriteLine("Le chemin ne peut pas être vide, réessaie.");

        } while (string.IsNullOrWhiteSpace(path));

        return path.Trim();
    }


    static string NormalizeStoragePath(string input)
    {
        var path = input.Trim().Trim('"');

        if (path.EndsWith(Path.DirectorySeparatorChar) ||
            path.EndsWith(Path.AltDirectorySeparatorChar) ||
            Directory.Exists(path))
        {
            path = Path.Combine(path, "todos.json");
        }

        if (string.IsNullOrEmpty(Path.GetExtension(path)))
        {
            path += ".json";
        }

        return path;
    }


}