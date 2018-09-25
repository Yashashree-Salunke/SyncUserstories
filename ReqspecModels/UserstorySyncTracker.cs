using System;

namespace ReqspecModels
{
    public class UserstorySyncTracker
    {
        public int Id { get; set; }
        public int UserstorySyncActionTypeId { get; set; }
        public virtual UserstorySyncActionType UserstorySyncActionType { get; set; }
        public int UserstoryId { get; set; }

        public DateTime CreatedOn { get; set; }

        public int TenantId { get; set; }
    }
}
