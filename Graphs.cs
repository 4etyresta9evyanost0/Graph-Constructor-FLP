using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Threading;
using System.Collections;

using System.Windows.Controls;

namespace Graphs
{
    public class Vertex
    {
        public string Name { get; set; }
        public int Index { get; set; }

        //public Point Location { get; set; }

        public Vertex(string name, int index)
        {
            Name = name;
            Index = index;
        }

        public double? Weight { get; set; }

        public Graph Graph { get; protected internal set; }

        public System.Windows.Controls.Grid Grid { get; set; }
        public System.Windows.Shapes.Ellipse Ellipse { get; set; }

        public Edge[] Begins
        {
            get
            {
                return Graph.Edges.FindAll((x) => x.Begin == this).ToArray();
            }
        }

        public Edge[] Ends
        {
            get
            {
                return Graph.Edges.FindAll((x) => x.End == this).ToArray();
            }
        }


        public Edge[] Edges
        {
            get
            {
                var arr = new HashSet<Edge>(Begins.Concat(Ends));
                //var dist = arr.Distinct().ToArray();
                //arr.ExceptWith(dist);
                return arr.ToArray();
            }
        }

        //public Edge[] Edges
        //{
        //    get => Graph.Edges.FindAll((x) => x.Begin == this || x.End == this).ToArray();
        //}

        public int[] AdjacencyRow
        {

            get
            {
                int[] row = new int[Graph.Vertices.Count];

                Begins.ToList().ForEach((x) => row[x.End.Index] = 1);
                Ends.ToList().ForEach((x) => {
                    if (!x.HasDirection)
                        row[x.Begin.Index] = 1;
                });

                return row;
                //int[] row = new int[Graph.Vertices.Count];
                //for (int i = 0; i < Graph.Vertices.Count; i++)
                //{
                //    if (Graph.Vertices[i].Begins.ToList(). )
                //    {

                //        row[Graph.Edges[i].End.Index] = 1;
                //    }
                //}
                //return row;
            }
        }

        public override string ToString()
        {
            return $"Vertex {Name}: {Index}";
        }
    }

    public class Edge : IComparable
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public bool HasDirection { get; set; }
        public bool HasWeight { get { return Weight != null; } }
        public double? Weight { get; set; }

        public bool isLoop
        {
            get { return Begin == End; }
        }

        public Vertex Begin { get; set; }
        public Vertex End { get; set; }

        public Vertex GetOpposite(Vertex v)
        {
            if (v == Begin)
                return End;
            else if (v == End)
                return Begin;
            else
                return null;

        }

        public Edge(string name, int index, bool hasDirection)
        {
            Name = name;
            Index = index;
            HasDirection = hasDirection;
        }

        public Edge(string name, int index, bool hasDirection, Vertex oneVertex)
        {
            Name = name;
            Index = index;
            HasDirection = hasDirection;
            Begin = oneVertex;
            End = oneVertex;
        }

        public Edge(string name, int index, bool hasDirection, Vertex begin, Vertex end)
        {
            Name = name;
            Index = index;
            HasDirection = hasDirection;
            Begin = begin;
            End = end;
        }

        public Edge(string name, int index, bool hasDirection, Vertex begin, Vertex end, double? weight)
        {
            Name = name;
            Index = index;
            HasDirection = hasDirection;
            Begin = begin;
            End = end;
            Weight = weight;
        }

        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }
            //if (value is Edge num)
            if ((Edge)value != null)
            {
                return CompareTo((Edge)value);
            }
            throw new ArgumentException("Must be edge");
        }

        public int CompareTo(Edge value)
        {
            if (value == null)
            {
                return 1;
            }
            if (Weight < value.Weight || (!HasWeight && value.HasWeight))
            {
                return -1;
            }
            if (Weight > value.Weight || (HasWeight && !value.HasWeight))
            {
                return 1;
            }
            return 0;
        }
        public override string ToString()
        {
            return $"Edge {Name}: {Index}, {Weight}";
        }

        public Graph Graph { get; protected internal set; }
    }

    public enum GraphMatrix { Adjacency = 0, Incidence = 1 }
    public enum GraphType { Empty = 0, Undirected = 1, Directed = 2, Mixed = 3 }

    public class Graph
    {
        public double AllWeights
        {
            get
            {
                double sum = 0;
                if (Edges.Any((x) => !x.HasWeight))
                    return -1;
                Edges.ForEach((x) => sum += x.Weight ?? 0);
                return sum;
            }
        }
        public List<Vertex> Vertices { get; set; } = new List<Vertex>();
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public GraphType Type
        {
            get
            {
                if (Edges.Count == 0 || Vertices.Count == 0)
                    return GraphType.Empty;

                GraphType gt = (Edges[0].HasDirection ? GraphType.Directed : GraphType.Undirected);
                for (int i = 1; i < Edges.Count; i++)
                {
                    if (gt != (Edges[i].HasDirection ? GraphType.Directed : GraphType.Undirected))
                        return GraphType.Mixed;
                }
                return gt;
            }
        }

        public Graph() { }

        public Graph(Edge[] edges, Vertex[] vertices)
        {
            Edges = edges.ToList();
            Vertices = vertices.ToList();

            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i].Graph = this;
                Vertices[i].Index = i;
            }

            for (int i = 0; i < Edges.Count; i++)
            {
                Edges[i].Graph = this;
                Edges[i].Index = i;
            }
        }

        public Graph(double?[,] mat, GraphType type) : base()
        {
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                Vertex e = new Vertex($"X{i + 1}", i);
                Vertices.Add(e);
            }
            if (mat.GetLength(0) == mat.GetLength(1))
            {   // По смежной матрице

                int c = 0;
                switch (type)
                {
                    case GraphType.Directed:
                        for (int i = 0; i < mat.GetLength(0); i++)
                        {
                            for (int j = 0; j < mat.GetLength(1); j++)
                            {
                                if (mat[i, j] != 0)
                                    Edges.Add(new Edge($"U{++c}", c, true, Vertices[i], Vertices[j], mat[i, j]));
                            }
                        }
                        break;
                    case GraphType.Undirected:
                        for (int i = 0; i < mat.GetLength(0); i++)
                        {
                            for (int j = i; j < mat.GetLength(1); j++)
                            {
                                if (mat[i, j] != 0)
                                    Edges.Add(new Edge($"U{++c}", c, false, Vertices[i], Vertices[j], mat[i, j]));
                            }
                        }
                        break;

                    default:
                        throw new NotImplementedException("Нельзя использовать другие типы матриц.");
                }
            }
            else
            {   // По матрице инцидентности
                for (int i = 0; i < mat.GetLength(1); i++)
                {
                    double[] vals = new double[mat.GetLength(0)];
                    for (int j = 0; j < vals.Length; j++)
                    {
                        vals[j] = mat[j, i] ?? 0;
                    }

                    var begin = vals.ToList().FindIndex((x) => x == 1);
                    var end = vals.ToList().FindLastIndex((x) => (type == GraphType.Undirected ? x == 1 : x == -1));

                    if (end != -1)
                        Edges.Add(new Edge($"U{i + 1}", i, Convert.ToBoolean((int)type - 1), Vertices[begin], Vertices[end]));
                    else
                        Edges.Add(new Edge($"U{i + 1}", i, Convert.ToBoolean((int)type - 1), Vertices[begin]));
                }
            }

            // Инициализация
            Vertices.ForEach((x) => x.Graph = this);
            Edges.ForEach((x) => x.Graph = this);
        }

        public int[,] AdjacencyMatrix
        {
            get
            {
                int[,] mat = new int[Vertices.Count, Vertices.Count];

                for (int i = 0; i < Vertices.Count; i++)
                {
                    for (int j = 0; j < Vertices.Count; j++)
                    {
                        mat[i, j] = Vertices[i].AdjacencyRow[j];
                    }
                }

                return mat;
            }
        }

        public int[,] IncidenceMatrix
        {
            get
            {
                int[,] mat = new int[Vertices.Count, Edges.Count];

                for (int i = 0; i < Edges.Count; i++)
                {
                    mat[Edges[i].Begin.Index, i] = 1;
                    mat[Edges[i].End.Index, i] = (Edges[i].HasDirection && Edges[i].Begin.Index != Edges[i].End.Index ? -1 : 1);
                }

                return mat;
            }
        }

        public int[,] ReachabilityMatrix
        {
            get
            {
                //if (Type != GraphType.Directed)
                //{
                //    return null;
                //}
                int[,] mat = new int[Vertices.Count, Vertices.Count];
                for (int i = 0; i < Vertices.Count; i++)
                    for (int j = 0; j < Vertices.Count; j++)
                        mat[i, j] = AdjacencyMatrix[i, j];


                for (int k = 0; k < Vertices.Count; k++)
                    for (int i = 0; i < Vertices.Count; i++)
                        for (int j = 0; j < Vertices.Count; j++)
                            mat[i, j] = mat[i, j] | (mat[i, k] & mat[k, j]);

                for (int i = 0; i < Vertices.Count; i++)
                    mat[i, i] = 1;

                return mat;
            }
        }

        public string AdjacencyMatrixStr
        {
            get { return CommonFunctions.GetStrMatrix(AdjacencyMatrix); }
        }

        public string IncidenceMatrixStr
        {
            get { return CommonFunctions.GetStrMatrix(IncidenceMatrix); }
        }

        public string ReachabilityMatrixStr
        {

            get { return CommonFunctions.GetStrMatrix(ReachabilityMatrix); }
        }

        enum V { W = 1, G = 2, B = 3 }

        bool DFS(Vertex v, List<Vertex> visitedV, List<Edge> visitedE)
        {
            if (visitedV.Contains(v))
                return true;
            visitedV.Add(v);
            for (int i = 0; i < v.Edges.Length; i++)
            {
                if (visitedE.Contains(v.Edges[i]))
                    continue;
                visitedE.Add(v.Edges[i]);
                var v2 = v.Edges[i].GetOpposite(v);
                if (DFS(v2, visitedV, visitedE))
                    return true;
            }

            return false;
        }

        public bool HasLoops
        {
            get
            {
                List<Vertex> visitedV = new List<Vertex>();
                List<Edge> visitedE = new List<Edge>();


                Vertex ve;
                //bool b = false;
                while ((ve = Vertices.FirstOrDefault((x) => !visitedV.Contains(x))) != null)
                {
                    if (DFS(ve, visitedV, visitedE))
                        return true;
                }

                return false;
            }
        }

        public double[,] Dijkstra()
        {
            double[,] dij = new double[Vertices.Count, Vertices.Count];
            for (int i = 0; i < Vertices.Count; i++)
            {
                double[] b = DijkstraOneVertex(Vertices[i]);
                for (int j = 0; j < b.Length; j++)
                    dij[i, j] = b[j];
            }

            return dij;
        }

        public double[] DijkstraOneVertex(Vertex s)
        {
            double[] D = new double[Vertices.Count];
            bool[] used = new bool[Vertices.Count];
            foreach (var v in Vertices)
            {
                D[v.Index] = double.PositiveInfinity;
                used[v.Index] = false;
            }
            D[s.Index] = 0;
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertex v = null;
                for (int j = 0; j < Vertices.Count; j++)
                {
                    if (!used[j] && (v == null || D[j] < D[v.Index]))
                        v = Vertices[j];
                }
                if (double.IsInfinity(D[v.Index]))
                    break;
                used[v.Index] = true;
                foreach (var e in v.Edges) // релаксация по всем рёбрам из v;
                {
                    if (D[v.Index] + e.Weight < D[e.GetOpposite(v).Index])
                        D[e.GetOpposite(v).Index] = D[v.Index] + e.Weight ?? 0;
                }
            }

            return D;
        }

    }

    internal static class CommonFunctions
    {
        static public string GetStrMatrix<T>(T[,] mat)
        {
            string str = "";
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    str += $"{mat[i, j]}\t";
                }
                str += "\r\n";
            }
            return str;
        }

        public static T[] Flatten<T>(this Array data)
        {
            var list = new List<T>();
            var stack = new Stack<IEnumerator>();
            stack.Push(data.GetEnumerator());
            do
            {
                for (var iterator = stack.Pop(); iterator.MoveNext();)
                {
                    if (iterator.Current is Array)
                    {
                        stack.Push(iterator);
                        iterator = (iterator.Current as IEnumerable).GetEnumerator();
                    }
                    else
                        list.Add((T)iterator.Current);
                }
            }
            while (stack.Count > 0);
            return list.ToArray();
        }
    }

}