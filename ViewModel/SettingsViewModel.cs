using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Graph_Constructor_FLP.ViewModel
{
    class SettingsViewModel : ViewModel
    {
        // Vertex
        public Color VertexFillColor
        {
            get { return GetValue<Color>(); }
            set { SetValue(value); }
        }

        public double VertexDiameter
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public Color VertexStrokeColor
        {
            get { return GetValue<Color>(); }
            set { SetValue(value); }
        }

        public double VertexStrokeSize
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        // Edge
        public Color EdgeStrokeColor
        {
            get { return GetValue<Color>(); }
            set { SetValue(value); }
        }

        public double EdgeStrokeSize
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public SettingsViewModel()
        { 
            VertexFillColor = Colors.MidnightBlue;
            VertexDiameter = 75;
            VertexStrokeColor = Colors.Black;
            VertexStrokeSize = 2;

            EdgeStrokeColor = Colors.Black;
            EdgeStrokeSize = 2;
        }
    }
}
