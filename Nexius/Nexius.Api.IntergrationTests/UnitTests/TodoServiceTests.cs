using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;
using Nexius.API;
using Nexius.API.Contracts.Requests;
using Nexius.API.DAL;
using Nexius.API.DAL.Models;
using Nexius.API.Services.Implementations;

namespace Nexius.Api.Tests.UnitTests
{
    public class TodoServiceTests
    {
        private readonly IServiceProvider _serviceProvider;

        public TodoServiceTests()
        {
            _serviceProvider = new ServiceCollection()
                .AddValidatorsFromAssemblyContaining<Program>()
                .BuildServiceProvider();
        }

        [Fact]
        public async Task ListTodos_ReturnsEmptySequence_When_NoItemIsAvailable()
        {
            // Arrange
            var dbContextMock = new Mock<TodoContext>();
            dbContextMock.Setup(x => x.TodoItems).ReturnsDbSet(Array.Empty<TodoItem>());

            var sut = new TodoService(dbContextMock.Object, _serviceProvider);

            // Act
            var result = await sut.ListTodos(new ListTodosRequest());

            // Assert
            Assert.True(result.IsSuccess);

            result.IfSucc(success =>
            {
                Assert.Empty(success);
            });
        }

        [Fact]
        public async Task AddTodoItem_ReturnsNewlyAddedTodoItem_When_ValidRequestSent()
        {
            // Arrange
            var dbContextMock = new Mock<TodoContext>();
            dbContextMock.Setup(x => x.TodoItems).ReturnsDbSet(Array.Empty<TodoItem>());

            var sut = new TodoService(dbContextMock.Object, _serviceProvider);

            // Act
            var result = await sut.AddTodoItem(new NewTodoItemRequest
            {
                Description = "Todo Test"
            });

            // Assert
            Assert.True(result.IsSuccess);

            result.IfSucc(success =>
            {
                Assert.NotNull(success);
            });
        }
    }
}
