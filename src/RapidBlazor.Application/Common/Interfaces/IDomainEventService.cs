using RapidBlazor.Domain.Common;
using System.Threading.Tasks;

namespace RapidBlazor.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
