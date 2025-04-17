using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using Todo.GrpcCommon;

var maxRetries = 10;
var delay = TimeSpan.FromSeconds(1);
var connected = false;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new Greeter.GreeterClient(channel);

for (int i = 0; i < maxRetries; i++)
{
    try
    {
        var reply = await client.SayHelloAsync(new HelloRequest { Name = "World!" });
        Console.WriteLine(reply.Message);
        connected = true;
        break;
    }
    catch (Exception ex) when (ex is HttpRequestException or SocketException or RpcException)
    {
        await Task.Delay(delay);
    }
}

if (!connected)
{
    Console.WriteLine("Failed to connect to gRPC service after multiple attempts.");
    return;
}

// Test the Todo Service
await TestTodoService();

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

static void PrintTodoItem(string prefix, TodoItemMessage todoItem)
{
    string message = $"{prefix} Id={todoItem.Id}, Title={todoItem.Title}, ";
    message += $"Description={todoItem.Description}, IsDone={todoItem.IsDone}";
    Console.WriteLine(message);
}

static async Task TestTodoService()
{
    // Create a GrpcChannel with the GrpcServer's address
    string address = "https://localhost:5001";
    GrpcChannel channel = GrpcChannel.ForAddress(address);

    // Create a gRPC client for the "Todo" service using the GrpcChannel
    TodoItems.TodoItemsClient _client = new TodoItems.TodoItemsClient(channel);

    // Add a first TodoItem
    TodoItemMessage todoItem = new TodoItemMessage()
    {
        Id = 0, Title = "Item1", Description = "Item one", IsDone = false
    };
    PrintTodoItem("\nAdding TodoItem ->", todoItem);
    todoItem = await _client.AddAsync(todoItem);
    PrintTodoItem("Added TodoItem ->", todoItem);

    // Add a second TodoItem
    todoItem = new TodoItemMessage()
    {
        Id = 0, Title = "Item2", Description = "Item two", IsDone = true
    };
    PrintTodoItem("\nAdding TodoItem ->", todoItem);
    todoItem = await _client.AddAsync(todoItem);
    PrintTodoItem("Added TodoItem ->", todoItem);

    // Get TodoItem with Id=1 (which will be the first TodoItem)
    IdentityMessage identity = new IdentityMessage() { Id = 1 };
    Console.WriteLine($"\nFetching TodoItem with Id={identity.Id}");
    todoItem = await _client.GetAsync(identity);
    PrintTodoItem("Fetched TodoItem ->", todoItem);

    // Get all TodoItems
    Console.WriteLine("\nFetching all TodoItems");
    TodoItemListMessage todoItems = await _client.GetAllAsync(new EmptyMessage());
    foreach (TodoItemMessage item in todoItems.Todoitems)
    {
        PrintTodoItem("Fetched TodoItem ->", item);
    }

    // Get list of TodoItems (with Ids 1 or 2)
    IdentityListMessage identities = new IdentityListMessage();
    identities.Ids.AddRange([new IdentityMessage() { Id=1 }, new IdentityMessage() { Id=2 }]);
    Console.WriteLine("\nFetching all TodoItems with Id=1 or Id=2");
    todoItems = await _client.GetManyAsync(identities);
    foreach (TodoItemMessage item in todoItems.Todoitems)
    {
        PrintTodoItem("Fetched TodoItem ->", item);
    }

    // Update
    todoItem = new TodoItemMessage()
    {
        Id = 1, Title = "Item1 updated", Description = "Item one updated", IsDone = true
    };
    PrintTodoItem("\nUpdating TodoItem ->", todoItem);
    identity = await _client.UpdateAsync(todoItem);
    Console.WriteLine($"TodoItem with Id={identity.Id} was updated");

    // Get all TodoItems
    Console.WriteLine("\nFetching all TodoItems");
    todoItems = await _client.GetAllAsync(new EmptyMessage());
    foreach (TodoItemMessage item in todoItems.Todoitems)
    {
        PrintTodoItem("Fetched TodoItem ->", item);
    }

    // Delete
    identity = new IdentityMessage() { Id = 1 };
    Console.WriteLine($"\nDeleting TodoItem with Id={identity.Id}");
    EmptyMessage emptyMessage = await _client.DeleteAsync(identity);

    // Get all TodoItems
    Console.WriteLine("\nFetching all TodoItems");
    todoItems = await _client.GetAllAsync(new EmptyMessage());
    foreach (TodoItemMessage item in todoItems.Todoitems)
    {
        PrintTodoItem("Fetched TodoItem ->", item);
    }
}