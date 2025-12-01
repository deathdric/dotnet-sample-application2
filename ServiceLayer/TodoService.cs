using System.Linq;
using System.Threading.Tasks;
using Application2.DataLayer;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace Application2.ServiceLayer
{

    public interface ITodoService
    {
        Task<TodoItem> CreateItemAsync(CreateTodoItemRequest createTodoItemRequest);
        Task<TodoItemList> FindAllAsync();

        Task<TodoItem?> GetByIdAsync(int id);

        Task<TodoItem?> UpdateAsync(int id, UpdateTodoItemRequest todoItem);
    }




    public class TodoService : ITodoService
    {

        private readonly IAppDbContextFactory _dbContextFactory;

        public TodoService(IAppDbContextFactory dbContextFactory)
        {
            this._dbContextFactory = dbContextFactory;
        }

        public async Task<TodoItem> CreateItemAsync(CreateTodoItemRequest createTodoItemRequest)
        {
            var newDbItem = new DbTodoItem
            {
                Title = createTodoItemRequest.Title,
                Completed = false
            };
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync()) {
                dbContext.TodoItems.Add(newDbItem);
                await dbContext.SaveChangesAsync();
            }

            return ConvertItem(newDbItem);
        }

        private static TodoItem ConvertItem(DbTodoItem dbTodoItem) {
            return new TodoItem
            {
                Id = dbTodoItem.Id,
                Title = dbTodoItem.Title,
                Completed = dbTodoItem.Completed
            };
        }

        public async Task<TodoItemList> FindAllAsync()
        {

            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var itemList = await dbContext.TodoItems.Select(it => ConvertItem(it)).ToListAsync();
            return new TodoItemList
            {
                Items = itemList
            };
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var item = await dbContext.TodoItems.FindAsync(id);
            if (item is not null)
            {
                return ConvertItem(item);
            }
            return null;
        }

        public async Task<TodoItem?> UpdateAsync(int id, UpdateTodoItemRequest todoItem)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var item = await dbContext.TodoItems.FindAsync(id);
            if (item is null)
            {
                return null;
            }
            item.Title = todoItem.Title;
            item.Completed = todoItem.Completed;
            await dbContext.SaveChangesAsync();
            return ConvertItem(item);
        }
    }

}