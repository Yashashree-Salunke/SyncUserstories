using ReqspecModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Scheduler.Models;
using System.IO;

namespace Scheduler.Persistence
{
    public class ReqspecScheduleContext : DbContext
    {
        public ReqspecScheduleContext()
        {
        }

        public ReqspecScheduleContext(DbContextOptions<ReqspecScheduleContext> options) : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<JobSyncTracker> JobSyncTrackers { get; set; }
        public DbSet<UserstorySyncActionType> UserstorySyncActionTypes { get; set; }
        public DbSet<UserstorySyncTracker> UserstorySyncTrackers { get; set; }
    }

    public class ReqspecScheduleContextDesignFactory : IDesignTimeDbContextFactory<ReqspecScheduleContext>
    {
        public ReqspecScheduleContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ReqspecScheduleContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new ReqspecScheduleContext(optionsBuilder.Options);
        }
    }
}
