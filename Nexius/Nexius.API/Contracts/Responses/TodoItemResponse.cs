namespace Nexius.API.Contracts.Responses
{
    public record TodoItemResponse
    {
        public Guid Id { get; init; }

        public string Description { get; init; } = "";

        public bool IsCompleted { get; init; }
    }
}
