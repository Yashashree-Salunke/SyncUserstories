using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scheduler.Persistence;
using System;

namespace Scheduler
{
    public static class MigrationExtension
    {
        public static IServiceProvider RunMigrations(this IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetService<ReqspecScheduleContext>();
            dbContext.Database.Migrate();
            new ReqspecScheduleContextSeed().SeedAsync(dbContext).Wait();

            return serviceProvider;
        }
    }
}
