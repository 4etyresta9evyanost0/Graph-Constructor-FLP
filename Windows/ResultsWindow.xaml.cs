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
using System.Windows.Shapes;
using Graph_Constructor_FLP;
using Graph_Constructor_FLP.ViewModel;

namespace Graph_Constructor_FLP.Windows
{
    public partial class ResultsWindow : Window
    {

        public ResultsWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
