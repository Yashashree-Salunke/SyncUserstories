using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Models
{
    public class JobSyncTracker
    {
        public int Id { get; set; }
        public int JobTypeId { get; set; }
        public virtual JobType JobType { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
