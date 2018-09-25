using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public class QuartzStartup
    {
        private IScheduler _scheduler; // after Start, and until shutdown completes, references the scheduler object
        private readonly IServiceProvider _serviceProvider;

        public QuartzStartup(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        // starts the scheduler, defines the jobs and the triggers
        public async Task Start()
        {
            if (_scheduler != null)
            {
                throw new InvalidOperationException("Already started.");
            }

            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler.JobFactory = new CustomJobFactory(_serviceProvider);
            await _scheduler.Start();

            IJobDetail gitSyncJob = JobBuilder.Create<GitSyncJob>()
                                                .WithIdentity("GitSyncJob", "GitSyncGroup")
                                                .Build();

            ITrigger tenSecondsTrigger = TriggerBuilder.Create()
                                                .WithIdentity("tenSecondsTrigger", "GitSyncGroup")
                                                .StartNow()
                                                .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
                                                .Build();

            await _scheduler.ScheduleJob(gitSyncJob, tenSecondsTrigger);
        }

        // initiates shutdown of the scheduler, and waits until jobs exit gracefully (within allotted timeout)
        public void Stop()
        {
            if (_scheduler != null)
            {
                if (_scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
                {
                    _scheduler = null;
                }
                else
                {
                    // jobs didn't exit in timely fashion - log a warning...
                }
            }
        }
    }
}
