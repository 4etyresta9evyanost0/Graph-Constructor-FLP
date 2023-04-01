using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.AvalonDock.Properties;

namespace Graph_Constructor_FLP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public bool IsDebug { get; private set; }
        public App()
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var args = e.Args;
            IsDebug = args.Contains("-debug");
        }
    }

    
}
