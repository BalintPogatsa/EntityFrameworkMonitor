using EntityFrameworkMonitor.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace EntityFrameworkMonitor
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        private static readonly int MAX_QUEUE_SIZE = 200;
        private bool queueSizeWarningFlag = false;

        // Construct a ConcurrentQueue.
        private ConcurrentQueue<Tuple<int, string>> messageQueue = new ConcurrentQueue<Tuple<int, string>>();

        private ObservableCollection<DbEventOverview> _dbEventOverviews = new ObservableCollection<DbEventOverview>();

        public ObservableCollection<DbEventOverview> DbEventOverviews
        {
            get { return _dbEventOverviews; }
            set 
            {
                _dbEventOverviews = value;
                OnNotifyPropertyChanged();
            }
        }

        private DbEventOverview _selectedDbEventOverview;

        public DbEventOverview SelectedDbEventOverview
        {
            get { return _selectedDbEventOverview; }
            set 
            { 
                _selectedDbEventOverview = value;
                OnNotifyPropertyChanged();
            }
        }

        private ObservableDictionary<string, int> _queryCounts = new ObservableDictionary<string,int>();

        public ObservableDictionary<string, int> QueryCounts
        {
            get { return _queryCounts; }
            set 
            { 
                _queryCounts = value;
                OnNotifyPropertyChanged();
            }
        }


        internal void DebugMonitor_OnOutputDebugString(int pid, string text)
        {
            if (text.StartsWith("<?xml"))
            {
                if (messageQueue.Count > MAX_QUEUE_SIZE)
                {
                    queueSizeWarningFlag = true;
                    return;
                }

                messageQueue.Enqueue(new Tuple<int, string>(pid, text));
            }
        }

        private string GetProcessName(int pid)
        {
            if (pid == -1)
                return Process.GetCurrentProcess().ProcessName;
            try
            {
                return Process.GetProcessById(pid).ProcessName;
            }
            catch
            {
                return "<exited>";
            }
        }

        internal void ProcessQueue(CancellationTokenSource cancellationTokenSource)
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                Tuple<int, string> queueItem;
                if (messageQueue.TryDequeue(out queueItem))
                {
                    var pid = queueItem.Item1;
                    var text = queueItem.Item2;
                    if (text.Contains("DbCommandResultInfo"))
                    {
                        var dbCommandResultInfo = DeserializeXml<DbCommandResultInfo>(text);

                        Application.Current.Dispatcher.BeginInvoke(
                              DispatcherPriority.Background,
                              new Action(() =>
                              {
                                  DbEventOverviews.Add(new DbEventOverview()
                                  {
                                      ProcessName = GetProcessName(pid),
                                      EventTime = dbCommandResultInfo.DbCommandInfo.ExecutedAt,
                                      Description = dbCommandResultInfo.DbCommandInfo.CommandText.Replace(Environment.NewLine, " ").Replace("\r", " ").Replace("\n", " "), 
                                      DbCommandInfo = dbCommandResultInfo.DbCommandInfo,
                                      ErrorMessage = dbCommandResultInfo.ExceptionMessage,
                                      Status = dbCommandResultInfo.DbResult.ToString(),
                                      Result = dbCommandResultInfo.Result
                                  });

                                  //TODO:
                                  //if (QueryCounts.ContainsKey(dbCommandResultInfo.CallStack))
                                  //{
                                  //    QueryCounts[dbCommandResultInfo.CallStack]++;
                                  //}
                                  //else
                                  //    QueryCounts.Add(dbCommandResultInfo.CallStack, 1);
                              }));
                    }
                    else if (text.Contains("DbCommandInfo"))
                    {
                        var dbCommandInfo = DeserializeXml<DbCommandInfo>(text);

                        Application.Current.Dispatcher.BeginInvoke(
                              DispatcherPriority.Background,
                              new Action(() =>
                              {
                                  DbEventOverviews.Add(new DbEventOverview()
                                  {
                                      ProcessName = GetProcessName(pid),
                                      EventTime = dbCommandInfo.ExecutedAt,
                                      Description = dbCommandInfo.CommandText.Replace(Environment.NewLine, " ").Replace("\r", " ").Replace("\n", " "),
                                      DbCommandInfo = dbCommandInfo,
                                      Status = "Starting"
                                  });
                              }));
                    }
                }

                Thread.Sleep(100);
            }
        }

        private T DeserializeXml<T>(string text)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            T result;
            using (TextReader reader = new StringReader(text))
            {
                result = (T)ser.Deserialize(reader);
                return result;
            }
        }
    }
}
