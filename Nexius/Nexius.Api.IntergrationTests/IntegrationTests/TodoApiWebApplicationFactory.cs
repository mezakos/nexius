using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nexius.API;
using Nexius.API.DAL;

namespace Nexius.Api.Tests.IntegrationTests
{
    public class TodoApiWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly IConfiguration _configuration;

        public IServiceProvider? ServiceProvider { get; private set; }

        public TodoApiWebApplicationFactory()
        {
            _configuration = new ConfigurationBuilder()
              .SetBasePath(AppContext.BaseDirectory)
              .AddJsonFile("appsettings.json", false, true)
              .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<DbContextOptions<TodoContext>>();

                services.AddDbContext<TodoContext>(optionsBuilder =>
                      optionsBuilder
                        .UseCosmos(
                          connectionString: _configuration.GetConnectionString("DefaultConnection")!,
                          databaseName: "TodoDB_Test",
                          options =>
                          {
                              options.ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Direct);
                              options.MaxRequestsPerTcpConnection(16);
                              options.MaxTcpConnectionsPerEndpoint(32);
                          }));
                ServiceProvider = services.BuildServiceProvider();
            });
        }
    }
}
