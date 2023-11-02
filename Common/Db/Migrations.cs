using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Skad.Common.Db;

public class Migrations
{
    private IConfiguration _config;
    private readonly ILogger<Migrations> _logger;
    private bool _migrationsComplete;

    public Migrations(IConfiguration config, ILogger<Migrations> logger)
    {
        _config = config ?? throw new ArgumentException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public bool MigrationsComplete => _migrationsComplete;
    
    public void MigrateDb()
    {
        try
        {
            var cnx = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
            var evolve = new Evolve.Evolve(cnx, msg => _logger.LogInformation(msg))
            {
                Locations = new[] { "db/migrations" },
                IsEraseDisabled = true,
            };

            evolve.Migrate();
            
            var delay = Environment.GetEnvironmentVariable("READINESS_DELAY");

            if ((delay ?? "false") == "true")
            {
                Thread.Sleep(120000);
            }

            _migrationsComplete = true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database migration failed.", ex);
            throw;
        }
    }
}
