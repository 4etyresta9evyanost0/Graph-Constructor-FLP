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
    public class SettingsViewModel : ViewModel
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

        // Text Vertex
        public Color VertexTextColor
        {
            get { return GetValue<Color>(); }
            set { SetValue(value); }
        }

        public double VertexTextSize
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public Color EdgeTextBackgroundColor
        {
            get { return GetValue<Color>(); }
            set { SetValue(value); }
        }

        // Text Edge
        public Color EdgeTextColor
        {
            get { return GetValue<Color>(); }
            set { SetValue(value); }
        }

        public double EdgeTextSize
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        // App
        public bool IsAskingForDelete
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool ShowBackgroundColor
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public Color CanvasBackgroundColor
        {
            get { return GetValue<Color>(); }
            set { SetValue(value); }
        }


        public SettingsViewModel()
        {
            VertexFillColor = Color.FromRgb(0xAB, 0xCB, 0x51); // #ABCB51
            VertexDiameter = 60;
            VertexStrokeColor = Colors.Black;
            VertexStrokeSize = 1;

            EdgeStrokeColor = Colors.Black;
            EdgeStrokeSize = 2.5;


            VertexTextColor = Colors.White;
            VertexTextSize = 10;

            EdgeTextColor = Colors.White;
            EdgeTextSize = 16;
            EdgeTextBackgroundColor = Colors.Black;

            IsAskingForDelete = true;

            CanvasBackgroundColor = Color.FromRgb(0xCC, 0xCC, 0xCC);

            SelectOnlyEdges = true;
            SelectOnlyVerts = true;

        }

        // в MainWindow

        public bool SelectOnlyEdges
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool SelectOnlyVerts
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }
    }
}
