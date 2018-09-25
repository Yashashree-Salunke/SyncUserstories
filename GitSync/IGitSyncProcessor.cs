using System;
using System.Collections.Generic;
using System.Text;

namespace GitSync
{
    public interface IGitSyncProcessor
    {
        void Execute(GitSyncContext context);
    }
}
