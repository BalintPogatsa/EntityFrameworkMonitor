using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkMonitor.Tools
{
    public class DbParameterInfo
    {
        public DbParameterInfo()
        {

        }

        public DbParameterInfo(System.Data.Common.DbParameter dbp)
        {
            DbType = dbp.DbType;
            Direction = dbp.Direction;
            IsNullable = dbp.IsNullable;
            ParameterName = dbp.ParameterName;
            Size = dbp.Size;
            Value = dbp.Value;
        }
        public DbType DbType { get; set; }
        
        public ParameterDirection Direction { get; set; }
        
        public bool IsNullable { get; set; }
        
        public string ParameterName { get; set; }
        
        public int Size { get; set; }
        
        public object Value { get; set; }
    }
}
