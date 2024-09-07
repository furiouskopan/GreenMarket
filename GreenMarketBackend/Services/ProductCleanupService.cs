using GreenMarketBackend.Data;

namespace GreenMarketBackend.Services
{
    public class ProductCleanupService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public ProductCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var expiredProducts = context.Products
                        .Where(p => p.DeletedAt != null && p.DeletedAt < DateTime.Now.AddDays(-30))
                        .ToList();

                    context.Products.RemoveRange(expiredProducts);
                    await context.SaveChangesAsync();
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
