using Microsoft.AspNetCore.Mvc;
using Nexius.API.Contracts.Requests;
using Nexius.API.Contracts.Responses;
using Nexius.API.Services.Abstractions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nexius.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController(ITodoService todoService, Mapper mapper) : BaseController
    {
        // GET: api/<TodoController>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ListTodosRequest? request = null)
        {
            if (request is null)
            {
                request = new ListTodosRequest();
            }

            var result = await todoService.ListTodos(request);

            return result.Match(success =>
            {
                return Ok(success.Select(x => mapper.Map<TodoItemResponse>(x)).ToList());
            },
            HandleNonSystemExceptions);
        }

        // GET api/<TodoController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] TodoItemRequest request)
        {
            var result = await todoService.GetTodoItem(request);
            return result.Match(success =>
            {
                return Ok(mapper.Map<TodoItemResponse>(success));
            },
            HandleNonSystemExceptions);
        }

        // POST api/<TodoController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewTodoItemRequest request)
        {
            var result = await todoService.AddTodoItem(request);
            return result.Match(success =>
            {
                return Ok(mapper.Map<TodoItemResponse>(success));
            },
            HandleNonSystemExceptions);
        }

        // PUT api/<TodoController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] EditTodoItemRequest request)
        {
            request = request with { Id = id };
            var result = await todoService.EditTodoItem(request);
            return result.Match(success =>
            {
                return Ok(mapper.Map<TodoItemResponse>(success));
            },
            HandleNonSystemExceptions);
        }

        // DELETE api/<TodoController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] TodoItemRequest request)
        {
            var result = await todoService.DeleteTodoItem(request);
            return result.Match(success =>
            {
                return Ok(mapper.Map<TodoItemResponse>(success));
            },
            HandleNonSystemExceptions);
        }
    }
}
