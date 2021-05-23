using RapidBlazor.Application.Common.Mappings;
using RapidBlazor.Domain.Entities;

namespace RapidBlazor.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
