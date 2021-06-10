using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RapidBlazor.Domain.Entities;
using RapidBlazor.Domain.ValueObjects;
using RapidBlazor.Infrastructure.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RapidBlazor.DbMigration
{
    public static class SeedData
    {
        public static async Task EnsureSeeding(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Seed, if necessary
            if (!dbContext.TodoLists.Any())
            {
                dbContext.TodoLists.Add(new TodoList
                {
                    Title = "Shopping",
                    Colour = Colour.Blue,
                    Items =
                    {
                        new TodoItem { Title = "Apples", Done = true },
                        new TodoItem { Title = "Milk", Done = true },
                        new TodoItem { Title = "Bread", Done = true },
                        new TodoItem { Title = "Toilet paper" },
                        new TodoItem { Title = "Pasta" },
                        new TodoItem { Title = "Tissues" },
                        new TodoItem { Title = "Tuna" },
                        new TodoItem { Title = "Water" }
                    }
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}

