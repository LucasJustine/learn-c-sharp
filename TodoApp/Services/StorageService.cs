using TodoApp.Models;
using System.Text.Json;

namespace TodoApp.Services;

public class StorageService(string filePath)
{
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    public bool SaveTodos(IEnumerable<Todo> todos)
    {
        string? directory = Path.GetDirectoryName(Path.GetFullPath(filePath));
        if (directory is not null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        try
        {
            var json = JsonSerializer.Serialize(todos, _options);
            File.WriteAllText(filePath, json);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<Todo> LoadTodos()
    {
        if (!File.Exists(filePath))
            return new List<Todo>();

        var json = File.ReadAllText(filePath);
        try
        {
            return JsonSerializer.Deserialize<List<Todo>>(json) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }
}