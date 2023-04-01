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

using MahApps.Metro.Controls;
using ControlzEx.Theming;
using DevExpress.Mvvm.Native;
using System.Windows.Controls.Primitives;
using Graph_Constructor_FLP.ViewModel;
using Graph_Constructor_FLP.Windows;
using DevExpress.Mvvm.ModuleInjection.Native;

namespace Graph_Constructor_FLP
{
    
    
    
    public partial class MainWindow : Window
    {
        SettingsWindow _settingsWindow;
        ApplicationViewModel AppVm => ViewModelController.ApplicationViewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadSettings_Click(object sender, RoutedEventArgs e)
        {
            _settingsWindow.Show();
            _settingsWindow.Owner = this;
        }

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _settingsWindow = new();
            debugButton.Visibility = AppVm.IsDebug ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Debug_Click(object sender, RoutedEventArgs e)
        {
            AppVm.ObjectsVm.CanvasObjects.Add(
                new Vertex(25d, 25d, 75d, 75d)
                );
        }

        private void Debug_Click_2(object sender, RoutedEventArgs e)
        {
            AppVm.ObjectsVm.CanvasObjects.Add(
                new Edge(4d,3d,2d,1d)
                );
        }

        private void Debug_Click_3(object sender, RoutedEventArgs e)
        {
            (AppVm.ObjectsVm.CanvasObjects[0] as Vertex).FillColor = Colors.Black;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Q: CBExecute(addingButton); break;
                case Key.W: CBExecute(movingButton); break;
                case Key.E: CBExecute(connectingButton); break;
                case Key.R: CBExecute(deletingButton); break;
            }
        }

        public void ExecuteWithParams(ButtonBase x) => x.Command.Execute(x.CommandParameter);

        public void CBExecute(RadioButton x)
        {
            ExecuteWithParams(x); x.IsChecked = true;
        }


        // Mouse Works
        #region Vertex Mouse Fields
        private bool isLeftMouseButtonDownOnWindow = false;
        private bool isDraggingSelectionRect = false;
        private Point origMouseDownPoint;
        private static readonly double DragThreshold = 5;
        private bool isLeftMouseDownOnRectangle = false;
        private bool isLeftMouseAndControlDownOnRectangle = false;
        private bool isDraggingRectangle = false;
        #endregion

        private void Vertex_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left || AppVm.CanvasAction != CanvasAction.Moving)
                return;

            var ellipse = (FrameworkElement)sender;
            var vertVm = (Vertex)ellipse.DataContext;

            isLeftMouseDownOnRectangle = true;

            if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) != 0)
                isLeftMouseAndControlDownOnRectangle = true;
            else
            {
                isLeftMouseAndControlDownOnRectangle = false;
                if (mainCanvas.SelectedItems.Count == 0)
                    mainCanvas.SelectedItems.Add(vertVm);
                else if (!mainCanvas.SelectedItems.Contains(vertVm))
                {
                    mainCanvas.SelectedItems.Clear();
                    mainCanvas.SelectedItems.Add(vertVm);
                }
            }

            ellipse.CaptureMouse();
            origMouseDownPoint = e.GetPosition(mainCanvas);

            e.Handled = true;
        }
        private void Vertex_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isLeftMouseDownOnRectangle)
            {
                var ellipse = (FrameworkElement)sender;
                var vertVm = (Vertex)ellipse.DataContext;

                if (!isDraggingRectangle)
                    if (isLeftMouseAndControlDownOnRectangle)
                        if (mainCanvas.SelectedItems.Contains(vertVm))
                            mainCanvas.SelectedItems.Remove(vertVm);
                        else
                            mainCanvas.SelectedItems.Add(vertVm);
                    else
                        if (mainCanvas.SelectedItems.Count != 1 ||
                            mainCanvas.SelectedItem != vertVm)
                        {
                            mainCanvas.SelectedItems.Clear();
                            mainCanvas.SelectedItems.Add(vertVm);
                        }

                ellipse.ReleaseMouseCapture();
                isLeftMouseDownOnRectangle = false;
                isLeftMouseAndControlDownOnRectangle = false;

                e.Handled = true;
            }

            isDraggingRectangle = false;
        }
        private void Vertex_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingRectangle)
            {
                Point curMouseDownPoint = e.GetPosition(mainCanvas);
                var dragDelta = curMouseDownPoint - origMouseDownPoint;

                origMouseDownPoint = curMouseDownPoint;

                foreach (CanvasObj rectangle in mainCanvas.SelectedItems)
                {
                    if (rectangle is not Vertex)
                        continue;
                    rectangle.X += dragDelta.X;
                    rectangle.Y += dragDelta.Y;

                }
            }
            else if (isLeftMouseDownOnRectangle)
            {
                Point curMouseDownPoint = e.GetPosition(mainCanvas);
                var dragDelta = curMouseDownPoint - origMouseDownPoint;
                double dragDistance = Math.Abs(dragDelta.Length);
                if (dragDistance > DragThreshold)
                    isDraggingRectangle = true;

                e.Handled = true;
            }
        }

        // Canvas Mouse
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left || AppVm.CanvasAction != CanvasAction.Moving)
                return;

            mainCanvas.SelectedItems.Clear();
            isLeftMouseButtonDownOnWindow = true;
            origMouseDownPoint = e.GetPosition(mainCanvas);
            mainCanvas.CaptureMouse();
            e.Handled = true;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left || AppVm.CanvasAction != CanvasAction.Moving)
            {
                return;
            }

            bool wasDragSelectionApplied = false;

            if (isDraggingSelectionRect)
            {
                isDraggingSelectionRect = false;
                ApplyDragSelectionRect();
                wasDragSelectionApplied = true;
                e.Handled = true;
            }

            if (isLeftMouseButtonDownOnWindow)
            {
                isLeftMouseButtonDownOnWindow = false;
                mainCanvas.ReleaseMouseCapture();
                e.Handled = true;
            }

            if (!wasDragSelectionApplied)
                mainCanvas.SelectedItems.Clear();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingSelectionRect)
            {
                Point curMouseDownPoint = e.GetPosition(mainCanvas);
                UpdateDragSelectionRect(origMouseDownPoint, curMouseDownPoint);
                e.Handled = true;
            }
            else if (isLeftMouseButtonDownOnWindow)
            {
                Point curMouseDownPoint = e.GetPosition(mainCanvas);
                var dragDelta = curMouseDownPoint - origMouseDownPoint;
                double dragDistance = Math.Abs(dragDelta.Length);
                if (dragDistance > DragThreshold)
                {
                    isDraggingSelectionRect = true;
                    InitDragSelectionRect(origMouseDownPoint, curMouseDownPoint);
                }
                e.Handled = true;
            }
        }

        private void InitDragSelectionRect(Point pt1, Point pt2)
        {
            UpdateDragSelectionRect(pt1, pt2);
            dragSelectionCanvas.Visibility = Visibility.Visible;
        }

        private void UpdateDragSelectionRect(Point pt1, Point pt2)
        {
            double x, y, width, height;

            if (pt2.X < pt1.X)
            {
                x = pt2.X;
                width = pt1.X - pt2.X;
            }
            else
            {
                x = pt1.X;
                width = pt2.X - pt1.X;
            }

            if (pt2.Y < pt1.Y)
            {
                y = pt2.Y;
                height = pt1.Y - pt2.Y;
            }
            else
            {
                y = pt1.Y;
                height = pt2.Y - pt1.Y;
            }

            Canvas.SetLeft(dragSelectionBorder, x);
            Canvas.SetTop(dragSelectionBorder, y);
            dragSelectionBorder.Width = width;
            dragSelectionBorder.Height = height;
        }

        private void ApplyDragSelectionRect()
        {
            dragSelectionCanvas.Visibility = Visibility.Collapsed;

            double x = Canvas.GetLeft(dragSelectionBorder);
            double y = Canvas.GetTop(dragSelectionBorder);
            double width = dragSelectionBorder.Width;
            double height = dragSelectionBorder.Height;
            Rect dragRect = new Rect(x, y, width, height);

            dragRect.Inflate(width / 10, height / 10);

            mainCanvas.SelectedItems.Clear();
            
            foreach (Vertex vert in ViewModelController.ObjectsViewModel.Vertices)
            {
                Rect itemRect = new Rect(vert.X, vert.Y, vert.Width ?? ViewModelController.SettingsViewModel.VertexDiameter, vert.Height ?? ViewModelController.SettingsViewModel.VertexDiameter);
                if (dragRect.Contains(itemRect))
                    mainCanvas.SelectedItems.Add(vert);
            }
        }
    }
}
