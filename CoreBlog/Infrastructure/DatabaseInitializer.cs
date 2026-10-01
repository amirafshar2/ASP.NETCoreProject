using DAL.Concrete;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CoreBlog.Infrastructure
{
    /// <summary>Wendet das Datenbankschema an, spielt Beispieldaten ein und setzt die Demo-Datenbank zurück.</summary>
    public static class DatabaseInitializer
    {
        private static readonly SemaphoreSlim Lock = new(1, 1);

        public static async Task InitializeAsync(IServiceProvider services, bool reset)
        {
            await Lock.WaitAsync();
            try
            {
                using var scope = services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<Context>();
                var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Database");

                if (reset)
                {
                    logger.LogInformation("Demo-Datenbank wird zurückgesetzt …");
                    SqliteConnection.ClearAllPools();
                    await context.Database.EnsureDeletedAsync();
                }

                if (context.Database.GetMigrations().Any())
                    await context.Database.MigrateAsync();
                else
                    await context.Database.EnsureCreatedAsync();
                await scope.ServiceProvider.GetRequiredService<DataSeeder>().SeedAsync();
            }
            finally
            {
                Lock.Release();
            }
        }
    }
}
