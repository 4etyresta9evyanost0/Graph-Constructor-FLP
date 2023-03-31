using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DevExpress.Mvvm.ModuleInjection.Native;
using System.Windows.Media.Media3D;
using DevExpress.Mvvm.POCO;

namespace Graph_Constructor_FLP.ViewModel
{
    class ApplicationViewModel : ViewModel
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion
        public ApplicationViewModel() { }

    }

    class ObjectsViewModel : ViewModel
    {
        #region Fields
        readonly ObservableCollection<CanvasObj> _canvasObjects = new();
        #endregion

        #region Properties
        public ObservableCollection<CanvasObj> CanvasObjects => _canvasObjects;
        public ObservableCollection<Vertex> Vertices => CanvasObjects
                .Where(x => x as Vertex != null)
                .Cast<Vertex>()
                .ToObservableCollection();

        public ObservableCollection<Edge> Edges => CanvasObjects
                .Where(x => x as Edge != null)
                .Cast<Edge>()
                .ToObservableCollection();

        #endregion

        #region Methods

        #endregion
        public ObjectsViewModel() {
            _canvasObjects.CollectionChanged += (x, ev) =>
            {
                RaisePropertiesChanged(
                    nameof(CanvasObjects),
                    nameof(Vertices),
                    nameof(Edges)
                );

                foreach (var item in CanvasObjects)
                    if (item is Vertex vertex)
                        vertex.Edges.RaisePropertiesChanged();
                    else if (item is Edge edge)
                        edge.Vertices.RaisePropertiesChanged();

            };
        }
    }

    class CanvasObj : BindableBase
    {
        #region Fields
        //double _x = 0;
        //double _y = 0;
        //double _width = 0;
        //double _height = 0;
        //System.Drawing.Color _color = System.Drawing.Color.Transparent;
        #endregion

        #region Properties
        public double X
        {
            get => GetValue<double>();
            set => SetValue(value);
        }
        public double Y
        {
            get => GetValue<double>();
            set => SetValue(value);
        }
        public double Width
        {
            get => GetValue<double>();
            set => SetValue(value);
        }
        public double Height
        {
            get => GetValue<double>(); 
            set => SetValue(value);
        }
        public Color StrokeColor
        {
            get => GetValue<Color>();
            set => SetValue(value);
        }

        public double StrokeSize
        {
            get => GetValue<double>();
            set => SetValue(value);
        }

        public Size Size
        {
            get => new(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }
        public Point Begin
        {
            get => new(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public Point End
        {
            get => new(X + Width, Y + Height);
            set
            {
                Width = X - value.X;
                Height = Y - value.Y;
            }
        }

        public double Value
        {
            get => GetValue<double>();
            set => SetValue(value); 
        }


        #endregion

        #region Methods

        #endregion

        public CanvasObj() 
        { 
            StrokeColor = Colors.Black; 
            StrokeSize = 1; 
        }
        public CanvasObj(double x, double y, double width, double height) : base()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public CanvasObj(double x, double y, Size size) : base()
        {
            X = x;
            Y = y;
            Size = size;
        }

        public CanvasObj(Point begin, Point end) : base()
        {
            Begin = begin;
            End = end;
        } 
    }

    class Vertex : CanvasObj
    {
        #region Fields
        readonly ObservableCollection<Edge> _edges = new();
        #endregion

        #region Properties
        public ObservableCollection<Edge> Edges => _edges;
        #endregion

        #region Methods

        #endregion
        public Vertex() : base()
        {

        }

        public Vertex(double x, double y, double width, double height) : base(x, y, width, height) { }

        public Vertex(double x, double y, Size size) : base(x, y, size) { }

        public Vertex(Point begin, Point end) : base(begin, end) { }

    }

    class Edge : CanvasObj
    {
        #region Fields
        readonly ObservableCollection<Vertex> _vertices = new();
        #endregion

        #region Properties
        public ObservableCollection<Vertex> Vertices => _vertices;
        #endregion

        #region Methods

        #endregion

        public Edge() : base()
        {

        }

        public Edge(double x, double y, double width, double height) : base(x, y, width, height) { }

        public Edge(double x, double y, Size size) : base(x, y, size) { }

        public Edge(Point begin, Point end) : base(begin, end) { }
    }
}
