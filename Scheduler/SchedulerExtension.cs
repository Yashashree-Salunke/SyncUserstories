using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    public static class SchedulerExtension
    {
        public static  IServiceProvider RunScheduler(this IServiceProvider serviceProvider)
        {
            try
            {
                var quartz = new QuartzStartup(serviceProvider);
                quartz.Start().GetAwaiter().GetResult();

                Console.WriteLine("Press any key to close the application");
                Console.ReadKey();

                quartz.Stop();
            }
            catch (SchedulerException se)
            {
                Console.Error.WriteLine(se.ToString());
            }

            return serviceProvider;
        }

    }
}
