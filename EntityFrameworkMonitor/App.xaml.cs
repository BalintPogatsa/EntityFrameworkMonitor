using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EntityFrameworkMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create the startup window
            MainWindow wnd = new MainWindow();
            var vm = new MainWindowViewModel();
            DebugMonitor.OnOutputDebugString += new OnOutputDebugStringHandler(vm.DebugMonitor_OnOutputDebugString);
            wnd.DataContext = vm;

            // Show the window
            wnd.Show();

            Task.Factory.StartNew(() =>
                {
                    vm.ProcessQueue(_tokenSource);
                }, _tokenSource.Token);

            try
            {
                DebugMonitor.Start();
            }
            catch (ApplicationException ex)
            {

                MessageBox.Show(ex.Message, "Failed to start DebugMonitor", MessageBoxButton.OK);
                return;
            }

        }

        protected override void OnExit(ExitEventArgs e)
        {
            DebugMonitor.Stop();
            _tokenSource.Cancel();
            base.OnExit(e);
        }
    }
}
