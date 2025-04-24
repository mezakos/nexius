namespace Nexius.API.Contracts.Requests
{
    public record EditTodoItemRequest
    {
        public Guid Id { get; init; }

        public string? Description { get; init; }

        public bool? IsCompleted { get; init; }
    }
}
