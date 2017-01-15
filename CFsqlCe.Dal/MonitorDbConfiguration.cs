using EntityFrameworkMonitor.Tools;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CFSqlCe.Dal
{
    public class MonitorDbConfiguration : DbConfiguration
    {
        public MonitorDbConfiguration()
        {
            SetDatabaseLogFormatter(
                (context, writeAction) => new MonitorDbLogFormatter(context, writeAction));
        }
    }
}
