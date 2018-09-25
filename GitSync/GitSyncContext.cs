using ReqspecModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GitSync
{
    public class GitSyncContext
    {
        public string RepositoryUrl { get; set; }
        public string AccessToken { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string SourceConnectionString { get; set; }
        public List<UserstorySyncTracker> Records { get; set; }
    }
}
