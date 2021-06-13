using RapidBlazor.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace RapidBlazor.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
