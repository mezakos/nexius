using FluentValidation;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Nexius.API.Contracts.Requests;
using Nexius.API.DAL;
using Nexius.API.DAL.Models;
using Nexius.API.Services.Abstractions;

namespace Nexius.API.Services.Implementations
{
    public class TodoService(TodoContext context, IServiceProvider serviceProvider) : ITodoService
    {
        private ValidationException? Validate<T>(T item)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(typeof(T));
            var itemValidator = serviceProvider.GetRequiredService(validatorType) as IValidator<T>;

            if (itemValidator is not null)
            {
                var validationResult = itemValidator.Validate(item);

                if (!validationResult.IsValid)
                {
                    var exception = new ValidationException(validationResult.Errors);
                    return exception;
                }
            }

            return null;
        }

        public async Task<Result<TodoItem>> AddTodoItem(NewTodoItemRequest request, CancellationToken cancellationToken = default)
        {
            var validationResult = Validate(request);

            if (validationResult is not null)
            {
                return new Result<TodoItem>(validationResult);
            }

            var newTodoItem = new TodoItem
            {
                Id = Guid.CreateVersion7(),
                Description = request.Description,
                IsCompleted = request.IsCompleted
            };

            context.TodoItems.Add(newTodoItem);
            await context.SaveChangesAsync(cancellationToken);

            return newTodoItem;
        }

        public async Task<Result<TodoItem>> DeleteTodoItem(TodoItemRequest request, CancellationToken cancellationToken = default)
        {
            var validationResult = Validate(request);

            if (validationResult is not null)
            {
                return new Result<TodoItem>(validationResult);
            }

            var existingItem = await context.TodoItems.FindAsync(request.Id, cancellationToken);
            if (existingItem is null)
            {
                var exception = new KeyNotFoundException();
                return new Result<TodoItem>(exception);
            }

            context.Remove(existingItem);
            await context.SaveChangesAsync(cancellationToken);
            return existingItem;
        }

        public async Task<Result<TodoItem>> GetTodoItem(TodoItemRequest request, CancellationToken cancellationToken = default)
        {
            var validationResult = Validate(request);

            if (validationResult is not null)
            {
                return new Result<TodoItem>(validationResult);
            }

            var existingItem = await context.TodoItems.FindAsync(request.Id, cancellationToken);
            if (existingItem is null)
            {
                var exception = new KeyNotFoundException();
                return new Result<TodoItem>(exception);
            }

            return existingItem;
        }

        public async Task<Result<List<TodoItem>>> ListTodos(ListTodosRequest request, CancellationToken cancellationToken = default)
        {
            var validationResult = Validate(request);

            if (validationResult is not null)
            {
                return new Result<List<TodoItem>>(validationResult);
            }

            var todoItems = await context.TodoItems.Where(x => (request.Description != null ? x.Description.ToLower().Contains(request.Description.ToLower()) : x.Description == x.Description)
                                                               && x.IsCompleted == (request.IsCompleted ?? x.IsCompleted))
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return todoItems;
        }

        public async Task<Result<TodoItem>> EditTodoItem(EditTodoItemRequest request, CancellationToken cancellationToken = default)
        {
            var validationResult = Validate(request);

            if (validationResult is not null)
            {
                return new Result<TodoItem>(validationResult);
            }

            var existingItem = await context.TodoItems.FindAsync(request.Id, cancellationToken);
            if (existingItem is null)
            {
                var exception = new KeyNotFoundException();
                return new Result<TodoItem>(exception);
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                existingItem.Description = request.Description;
            }
            if (request.IsCompleted is not null)
            {
                existingItem.IsCompleted = request.IsCompleted.Value;
            }

            await context.SaveChangesAsync(cancellationToken);

            return existingItem;
        }
    }
}
