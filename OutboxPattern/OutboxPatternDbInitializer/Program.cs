using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace OutboxPatternDbInitializer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = CreateServices();

            // Executa as migrations
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres() // 👈 banco de destino
.WithGlobalConnectionString("Host=postgres;Port=5432;Database=mydatabase;Username=user;Password=password")

                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp(); // roda todas as migrations pendentes
        }
    }
}
