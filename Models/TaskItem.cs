public class TaskItem
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }

    public override string ToString()
    {
        return $"Task ID: {Id}, Title: {Title}, Description: {Description}, Completed: {IsCompleted}";
    }

}
