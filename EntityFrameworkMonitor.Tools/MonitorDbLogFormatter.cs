using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EntityFrameworkMonitor.Tools
{
    public class MonitorDbLogFormatter : DatabaseLogFormatter
    {
        public MonitorDbLogFormatter(DbContext context, Action<string> writeAction)
            : base(context, writeAction)
        {
        }

        public override void LogCommand<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            Check.NotNull<DbCommand>(command, "command");
            Check.NotNull<DbCommandInterceptionContext<TResult>>(interceptionContext, "interceptionContext");
            
            var dbCommandInfo = new DbCommandInfo(command, interceptionContext.IsAsync, DateTimeOffset.Now);

            var logText = SerializeObject(dbCommandInfo);
            this.Write(logText);
        }

        public override void LogResult<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            Check.NotNull<DbCommand>(command, "command");
            Check.NotNull<DbCommandInterceptionContext<TResult>>(interceptionContext, "interceptionContext");

            var dbCommandResultInfo = new DbCommandResultInfo();

            if (interceptionContext.Exception != null)
            {
                dbCommandResultInfo.DbResult = DbResult.Failed;
                dbCommandResultInfo.ElapsedTime = this.Stopwatch.ElapsedMilliseconds;
                dbCommandResultInfo.ExceptionMessage = interceptionContext.Exception.Message;
            }
            else if (interceptionContext.TaskStatus.HasFlag(TaskStatus.Canceled))
            {
                dbCommandResultInfo.DbResult = DbResult.Canceled;
                dbCommandResultInfo.ElapsedTime = this.Stopwatch.ElapsedMilliseconds;
            }
            else
            {
                TResult result = interceptionContext.Result;
                string p = (result == null) ? "null" : ((result is DbDataReader) ? result.GetType().Name : result.ToString());
                dbCommandResultInfo.DbResult = DbResult.Completed;
                dbCommandResultInfo.ElapsedTime = this.Stopwatch.ElapsedMilliseconds;
                dbCommandResultInfo.Result = p;
            }

            dbCommandResultInfo.DbCommandInfo = new DbCommandInfo(command, interceptionContext.IsAsync, DateTimeOffset.Now);
            
            //TODO: limit stack length (to user calls maybe)
            //dbCommandResultInfo.CallStack = Environment.StackTrace;
            var logText = SerializeObject(dbCommandResultInfo);
            this.Write(logText);
        }

        public static string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }
}
