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

using DevExpress.Mvvm.Native;

namespace Graph_Constructor_FLP.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChangeVertexWindow.xaml
    /// </summary>
    public partial class ChangeVertexWindow : Window
    {

        public Vertex Vertex
        {
            get { return (Vertex)GetValue(VertexProperty); }
            set { SetValue(VertexProperty, value); }
        }

        public static readonly DependencyProperty VertexProperty =
            DependencyProperty.Register("Vertex", typeof(Vertex), typeof(ChangeVertexWindow), new PropertyMetadata(null));

        public ChangeVertexWindow(Vertex vertex)
        {
            Vertex = vertex;
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
