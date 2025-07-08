using Microsoft.Extensions.Caching.Memory;

public class TaskDatabase
{

    private List<TaskItem> _tasks = new();
    private readonly ILogger<TaskService> _logger;
    private readonly IMemoryCache _cache;
    public TaskDatabase(ILogger<TaskService> logger, IMemoryCache cache)
    {
        _cache = cache;
        _logger = logger;
    }

    public List<TaskItem> CreateTask(TaskItem task)
    {
        Guid id = Guid.NewGuid();
        task.Id = id;
        _tasks.Add(task);
        _logger.LogInformation($"Task created with ID: {task.Id}");
        UpdateCache();
        return _tasks;
    }

    public TaskItem? ReadTask(Guid id)
    {
        _logger.LogInformation($"Searching for task with ID: {id} ...");
        var task = _tasks.Find(t => t.Id == id);
        if (_cache.TryGetValue(id, out TaskItem? taskItem))
        {
            _logger.LogInformation($"Task found in cache: {taskItem}");
            return taskItem;
        } else if (task != null)
        {
            _logger.LogInformation($"Task found in db: {task}");
            _cache.Set(id, task, TimeSpan.FromMinutes(5)); 
            return task;
        } else {
            _logger.LogWarning($"Task with ID {id} not found.");
            return null;
        } 
    }

public List<TaskItem> UpdateTask(Guid id, TaskItem updatedTask)
{
    var task = _tasks.Find(task => task.Id == id);
    if (task != null)
    {
        int index = _tasks.IndexOf(task);
        _tasks[index] = updatedTask;
        UpdateCache();
        _logger.LogInformation($"Task updated to: {updatedTask};");
    }
    else
    {
        _logger.LogWarning($"Task with ID {id} not found.");
    }
    return _tasks;
}
    public List<TaskItem> DeleteTask(Guid id)
    {
        var taskToRemove = _tasks.FirstOrDefault(t => t.Id == id);

        if (taskToRemove != null)
        {
            _tasks.Remove(taskToRemove);
            UpdateCache();
            _logger.LogInformation($"Removing {taskToRemove.Id} from the Task List.");
        } else  {
            _logger.LogWarning($"Task with ID {id} not found.");
        }
       return _tasks;
    }

    public List<TaskItem> GetAllTasks()
    {
        List<TaskItem> tasks;
        if (_cache.TryGetValue("tasks", out tasks))
        {
            _logger.LogInformation("Retrieving tasks from cache... TASKS LIST:");
            foreach (var task in tasks)
            {
                _logger.LogInformation($"Cached Task: {task}");
            }
            return tasks;
        }

        _logger.LogInformation("Full Task List not in cache. Retreving tasks from db... TASKS LIST:");
        foreach (var task in _tasks)
        {
            _logger.LogInformation($"{task}");
        }
        UpdateCache();
        return _tasks;
    }
    
    private void UpdateCache()
    {
        _cache.Set("tasks", _tasks.ToList(), TimeSpan.FromMinutes(5));
        foreach (var task in _tasks)
        {
            _cache.Set(task.Id, task, TimeSpan.FromMinutes(5));
        }
    }
}

// think of how to cache request to db?? Done??

// if one of these methods didn't update the cache, it would affect the GetAllTasks... how do you protect against this?

// Type 'TaskItem' is not awaitable


// Decorate: updates on to cache, then onto repository, cannot not update cache or miss it
// cache service, https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-9.0 ,
// https://andrewlock.net/adding-decorated-classes-to-the-asp.net-core-di-container-using-scrutor/ 
// https://github.com/HillLuke/DesignPatterns/blob/main/DesignPatterns_Console/Structural/Decorator.cs
// https://refactoring.guru/design-patterns/decorator/csharp/example
// spin up MongoDb docker
// auth service
// auto mapper https://automapper.org/ making dto's 
// async https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/
// return completed/non-completed. lint filtering