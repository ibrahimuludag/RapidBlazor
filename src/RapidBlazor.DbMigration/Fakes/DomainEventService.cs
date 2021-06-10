using RapidBlazor.Application.Common.Interfaces;
using RapidBlazor.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidBlazor.DbMigration.Fakes
{
    public class DomainEventService : IDomainEventService
    {
        public Task Publish(DomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
