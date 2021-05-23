using RapidBlazor.Domain.Common;
using RapidBlazor.Domain.Entities;

namespace RapidBlazor.Domain.Events
{
    public class TodoItemCompletedEvent : DomainEvent
    {
        public TodoItemCompletedEvent(TodoItem item)
        {
            Item = item;
        }

        public TodoItem Item { get; }
    }
}
