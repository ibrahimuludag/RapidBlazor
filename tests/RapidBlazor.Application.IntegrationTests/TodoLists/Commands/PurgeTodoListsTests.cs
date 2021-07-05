using RapidBlazor.Application.Common.Exceptions;
using RapidBlazor.Application.Common.Security;
using RapidBlazor.Application.TodoLists.Commands.CreateTodoList;
using RapidBlazor.Application.TodoLists.Commands.PurgeTodoLists;
using RapidBlazor.Application.TodoLists.Queries.ExportTodos;
using RapidBlazor.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace RapidBlazor.Application.IntegrationTests.TodoLists.Commands
{
    using static Testing;

    public class PurgeTodoListsTests : TestBase
    {
        [Test]
        public void ShouldDenyAnonymousUser()
        {
            RunAsAnonymUser();

            var command = new PurgeTodoListsCommand();

            command.GetType().Should().BeDecoratedWith<AuthorizeAttribute>();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<UnauthorizedAccessException>();
        }

        [Test]
        public void ShouldDenyNonAdministrator()
        {
            RunAsDefaultUser();

            var command = new PurgeTodoListsCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ForbiddenAccessException>();
        }

        [Test]
        public void ShouldAllowAdministrator()
        {
            RunAsAdministrator();

            var command = new PurgeTodoListsCommand();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().NotThrow<ForbiddenAccessException>();
        }

        [Test]
        public async Task ShouldDeleteAllLists()
        {
            RunAsAdministrator();

            await SendAsync(new CreateTodoListCommand
            {
                Title = "New List #1"
            });

            await SendAsync(new CreateTodoListCommand
            {
                Title = "New List #2"
            });

            await SendAsync(new CreateTodoListCommand
            {
                Title = "New List #3"
            });

            await SendAsync(new PurgeTodoListsCommand());

            var count = await CountAsync<TodoList>();

            count.Should().Be(0);
        }
    }
}
