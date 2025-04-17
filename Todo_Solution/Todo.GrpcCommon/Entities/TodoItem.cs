namespace Todo.GrpcCommon.Entitiies;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = String.Empty;
    public bool IsDone { get; set; } = false;

    public override string ToString()
    {
        return $"Id: {Id}, Title: {Title}, Description: {Description}, IsDone: {IsDone}";
    }
}