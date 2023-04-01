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

        public bool IsAskingForDelete
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public SettingsViewModel()
        { 
            VertexFillColor = Color.FromRgb(0xAB, 0xCB, 0x51); // #ABCB51
            VertexDiameter = 60;
            VertexStrokeColor = Colors.Black;
            VertexStrokeSize = 1;

            EdgeStrokeColor = Colors.Black;
            EdgeStrokeSize = 1;

            IsAskingForDelete = true;
        }
    }
}
