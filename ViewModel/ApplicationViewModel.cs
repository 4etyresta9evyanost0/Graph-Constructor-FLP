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
using DevExpress.Mvvm.ModuleInjection.Native;
using System.Windows.Media.Media3D;
using DevExpress.Mvvm.DataAnnotations;
using System.Windows.Data;
using DevExpress.Xpf.DXBinding;

namespace Graph_Constructor_FLP.ViewModel
{
    enum CanvasAction
    {
        Adding,
        Moving,
        Connecting,
        Deleting
    }

    class ApplicationViewModel : ViewModel
    {
        #region Fields

        #endregion

        #region Properties
        // Vm's
        public ObjectsViewModel ObjectsVm => ViewModelController.ObjectsViewModel;
        public SettingsViewModel Settings => ViewModelController.SettingsViewModel;

        public bool IsDebug => ((App)Application.Current).IsDebug;

        public CanvasAction CanvasAction
        {
            get => GetValue<CanvasAction>();
            private set => SetValue(value);
        }
        public DelegateCommand<CanvasAction> CanvasActionChangeCommand { get; private set; }



        #endregion

        #region Methods

        #endregion
        public ApplicationViewModel() 
        {
            CanvasAction = CanvasAction.Moving;
            CanvasActionChangeCommand = new DelegateCommand<CanvasAction>(ChangeCanvasAction);
        }

        public void ChangeCanvasAction(CanvasAction action) => CanvasAction = action;


    }

    class ObjectsViewModel : ViewModel
    {
        #region Fields
        ObservableCollection<CanvasObj> _canvasObjects = new();
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
        public void AddObj(CanvasObj canvasObj) => _canvasObjects.Add(canvasObj);
        #endregion
        public ObjectsViewModel() {
            _canvasObjects.CollectionChanged += (x, ev) =>
            {
                RaisePropertiesChanged(
                    nameof(CanvasObjects),
                    nameof(Vertices),
                    nameof(Edges)
                );

                //foreach (var item in CanvasObjects)
                //    if (item is Vertex vertex)
                //        vertex.RaisePropertiesChanged();
                //    else if (item is Edge edge)
                //        edge.Vertices.RaisePropertiesChanged();
            };
        }
    }

    [POCOViewModel()]
    abstract class CanvasObj : BindableBase
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
        public double? Width
        {
            get => GetValue<double>();
            set => SetValue(value);
        }
        public double? Height
        {
            get => GetValue<double>(); 
            set => SetValue(value);
        }
        public Color? StrokeColor// => Settings.EdgeStrokeColor;
        {
            get => GetValue<Color>();
            set => SetValue(value);
        }

        public double? StrokeThickness //=> Settings.EdgeStrokeSize;
        {
            get => GetValue<double>();
            set => SetValue(value);
        }

        public Size Size
        {
            get => new(Width ?? 0, Height ?? 0);
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
            get => new(X + Width ?? 0, Y + Height ?? 0);
            set
            {
                Width = X - value.X;
                Height = Y - value.Y;
            }
        }

        public double? Value
        {
            get => GetValue<double>();
            set => SetValue(value); 
        }

        public UIElement UIElement
        {
            get => GetValue<UIElement>();
            set => SetValue(value);
        }

        public static ApplicationViewModel AppVm => ViewModelController.ApplicationViewModel;
        public static SettingsViewModel Settings => ViewModelController.SettingsViewModel;

        #endregion

        #region Methods

        #endregion

        public CanvasObj() 
        {
            //StrokeColor = Colors.Bisque;
            //StrokeThickness = 1; 
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

        public Color? FillColor
        {
            get => GetValue<Color>();
            set => SetValue(value);
        }

        //public double StrokeThickness => SettingsVm.VertexDiameter;
        //{
        //    get => GetValue<double>();
        //    set => SetValue(value);
        //}

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
        public (Vertex begin, Vertex end) Vertices => (_vertices[0], _vertices[1]);

        #endregion

        #region Methods

        #endregion

        public Edge() : base()
        {
            _vertices.CollectionChanged += (x, ev) =>
            {
                RaisePropertyChanged(() => Vertices);
            };
        }

        public Edge(double x1, double y1, double x2, double y2) : base(x1, y1, x2, y2) { }

        public Edge(double x, double y, Size size) : base(x, y, size) { }

        public Edge(Point begin, Point end) : base(begin, end) { }

        public Edge(Vertex begin, Vertex end)
        {
            _vertices[0] = begin;
            _vertices[1] = end;
        }
    }
}
