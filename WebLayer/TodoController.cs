using Microsoft.AspNetCore.Mvc;
using Application2.ServiceLayer;
using System.Threading.Tasks;

namespace Application2.WebLayer
{

    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            this._todoService = todoService;
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateItemAsync([FromBody] CreateTodoItemRequest request)
        {
            var createdItem = await _todoService.CreateItemAsync(request);
            return CreatedAtRoute("", new { id = createdItem.Id }, createdItem);
        }

        [HttpGet]
        public async Task<ActionResult<TodoItemList>> FindAllItemsAsync()
        {
            return Ok(await _todoService.FindAllAsync());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TodoItem>> UpdateAsync(int id, [FromBody] UpdateTodoItemRequest request)
        {
            var todo = await _todoService.UpdateAsync(id, request);

            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TodoItem>> GetByIdAsync(int id)
        {
            var todo = await _todoService.GetByIdAsync(id);

            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

    }

}