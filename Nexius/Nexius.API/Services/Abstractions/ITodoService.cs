using LanguageExt.Common;
using Nexius.API.Contracts.Requests;
using Nexius.API.DAL.Models;

namespace Nexius.API.Services.Abstractions
{
    public interface ITodoService
    {
        Task<Result<TodoItem>> GetTodoItem(TodoItemRequest request, CancellationToken cancellationToken = default);

        Task<Result<TodoItem>> AddTodoItem(NewTodoItemRequest request, CancellationToken cancellationToken = default);

        Task<Result<TodoItem>> EditTodoItem(EditTodoItemRequest request, CancellationToken cancellationToken = default);

        Task<Result<List<TodoItem>>> ListTodos(ListTodosRequest request, CancellationToken cancellationToken = default);

        Task<Result<TodoItem>> DeleteTodoItem(TodoItemRequest request, CancellationToken cancellationToken = default);
    }
}
