using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Skad.Common.Auth;
using Skad.Common.Db;
using Skad.Common.Host;
using Skad.Common.Http;
using Skad.Subscription.Config;
using Skad.Subscription.Data;
using Skad.Subscription.Data.Model;
using Skad.Subscription.Domain.Repository;
using Skad.Subscription.Domain.Service;
using Skad.Subscription.MvcControllers.ViewModels;

namespace Skad.Subscription
{
    public class Startup
    {
        private static string ENDPOINT_SETTINGS_SECTION = "EndpointSettings";
        private static string SUBSCRIPTION_TIER_SETTINGS_SECTION = "SubscriptionTierSettings";
        private static string RECEIPT_SETTINGS_SECTION = "ReceiptSettings";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SetupLogging();
            
            services.AddMvc();
            services.AddControllers();

            services.Configure<EndpointSettings>(Configuration.GetSection(ENDPOINT_SETTINGS_SECTION));
            services.Configure<SubscriptionTierSettings>(Configuration.GetSection(SUBSCRIPTION_TIER_SETTINGS_SECTION));
            services.Configure<ReceiptSettings>(Configuration.GetSection(RECEIPT_SETTINGS_SECTION));
            
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.All;
            });

            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            
            services.AddScoped<ISubscriptionService, SubscriptionService>();

            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IReceiptManager, InMemoryReceiptManager>();
            services.AddSingleton<Migrations>();
            services.AddScoped<LinkGenerator>();
            services.AddScoped<SubscriptionLinkGenerator>();

            services.Configure<OauthSettings>(Configuration.GetSection("OauthProxySettings"));
            services.AddScoped<IUserInfoAccessor, ProxyUserInfoAccessor>();
            
            services.AddHostedService<DbMigrationHostedService>();
            
            services.AddDbContext<SubscriptionDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            loggerFactory.AddSerilog();
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        
        private void SetupLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom
                .Configuration(Configuration)
                .CreateLogger();
        }
    }
}
