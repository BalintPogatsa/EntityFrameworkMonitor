using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityFrameworkMonitor.Tools
{
    public enum DbResult
    { 
        Failed,
        Canceled,
        Completed
    }

    public class DbCommandResultInfo
    {
        public DbResult DbResult { get; set; }
        public long ElapsedTime { get; set; }
        public string Result { get; set; }
        public string ExceptionMessage { get; set; }
        public DbCommandInfo DbCommandInfo { get; set; }

        public string CallStack { get; set; }
    }
}
