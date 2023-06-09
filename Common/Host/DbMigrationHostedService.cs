using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Skad.Common.Db;

namespace Skad.Common.Host;

public class DbMigrationHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    
    public DbMigrationHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using(var scope = _serviceProvider.CreateScope())
        {
            var migrations = scope.ServiceProvider.GetService<Migrations>();
            var migration = Task.Run(() => migrations.MigrateDb());
            await migration;
        }
    }
}