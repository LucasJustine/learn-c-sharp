using TodoApp.Models;

namespace TodoApp.Services;
public class TodoService
{
    private readonly StorageService _storage;
    private readonly List<Todo> _todos;
    public IReadOnlyList<Todo> Todos => _todos; 
    private int _nextId = 1;


    public TodoService(StorageService storage)
    {
        _storage = storage;
        _todos = storage.LoadTodos();

        _nextId = _todos.Count == 0 ? 1 : _todos.Max(t => t.Id) + 1;
    }

    public Todo AddTodo(string title)
    {
        var todo = new Todo(_nextId++, title);
        _todos.Add(todo);
        return todo;
    }

    public bool RemoveTodo(int id)
    {
        return _todos.RemoveAll(t => t.Id == id) > 0;
    }

    public bool UpdateTodo(int id, string title, bool isCompleted)
    {
        var index = _todos.FindIndex(t => t.Id == id);
        if (index == -1) return false;
        _todos[index] = _todos[index] with { Title = title, IsCompleted = isCompleted };
        
        return true;
    }

    public bool SaveTodos()
    {
        return _storage.SaveTodos(_todos);
    }
    
}