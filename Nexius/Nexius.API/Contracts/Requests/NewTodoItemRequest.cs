namespace Nexius.API.Contracts.Requests
{
    public record NewTodoItemRequest
    {
        public string Description { get; init; } = "";

        public bool IsCompleted { get; init; }
    }
}
