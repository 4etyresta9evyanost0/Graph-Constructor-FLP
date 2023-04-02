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
using Graph_Constructor_FLP.Windows;
using MaterialDesignThemes.Wpf;

namespace Graph_Constructor_FLP.ViewModel
{
    public enum CanvasAction
    {
        Adding,
        Moving,
        Connecting,
        Deleting
    }

    public class ApplicationViewModel : ViewModel
    {
        #region Fields

        #endregion

        #region Properties
        // Vm's
        public ObjectsViewModel ObjectsVm => ViewModelController.ObjectsViewModel;
        public SettingsViewModel Settings => ViewModelController.SettingsViewModel;
        public Point MousePosWnd 
        { 
            get => GetValue<Point>(); 
            set => SetValue(value); 
        }
        public Point MousePosCanv 
        {
            get => GetValue<Point>();
            set => SetValue(value);
        }
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

    public class ObjectsViewModel : ViewModel
    {
        #region Fields
        ObservableCollection<CanvasObj> _canvasObjects = new();
        //Dictionary<int,string> _stringObjects = new();
        protected internal int _counterVerts = 1;
        protected internal int _counterEdges = 1;
        #endregion

        #region Properties
        public ObservableCollection<CanvasObj> CanvasObjects => _canvasObjects;
        public ObservableCollection<CanvasObj> VerticesNames => _canvasObjects;
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


                switch (ev.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        {
                            foreach (var item in ev.NewItems)
                            {
                                if (item is Vertex v)
                                    v.Name ??= $"X{_counterVerts++}";
                                if (item is Edge e)
                                    e.Name ??= $"U{_counterEdges++}";
                            }
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        break;
                    default:
                        break;
                }

                //foreach (var item in CanvasObjects)
                //    if (item is Vertex vertex)
                //        vertex.RaisePropertiesChanged();
                //    else if (item is Edge edge)
                //        edge.Vertices.RaisePropertiesChanged();
            };
        }
    }

    [POCOViewModel()]
    public abstract class CanvasObj : BindableBase
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
            set
            {
                SetValue(value);
                RaisePropertyChanged(nameof(Center));
            }
        }
        public double Y
        {
            get => GetValue<double>();
            set
            {
                SetValue(value);
                RaisePropertyChanged(nameof(Center));
            }
        }
        public double? Width
        {
            get => GetValue<double?>();
            set
            {
                SetValue(value);
                RaisePropertyChanged(nameof(Center));
            }
        }
        public double? Height
        {
            get => GetValue<double?>();
            set
            {
                SetValue(value);
                RaisePropertyChanged(nameof(Center));
            }
        }
        public Color? StrokeColor// => Settings.EdgeStrokeColor;
        {
            get => GetValue<Color?>();
            set => SetValue(value);
        }

        public double? StrokeThickness //=> Settings.EdgeStrokeSize;
        {
            get => GetValue<double?>();
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
        public virtual Point Begin
        {
            get => new(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public virtual Point End
        {
            get => new(Width ?? 0, Height ?? 0);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int Index { get; set; }

        public double? Value
        {
            get => GetValue<double?>();
            set => SetValue(value); 
        }

        public virtual Point Center
        {
            get { return new(X + (Width ?? 0) / 2, Y + (Height ?? 0) / 2); }
            set { X = value.X; Y = value.Y;}
        }

        #region Костыль
        public virtual Vertex VertBegin
        {
            get => GetValue<Vertex>();
            set => SetValue(value);
        }

        public virtual Vertex VertEnd
        {
            get => GetValue<Vertex>();
            set => SetValue(value);
        }
        #endregion

        public static ApplicationViewModel AppVm => ViewModelController.ApplicationViewModel;
        public static SettingsViewModel Settings => ViewModelController.SettingsViewModel;

        #endregion

        #region Methods

        public virtual void Init() 
        {
            Value = null;
        }

        #endregion

        public CanvasObj() : this(5, 5, 30, 30) { }

        public CanvasObj(double x, double y, double? width, double? height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public CanvasObj(double x, double y, Size size)
        {
            X = x;
            Y = y;
            Size = size;
        }

        public CanvasObj(Point begin, Point end)
        {
            Begin = begin;
            End = end;
        } 
    }

    public class Vertex : CanvasObj
    {
        #region Fields
        readonly ObservableCollection<Edge> _edges = new();
        #endregion

        #region Properties
        public ObservableCollection<Edge> Edges => _edges;

        public Color? FillColor
        {
            get => GetValue<Color?>();
            set => SetValue(value);
        }

        public override Point End
        {
            get => new(X + Settings.VertexDiameter, Y + Settings.VertexDiameter);
            //set
            //{
            //    Width = X - value.X;
            //    Height = Y - value.Y;
            //}
        }

        public Point Center
        {
            get { return new(X + Settings.VertexDiameter / 2 + 2, Y + Settings.VertexDiameter / 2 + 2); }
        }

        //public DelegateCommand<Color> FillColorChanged { get; private set; }
        public DelegateCommand<Vertex> ChangeCommand { get; private set; }
        public DelegateCommand SettingStandardFillColor { get; private set; }
        public DelegateCommand MoveOnTopCommand { get; private set; }
        public DelegateCommand MoveOnBottomCommand { get; private set; }

        //public double StrokeThickness => SettingsVm.VertexDiameter;
        //{
        //    get => GetValue<double>();
        //    set => SetValue(value);
        //}

        #endregion

        #region Methods

        public override void Init()
        {
            base.Init();
            //FillColorChanged = new DelegateCommand<Color>(FillColorChange);
            SettingStandardFillColor = new DelegateCommand(SetStandardFillColor);

            ChangeCommand = new DelegateCommand<Vertex>(ShowChangeWindow);
            MoveOnTopCommand = new DelegateCommand(MoveOnTop);
            MoveOnBottomCommand = new DelegateCommand(MoveOnBottom);
        }

        public void MoveOnTop()
        {
            var last = (Vertex)AppVm.ObjectsVm.CanvasObjects.Last(x => x is Vertex);
            var vertVmIndex = AppVm.ObjectsVm.CanvasObjects.IndexOf(this);
            var lastIndex = AppVm.ObjectsVm.CanvasObjects.IndexOf(last);
            AppVm.ObjectsVm.CanvasObjects.Swap(vertVmIndex, lastIndex);
        }

        public void MoveOnBottom()
        {
            var first = (Vertex)AppVm.ObjectsVm.CanvasObjects.First(x => x is Vertex);
            var vertVmIndex = AppVm.ObjectsVm.CanvasObjects.IndexOf(this);
            var firstIndex = AppVm.ObjectsVm.CanvasObjects.IndexOf(first);
            AppVm.ObjectsVm.CanvasObjects.Swap(vertVmIndex, firstIndex);
        }

        [Command]
        public void ShowChangeWindow(Vertex parameter)
        {
            ChangeVertexWindow wind = new(parameter);

            var n = parameter.Name;
            var v = parameter.Value;

            if (wind.ShowDialog() != true)
            {
                parameter.Name = n;
                parameter.Value = v;
            }
        }

        public void FillColorChange(Color color) => FillColor = color;
        public void SetStandardFillColor() => FillColor = null;
        #endregion

        public Vertex(double x, double y, double width, double height) : base(x, y, width, height) => Init();

        public Vertex(double x, double y, Size size) : base(x, y, size) => Init();

        public Vertex(Point begin, Point end) : base(begin, end) => Init();

        public Vertex() : base() => Init();


        public Vertex(double x, double y, double width, double height, string name) : this(x, y, width, height) => Name = name;

        public Vertex(double x, double y, Size size, string name) : this(x, y, size) => Name = name;

        public Vertex(Point begin, Point end, string name) : this(begin, end) => Name = name;

        public Vertex(string name) : this() => Name = name;
    }

    public class Edge : CanvasObj
    {
        #region Fields

        #endregion

        #region Properties

        public override Vertex VertBegin
        {
            get => GetValue<Vertex>();
            set => SetValue(value);
        }

        public override Vertex VertEnd
        {
            get => GetValue<Vertex>();
            set => SetValue(value);
        }

        public override Point End 
        {
            get => new(Width ?? 0, Height ?? 0);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }
        #endregion

        #region Methods
        public override void Init()
        {
            //_vertices.CollectionChanged += (x, ev) =>
            //{
            //    RaisePropertiesChanged(
            //        nameof(Vertices),
            //        nameof(Begin),
            //        nameof(End));
            //};
            //var b = VertEnd != null ? VertEnd.Center.Y : Height;
        }
        #endregion

        public Edge(double x1, double y1, double x2, double y2) : base(x1, y1, x2, y2) { Init(); }

        public Edge(double x, double y, Size size) : base(x, y, size) { Init(); }

        public Edge(Point begin, Point end) : base(begin, end) { Init(); }

        public Edge(Vertex begin, Vertex end)
        {
            Init();
            VertBegin = begin;
            VertEnd = end;
        }
    }
}
