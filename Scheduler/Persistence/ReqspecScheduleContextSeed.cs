using ReqspecModels;
using Microsoft.EntityFrameworkCore;
using Scheduler.Models;
using System;
using System.Threading.Tasks;

namespace Scheduler.Persistence
{
    public class ReqspecScheduleContextSeed
    {
        public async Task SeedAsync(ReqspecScheduleContext context)
        {
            await JobTypesSeed(context);
            await UserstorySyncActionTypesSeed(context);
            await JobSyncTrackersSeed(context);
            await TenantsSeed(context);
        }

        private async Task TenantsSeed(ReqspecScheduleContext context)
        {
            if (await context.Tenants.AnyAsync() == false)
            {
                context.Tenants.Add(new Tenant { Code = "Tenant 1", AccessToken = "T1_T1", RepositoryUrl = "Tenant one repo.git", Username = "adhamankar", Password = "somepassword" });
                context.Tenants.Add(new Tenant { Code = "Tenant 2", AccessToken = "T2_T2", RepositoryUrl = "Tenant two repo.git" });
                await context.SaveChangesAsync();
            }
        }

        private async Task JobSyncTrackersSeed(ReqspecScheduleContext context)
        {
            if (await context.JobSyncTrackers.AnyAsync() == false)
            {
                context.JobSyncTrackers.Add(new JobSyncTracker { JobTypeId = 1, LastModifiedOn = DateTime.UtcNow });
                await context.SaveChangesAsync();
            }
        }

        private async Task UserstorySyncActionTypesSeed(ReqspecScheduleContext context)
        {
            if (await context.UserstorySyncActionTypes.AnyAsync() == false)
            {
                context.UserstorySyncActionTypes.Add(new UserstorySyncActionType { Code = "ADD" });
                context.UserstorySyncActionTypes.Add(new UserstorySyncActionType { Code = "DELETE" });
                context.UserstorySyncActionTypes.Add(new UserstorySyncActionType { Code = "UPDATE" });
                context.UserstorySyncActionTypes.Add(new UserstorySyncActionType { Code = "MOVE" });
                await context.SaveChangesAsync();
            }
        }

        private async Task JobTypesSeed(ReqspecScheduleContext context)
        {
            if (await context.JobTypes.AnyAsync() == false)
            {
                context.JobTypes.Add(new JobType { Code = "USERSTORYSYNC", Title = "Userstory sync job" });
                await context.SaveChangesAsync();
            }
        }
    }
}
