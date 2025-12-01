using System.Collections.Generic;

namespace Application2.ServiceLayer
{

    public class TodoItem
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public bool Completed { get; set; }
    }

    public class TodoItemList
    {
        public required IList<TodoItem> Items { get; set; }
    }

    public class CreateTodoItemRequest
    {
        public required string Title { get; set; }
    }

    public class UpdateTodoItemRequest
    {
        public required string Title { get; set; }
        public bool Completed { get; set; }
    }
}