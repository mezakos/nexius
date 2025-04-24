
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Nexius.API.DAL;
using Nexius.API.Services.Abstractions;
using Nexius.API.Services.Implementations;
using Nexius.ErrorHandling;

namespace Nexius.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<TodoContext>(optionsBuilder =>
              optionsBuilder
                .UseCosmos(
                  connectionString: builder.Configuration.GetConnectionString("DefaultConnection")!,
                  databaseName: "TodoDB",
                  cosmosOptionsAction: options =>
                  {
                      options.ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Direct);
                      options.MaxRequestsPerTcpConnection(16);
                      options.MaxTcpConnectionsPerEndpoint(32);
                  }));

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddScoped<ITodoService, TodoService>();
            builder.Services.AddSingleton<Mapper>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                using (var scope = app.Services.CreateScope())
                {
                    using var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
                    if (context?.Database is not null)
                    {
                        await context.Database.EnsureCreatedAsync();
                    }
                }
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseExceptionHandler();

            app.MapControllers();

            app.Run();
        }
    }
}
