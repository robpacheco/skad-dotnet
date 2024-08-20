using System;
using System.Threading;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
=======
>>>>>>> unit7/m2
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Skad.Common.Db;

namespace Skad.Common.Host
{
    public static class WebHostExtensions
    {
        public static IHost MigrateDb(this IHost webHost)
        {
<<<<<<< HEAD
            var migrations = webHost.Services.GetService<Migrations>();
            migrations.MigrateDb();
=======
            var config = webHost.Services.GetService<IConfiguration>();
            var logger = webHost.Services.GetService<ILogger<IHost>>();

            try
            {
                var cnx = new NpgsqlConnection(config.GetConnectionString("DefaultConnection"));
                var evolve = new Evolve.Evolve(cnx, msg => logger.LogInformation(msg))
                {
                    Locations = new[] { "db/migrations" },
                    IsEraseDisabled = true,
                };

                evolve.Migrate();
                Thread.Sleep(120000);
            }
            catch (Exception ex)
            {
                logger.LogError("Database migration failed.", ex);
                throw;
            }
>>>>>>> unit7/m2
            
            return webHost;
        }
    }
}