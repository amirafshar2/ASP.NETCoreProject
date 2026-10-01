using DAL.Concrete;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CoreBlog.Infrastructure
{
    /// <summary>Setzt die Demo-Daten in einem festen Intervall automatisch zurück.</summary>
    public class DemoResetService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly DemoOptions _demo;
        private readonly ILogger<DemoResetService> _logger;

        public DemoResetService(IServiceProvider services, IOptions<DemoOptions> demo, ILogger<DemoResetService> logger)
        {
            _services = services;
            _demo = demo.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_demo.Enabled || _demo.ResetIntervalHours <= 0) return;

            using var timer = new PeriodicTimer(TimeSpan.FromHours(_demo.ResetIntervalHours));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await DatabaseInitializer.InitializeAsync(_services, reset: true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Zurücksetzen der Demo-Datenbank fehlgeschlagen.");
                }
            }
        }
    }
}
