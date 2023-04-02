using Graph_Constructor_FLP.ViewModel;
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

namespace Graph_Constructor_FLP.Windows
{
    /// <summary>
    /// Логика взаимодействия для DEBUG_Window.xaml
    /// </summary>
    public partial class DEBUG_Window : Window
    {
        ApplicationViewModel AppVm => ViewModelController.ApplicationViewModel;
        ObjectsViewModel ObjVm => ViewModelController.ObjectsViewModel;
        SettingsViewModel Settings => ViewModelController.SettingsViewModel;

        public DEBUG_Window()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        #region Debug Methods
        private void Debug_Click(object sender, RoutedEventArgs e)
        {
            ObjVm.CanvasObjects.Add(
                new Vertex(25d, 25d, 75d, 75d, "Debug_Vert")
                );
        }

        private void Debug_Click_2(object sender, RoutedEventArgs e)
        {
            ObjVm.CanvasObjects.Insert(ObjVm.Edges.Count,
                new Edge(19d, 50d, 200d, 200d)
                );
        }

        private void Debug_Click_3(object sender, RoutedEventArgs e)
        {
            (ObjVm.CanvasObjects[0] as Vertex).FillColor = Colors.Black;
        }

        private void Debug_Click_4(object sender, RoutedEventArgs e)
        {
            var rnd = new Random();
            ObjVm.Vertices[0].Value = rnd.Next(1024);
        }

        #endregion
    }
}
