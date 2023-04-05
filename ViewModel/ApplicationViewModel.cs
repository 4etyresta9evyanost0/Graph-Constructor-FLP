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


                // Function to perform DFS traversal on the graph on a graph
        void DFS(Vertex v, List<Vertex> visited)
        {
            // mark current node as visited
            visited.Add(v);
 
            // do for every edge begin
            for (int i = 0; i < v.EdgesBegin.Count; i++)
            {
                if (!visited.Contains(v.EdgesBegin[i].VertEnd))
                {
                    DFS(v.EdgesBegin[i].VertEnd, visited);
                }
            }
            // do for every edge end
            for (int i = 0; i < v.EdgesEnd.Count; i++)
            {
                if (!visited.Contains(v.EdgesEnd[i].VertBegin))
                {
                    DFS(v.EdgesEnd[i].VertBegin, visited);
                }
            }
        }

        public bool IsAllConnected
        {
            get
            {
                if (Edges.Count < 1)
                {
                    return false;
                }
                var visted = new List<Vertex>(Vertices.Count);
                //var toCheckEdges = new List<Edge>(Edges);

                DFS(Vertices[0], visted);

                return visted.Count == Vertices.Count;
            }
        }

        #endregion

        #region Methods
        public void AddObj(CanvasObj canvasObj) => _canvasObjects.Add(canvasObj);
        #endregion
        public ObjectsViewModel()
        {
            _canvasObjects.CollectionChanged += (x, ev) =>
            {
                var collection = x as ObservableCollection<CanvasObj>;
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
                        {
                            foreach (var item in ev.OldItems)
                            {
                                if (item is Vertex v)
                                {
                                    ;//v.Name ??= $"X{_counterVerts++}";
                                }
                                if (item is Edge e)
                                    ;// e.Name ??= $"U{_counterEdges++}";
                            }
                        }
                        break;
                    default:
                        break;

                }

                for (int i = 0; i < Vertices.Count; i++)
                    Vertices[i].Index = i;
                for (int i = 0; i < Edges.Count; i++)
                    Edges[i].Index = i;

                //foreach (var item in CanvasObjects)
                //    if (item is Vertex vertex)
                //        vertex.RaisePropertiesChanged();
                //    else if (item is Edge edge)
                //        edge.Vertices.RaisePropertiesChanged();
            };
            Vertex v1;
            Vertex v2;
            Vertex v3;
            Vertex v4;
            Vertex v5;
            Vertex v6;
            Vertex v7;
            Vertex v8;
            _canvasObjects.Add(v1 = new Vertex(201, 34, 0, 0));  _canvasObjects.Last().Value = 500;
            _canvasObjects.Add(v2 = new Vertex(308, 40, 0, 0)); _canvasObjects.Last().Value = 300;
            _canvasObjects.Add(v3 = new Vertex(451, 113, 0, 0));  _canvasObjects.Last().Value = 120;
            _canvasObjects.Add(v4 = new Vertex(457, 210, 0, 0)); _canvasObjects.Last().Value = 700;
            _canvasObjects.Add(v5 = new Vertex(309, 301, 0, 0));  _canvasObjects.Last().Value = 600;
            _canvasObjects.Add(v6 = new Vertex(188, 289, 0, 0)); _canvasObjects.Last().Value = 620;
            _canvasObjects.Add(v7 = new Vertex(59, 232, 0, 0));  _canvasObjects.Last().Value = 450;
            _canvasObjects.Add(v8 = new Vertex(82, 107, 0, 0)); _canvasObjects.Last().Value = 220;
            _canvasObjects.Insert(0, new Edge(v1, v2)); _canvasObjects[0].Value = 2;
            _canvasObjects.Insert(0, new Edge(v1, v3)); _canvasObjects[0].Value = 1;
            _canvasObjects.Insert(0, new Edge(v1, v6)); _canvasObjects[0].Value = 7;
            _canvasObjects.Insert(0, new Edge(v1, v7)); _canvasObjects[0].Value = 4;
            _canvasObjects.Insert(0, new Edge(v1, v8)); _canvasObjects[0].Value = 2;

            _canvasObjects.Insert(0, new Edge(v2, v3)); _canvasObjects[0].Value = 3;
            _canvasObjects.Insert(0, new Edge(v2, v5)); _canvasObjects[0].Value = 9;

            _canvasObjects.Insert(0, new Edge(v3, v4)); _canvasObjects[0].Value = 7;
            _canvasObjects.Insert(0, new Edge(v3, v7)); _canvasObjects[0].Value = 4;

            _canvasObjects.Insert(0, new Edge(v4, v5)); _canvasObjects[0].Value = 1;
            _canvasObjects.Insert(0, new Edge(v4, v6)); _canvasObjects[0].Value = 8;

            _canvasObjects.Insert(0, new Edge(v5, v6)); _canvasObjects[0].Value = 7;

            _canvasObjects.Insert(0, new Edge(v6, v7)); _canvasObjects[0].Value = 3;
            _canvasObjects.Insert(0, new Edge(v6, v8)); _canvasObjects[0].Value = 3;

            _canvasObjects.Insert(0, new Edge(v7, v8)); _canvasObjects[0].Value = 1;

            //double[] vWeights = { 500, 300, 120, 700, 600, 620, 450, 220 };

            //int[,] ints =
            //{
            //    { 0, 2, 1, 0, 0, 7, 4, 2 },
            //    { 2, 0, 3, 0, 9, 0, 0, 0 },
            //    { 1, 3, 0, 7, 0, 0, 4, 0 },
            //    { 0, 0, 7, 0, 1, 8, 0, 0 },
            //    { 0, 9, 0, 1, 0, 7, 0, 0 },
            //    { 7, 0, 0, 8, 7, 0, 3, 3 },
            //    { 4, 0, 4, 0, 0, 3, 0, 1 },
            //    { 2, 0, 0, 0, 0, 3, 1, 0 }
            //};
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
            set { X = value.X; Y = value.Y; }
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
        public static ObjectsViewModel ObjVm => ViewModelController.ObjectsViewModel;
        public static SettingsViewModel Settings => ViewModelController.SettingsViewModel;

        #endregion

        #region Methods

        public virtual void Init()
        {
            Value = null;
        }

        public virtual void Remove()
        {
            ObjVm.CanvasObjects.Remove(this);
            //v.EdgesBegin.ForEach(edge => _canvasObjects.Remove(edge));
            //v.EdgesEnd.ForEach(edge => _canvasObjects.Remove(edge));
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
        ObservableCollection<Edge> _edgesBegin = new();
        ObservableCollection<Edge> _edgesEnd = new();
        #endregion

        #region Properties
        public ObservableCollection<Edge> EdgesBegin => _edgesBegin;
        public ObservableCollection<Edge> EdgesEnd => _edgesEnd;

        public ObservableCollection<Edge> Edges => EdgesBegin.Concat(EdgesEnd).ToObservableCollection();

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
        public override void Remove()
        {
            this.Edges.ForEach(x => ObjVm.CanvasObjects.Remove(x));
            base.Remove();
        }
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
            set
            {
                SetValue(value);
                value.EdgesBegin.Add(this);
            }
        }

        public override Vertex VertEnd
        {
            get => GetValue<Vertex>();
            set
            {
                SetValue(value);
                value.EdgesEnd.Add(this);
            }
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
        public override void Remove()
        {
            base.Remove();
            this.VertBegin.EdgesBegin.Remove(this);
            this.VertEnd.EdgesEnd.Remove(this);
        }
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
