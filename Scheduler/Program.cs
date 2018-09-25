using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Scheduler.Persistence;
using System;
using System.Collections.Specialized;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Quartz.Logging;
using GitSync;

namespace Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceProvider serviceProvider = RegisterServiceProviders()
            .RunMigrations()
            .RunScheduler()
            ;
        }

        private static ServiceProvider RegisterServiceProviders()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var migrationAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var serviceProvider = new ServiceCollection()
                .AddLogging(p => p.AddConsole())
                .AddDbContext<ReqspecScheduleContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(migrationAssemblyName);
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
                    options.EnableSensitiveDataLogging();
                })
                .AddTransient<GitSyncJob>()
                .AddScoped<IGitSyncProcessor, GitSyncProcessor>()
                .Configure<ReqspecScheduleSetting>(configuration)
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
