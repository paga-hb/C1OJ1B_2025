using Grpc.Core;
using Todo.GrpcCommon;
using Todo.GrpcCommon.Entitiies;

namespace Todo.GrpcServer.Services;

public class TodoItemsService : TodoItems.TodoItemsBase
{
    private readonly ILogger<TodoItemsService> _logger;
    private static List<TodoItem> _todoItems = new List<TodoItem>();

    public TodoItemsService(ILogger<TodoItemsService> logger)
    {
        _logger = logger;
    }

    // Create

    public override async Task<TodoItemMessage> Add(TodoItemMessage request,
                                                    ServerCallContext context)
    {
        if (string.IsNullOrEmpty(request.Title))
            throw new RpcException(new Status(
                StatusCode.InvalidArgument,
                $"The requested Title [{request.Title}] must have a value."
            ));

        TodoItem todoItem = new()
        {
            Id = _todoItems.Count + 1,
            Title = request.Title,
            Description = request.Description,
            IsDone = request.IsDone
        };

        _todoItems.Add(todoItem);

        TodoItemMessage response = new()
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            IsDone = todoItem.IsDone
        };

        _logger.LogDebug($"Add({request}) -> {response}");

        return await Task.FromResult(response);
    }

    // Read

    public override async Task<TodoItemMessage> Get(IdentityMessage request,
                                                    ServerCallContext context)
    {
        if (request.Id <= 0)
            throw new RpcException(new Status(
                StatusCode.InvalidArgument,
                $"The requested Id [{request.Id}] must be an integer larger than 0.")
            );

        TodoItem? todoItem = _todoItems.FirstOrDefault(item => item.Id == request.Id);

        if (todoItem == null)
            throw new RpcException(new Status(
                StatusCode.NotFound,
                $"The TodoItem with the requested Id [{request.Id}] does not exist.")
            );

        TodoItemMessage response = new()
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            IsDone = todoItem.IsDone
        };

        _logger.LogDebug($"Get({request}) -> {response}");

        return await Task.FromResult(response);
    }


    public override async Task<TodoItemListMessage> GetAll(EmptyMessage request,
                                                           ServerCallContext context)
    {
        TodoItemListMessage response = new();

        IEnumerable<TodoItem> todoItems = _todoItems.OrderBy(item => item.Id).ToList();

        foreach (TodoItem todoItem in todoItems)
        {
            response.Todoitems.Add(new TodoItemMessage()
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Description = todoItem.Description,
                IsDone = todoItem.IsDone
            });
        }

        _logger.LogDebug($"GetAll({request}) -> {response}");

        return await Task.FromResult(response);
    }

    public override async Task<TodoItemListMessage> GetMany(IdentityListMessage request,
                                                            ServerCallContext context)
    {
        TodoItemListMessage response = new();

        List<int> ids = new List<int>();
        foreach (IdentityMessage identity in request.Ids)
        {
            ids.Add(identity.Id);
        }

        List<TodoItem> todoItems = _todoItems
                                     .Where(item => ids.Contains(item.Id))
                                     .OrderBy(item => item.Id)
                                     .ToList();

        foreach (TodoItem todoItem in todoItems)
        {
            response.Todoitems.Add(new TodoItemMessage()
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Description = todoItem.Description,
                IsDone = todoItem.IsDone
            });
        }

        _logger.LogDebug($"GetMany({request}) -> {response}");
        
        return await Task.FromResult(response);
    }

    
    // Update
    public override async Task<IdentityMessage> Update(TodoItemMessage request,
                                                       ServerCallContext context)
    {
        if (request.Id <= 0 || string.IsNullOrEmpty(request.Title))
            throw new RpcException(new Status(
                StatusCode.InvalidArgument,
                $"The requested Id [{request.Id}] must be an integer larger than 0, " +
                $"and the requested Title [{request.Title}] must have a value.")
            );

        TodoItem? todoItem = _todoItems.FirstOrDefault(item => item.Id == request.Id);

        if (todoItem == null)
            throw new RpcException(new Status(
                StatusCode.NotFound, $"The requested Id [{request.Id}] does not exist.")
            );

        _todoItems.Remove(todoItem);

        todoItem.Title = request.Title;
        todoItem.Description = request.Description;
        todoItem.IsDone = request.IsDone;

        _todoItems.Add(todoItem);

        IdentityMessage response = new() { Id = todoItem.Id };
        _logger.LogDebug($"Update({request}) -> todoItems: {response}");
        return await Task.FromResult(response);
    }

    // Delete
    public override async Task<EmptyMessage> Delete(IdentityMessage request,
                                                    ServerCallContext context)
    {
        if (request.Id <= 0)
            throw new RpcException(new Status(
                StatusCode.InvalidArgument,
                $"The requested Id [{request.Id}] must be an integer larger than 0.")
            );

        TodoItem? todoItem = _todoItems.FirstOrDefault(item => item.Id == request.Id);

        if (todoItem == null)
            throw new RpcException(new Status(
                StatusCode.NotFound, $"The requested Id [{request.Id}] does not exist.")
            );

        _todoItems.Remove(todoItem);

        EmptyMessage response = new();
        _logger.LogDebug($"Delete({request}) -> todoItems: {response}");
        return await Task.FromResult(response);
    }
}