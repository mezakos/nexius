namespace Nexius.API.Contracts.Requests
{
    public record ListTodosRequest
    {
        public string? Description { get; init; }

        public bool? IsCompleted { get; init; }

        public int Page { get; init; } = 1;

        public int PageSize { get; init; } = 25;
    }
}
