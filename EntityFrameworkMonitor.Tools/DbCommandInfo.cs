using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkMonitor.Tools
{
    public class DbCommandInfo
    {
        public DbCommandInfo()
        {

        }

        public DbCommandInfo(DbCommand dbCommand, bool isAsync, DateTimeOffset dateTimeOffset)
        {
            CommandText = dbCommand.CommandText ?? "<null>";
            IsAsync = isAsync;
            ExecutedAt = dateTimeOffset.ToString();

            DbParameterInfos = dbCommand.Parameters.Cast<DbParameter>().Select(dbp => new DbParameterInfo(dbp)).ToArray();
        }

        public string CommandText { get; set; }
        public bool IsAsync { get; set; }
        public string ExecutedAt { get; set; }

        public DbParameterInfo[] DbParameterInfos { get; set; }
    }
}
