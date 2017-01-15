using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkMonitor
{
    public class DbEventOverview
    {
        private string _processName;

        public string ProcessName
        {
            get { return _processName; }
            set { _processName = value; }
        }
        

        private string _eventTime;

        public string EventTime
        {
            get { return _eventTime; }
            set { _eventTime = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }


        public Tools.DbCommandInfo DbCommandInfo { get; set; }

        public string ErrorMessage { get; set; }

        public string Status { get; set; }

        public string Result { get; set; }
    }
}
