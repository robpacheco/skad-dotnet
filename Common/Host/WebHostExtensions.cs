using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
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
            var migrations = webHost.Services.GetService<Migrations>();
            migrations.MigrateDb();
            
            return webHost;
        }
    }
}