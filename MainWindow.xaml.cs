using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MahApps.Metro.Controls;
using ControlzEx.Theming;
using DevExpress.Mvvm.Native;
using Graph_Constructor_FLP.ViewModel;

namespace Graph_Constructor_FLP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SettingsWindow _settingsWindow = new SettingsWindow();

        public MainWindow()
        {
            InitializeComponent();

            Closed += (x, ev) => {
                foreach (Window w in OwnedWindows)
                    w.Close();
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _settingsWindow.Show();
            _settingsWindow.Owner = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModelController.ApplicationViewModel;

        }
    }
}
