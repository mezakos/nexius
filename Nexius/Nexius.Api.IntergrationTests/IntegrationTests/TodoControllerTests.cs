using Flurl;
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Nexius.API.Contracts.Requests;
using Nexius.API.Contracts.Responses;
using Nexius.API.DAL;
using System.Net.Http.Json;
using Xunit.Priority;

namespace Nexius.Api.Tests.IntegrationTests
{
    public class TodoControllerTests : IClassFixture<TodoApiWebApplicationFactory>, IAsyncLifetime
    {
        private readonly TodoApiWebApplicationFactory _factory;

        private readonly HttpClient _httpClient;

        public TodoControllerTests(TodoApiWebApplicationFactory factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            if (_factory.ServiceProvider is not null)
            {
                using var scope = _factory.ServiceProvider.CreateScope();
                using var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();
            }
        }

        [Fact, Priority(0)]
        public async Task NewTodoItemRequest_AddNewTodoItem()
        {
            var request = new NewTodoItemRequest
            {
                Description = "First todo"
            };

            var query = "api/todo";

            var response = await _httpClient.PostAsJsonAsync(query, request);

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode);

            Assert.IsType<TodoItemResponse>(JsonConvert.DeserializeObject<TodoItemResponse>(responseString));
        }

        [Fact, Priority(1)]
        public async Task ListTodosRequest_ListsTodos()
        {
            var request = new ListTodosRequest();
            var query = "api/todo".SetQueryParams(request);

            var response = await _httpClient.GetAsync(query);

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode);

            Assert.IsType<List<TodoItemResponse>>(JsonConvert.DeserializeObject<List<TodoItemResponse>>(responseString));
        }
    }
}
