using Newtonsoft.Json.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Graph_Constructor_FLP
{
    /// <summary>
    /// Логика взаимодействия для BorderedLine.xaml
    /// </summary>
    public partial class BorderedLine : UserControl
    {
        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }

        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register("X1", typeof(double), typeof(BorderedLine), new PropertyMetadata(0d));

        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }

        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register("Y1", typeof(double), typeof(BorderedLine), new PropertyMetadata(0d));

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2", typeof(double), typeof(BorderedLine), new PropertyMetadata(0d));

        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2", typeof(double), typeof(BorderedLine), new PropertyMetadata(0d));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(BorderedLine), new PropertyMetadata(Brushes.Black));

        public Brush StrokePaddingBrush
        {
            get { return (Brush)GetValue(StrokePaddingBrushProperty); }
            set { SetValue(StrokePaddingBrushProperty, value); }
        }

        public static readonly DependencyProperty StrokePaddingBrushProperty =
            DependencyProperty.Register("StrokePaddingBrush", typeof(Brush), typeof(BorderedLine), new PropertyMetadata(Brushes.Transparent));

        public Brush StrokeBorderBrush
        {
            get { return (Brush)GetValue(StrokeBorderProperty); }
            set { SetValue(StrokeBorderProperty, value); }
        }

        public static readonly DependencyProperty StrokeBorderProperty =
            DependencyProperty.Register("StrokeBorderBrush", typeof(Brush), typeof(BorderedLine), new PropertyMetadata(Brushes.Transparent));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(BorderedLine), new PropertyMetadata(1d));

        public double StrokePadding
        {
            get { return (double)GetValue(StrokePaddingProperty); }
            set { SetValue(StrokePaddingProperty, value); }
        }

        public static readonly DependencyProperty StrokePaddingProperty =
            DependencyProperty.Register("StrokePadding", typeof(double), typeof(BorderedLine), new PropertyMetadata(0d));

        public double StrokeBorderThickness
        {
            get { return (double)GetValue(StrokeBorderThicknessProperty); }
            set { SetValue(StrokeBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeBorderThicknessProperty =
            DependencyProperty.Register("StrokeBorderThickness", typeof(double), typeof(BorderedLine), new PropertyMetadata(0d));

        public BorderedLine()
        {
            InitializeComponent();
        }
    }
}
