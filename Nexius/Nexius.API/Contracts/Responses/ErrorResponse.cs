namespace Nexius.API.Contracts.Responses
{
    public record ErrorResponse
    {
        public int StatusCode { get; init; }

        public dynamic ErrorDetails { get; init; } = default!;
    }
}
