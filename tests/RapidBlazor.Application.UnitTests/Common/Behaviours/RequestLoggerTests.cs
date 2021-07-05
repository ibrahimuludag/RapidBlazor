using RapidBlazor.Application.Common.Behaviours;
using RapidBlazor.Application.Common.Interfaces;
using RapidBlazor.Application.TodoItems.Commands.CreateTodoItem;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace RapidBlazor.Application.UnitTests.Common.Behaviours
{
    public class RequestLoggerTests
    {
        [Test]
        public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
        {
            var currentUserService = new Mock<ICurrentUserService>();
            var logger = new Mock<ILogger<CreateTodoItemCommand>>();

            currentUserService.Setup(x => x.UserId).Returns("Administrator");

            var requestLogger = new LoggingBehaviour<CreateTodoItemCommand>(logger.Object, currentUserService.Object);

            await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());

            currentUserService.Verify(i => i.GetUserName(), Times.Once);
        }

        [Test]
        public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
        {
            var currentUserService = new Mock<ICurrentUserService>();
            var logger = new Mock<ILogger<CreateTodoItemCommand>>();

            var requestLogger = new LoggingBehaviour<CreateTodoItemCommand>(logger.Object, currentUserService.Object);

            await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());

            currentUserService.Verify(i => i.GetUserName(), Times.Never);
        }
    }
}
