public class TaskService
{
    private readonly TaskDatabase _taskDatabase;

    public TaskService(TaskDatabase taskDatabase)
    {
        _taskDatabase = taskDatabase;
    }

    public void Run()
    {
        var newTask = new TaskItem
        {
            Title = "New Task",
            Description = "This is a new task.",
            IsCompleted = false,
        };

        var newTask2 = new TaskItem
        {
            Title = "New Task 2",
            Description = "This is a new task 2.",
            IsCompleted = false,
        };

        _taskDatabase.CreateTask(newTask);
        _taskDatabase.ReadTask(newTask.Id);
        _taskDatabase.ReadTask(newTask2.Id);
        _taskDatabase.CreateTask(newTask2);
        _taskDatabase.ReadTask(newTask2.Id);
        _taskDatabase.UpdateTask(newTask.Id, new TaskItem
        {
            Id = newTask.Id,
            Title = "Updated Task",
            Description = "This is an updated task.",
            IsCompleted = true,
        });
        _taskDatabase.GetAllTasks();
        _taskDatabase.DeleteTask(newTask.Id);
        _taskDatabase.GetAllTasks();
        _taskDatabase.GetAllTasks();
    }
}


// typically controller calls service, calls caching layer, calls db. 
// Look at how I can alter cache time in IOptions. Create Json in app settings and create a model based of the json. configure that in program.cs
// make things async
