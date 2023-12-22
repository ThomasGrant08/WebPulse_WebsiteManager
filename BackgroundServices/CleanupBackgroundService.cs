using WebPulse_WebManager.Services;

namespace WebPulse_WebManager.BackgroundServices
{
    public class CleanupBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CleanupBackgroundService> _logger; 

        public CleanupBackgroundService(IServiceProvider serviceProvider, ILogger<CleanupBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var cleanupService = scope.ServiceProvider.GetRequiredService<CleanupService>();
                        cleanupService.CleanupOldRecords();

                        _logger.LogInformation("Cleanup executed at: {time}", DateTimeOffset.Now);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing cleanup");
                }

                await Task.Delay(TimeSpan.FromDays(7), stoppingToken);
            }
        }

    }
}
