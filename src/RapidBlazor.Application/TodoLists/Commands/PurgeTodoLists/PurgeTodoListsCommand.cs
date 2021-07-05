using RapidBlazor.Application.Common.Interfaces;
using RapidBlazor.Application.Common.Security;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RapidBlazor.Shared.Security;

namespace RapidBlazor.Application.TodoLists.Commands.PurgeTodoLists
{
    [Authorize(Roles = Constants.AdminRole)]
    [Authorize(Policy = nameof(Policies.PurgePolicy))]
    public class PurgeTodoListsCommand : IRequest
    {
    }

    public class PurgeTodoListsCommandHandler : IRequestHandler<PurgeTodoListsCommand>
    {
        private readonly IApplicationDbContext _context;

        public PurgeTodoListsCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PurgeTodoListsCommand request, CancellationToken cancellationToken)
        {
            _context.TodoLists.RemoveRange(_context.TodoLists);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
