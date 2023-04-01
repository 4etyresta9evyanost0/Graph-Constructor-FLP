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
        ObjectsViewModel ObjVm => ViewModelController.ObjectsViewModel;
        SettingsViewModel Settings => ViewModelController.SettingsViewModel;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _settingsWindow = new();
            debugButton.Visibility = AppVm.IsDebug ? Visibility.Visible : Visibility.Collapsed;
        }

        #region Menu Methods

        
        private void LoadSettings_Click(object sender, RoutedEventArgs e)
        {
            _settingsWindow.Show();
            _settingsWindow.Owner = this;
        }

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Delete_All_Edges_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < ObjVm.Edges.Count; i++)
                ObjVm.CanvasObjects.Remove(ObjVm.Edges[i]);
        }

        private void Delete_All_Objects_Click(object sender, RoutedEventArgs e) => ObjVm.CanvasObjects.Clear();

        #endregion

        #region Debug Methods
        private void Debug_Click(object sender, RoutedEventArgs e)
        {
            AppVm.ObjectsVm.CanvasObjects.Add(
                new Vertex(25d, 25d, 75d, 75d, "Debug_Vert")
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

        private void Debug_Click_4(object sender, RoutedEventArgs e)
        {
            var rnd = new Random();
            (AppVm.ObjectsVm.CanvasObjects[0] as Vertex).Value = rnd.Next(1024);
        }

        #endregion

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Q: CBExecute(addingButton); break;
                case Key.W: CBExecute(movingButton); break;
                case Key.E: CBExecute(connectingButton); break;
                case Key.R: CBExecute(deletingButton); break;
                case Key.A: if ((Keyboard.Modifiers & ModifierKeys.Control) != 0) mainCanvas.SelectAll(); break;
            }
        }
        public void ExecuteWithParams(ButtonBase x) => x.Command.Execute(x.CommandParameter);
        public void CBExecute(RadioButton x)
        {
            ExecuteWithParams(x); x.IsChecked = true;

            ResetMouseFields();
        }
        private void SwitchCanvas_Click(object sender, RoutedEventArgs e)
        {
            ResetMouseFields();
        }

        // Mouse Works
        #region  Mouse Fields
        private bool isLeftMouseButtonDownOnCanvas = false;
        private bool isDraggingSelectionRect = false;
        private Point origMouseDownPoint;
        private static readonly double DragThreshold = 5;
        private bool isLeftMouseDownOnVertex = false;
        private bool isLeftMouseAndControlDownOnVertex = false;
        private bool isDraggingVertex = false;
        #endregion

        private void ResetMouseFields()
        {
            isLeftMouseButtonDownOnCanvas = false;
            isDraggingSelectionRect = false;
            isLeftMouseDownOnVertex = false;
            isLeftMouseAndControlDownOnVertex = false;
            isDraggingVertex = false;

            mainCanvas.SelectedItems.Clear();
            mainCanvas.ReleaseMouseCapture();
        }

        #region Vertex MouseMethods

        private void Vertex_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            var ellipse = (FrameworkElement)sender;
            var vertVm = (Vertex)ellipse.DataContext;

            switch (AppVm.CanvasAction)     
            {
                case CanvasAction.Moving:
                case CanvasAction.Adding:


                    isLeftMouseDownOnVertex = true;

                    if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) != 0)
                        isLeftMouseAndControlDownOnVertex = true;
                    else
                    {
                        isLeftMouseAndControlDownOnVertex = false;
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
                    break;
                case CanvasAction.Deleting:
                    if (!Settings.IsAskingForDelete || MessageBox.Show("Вы уверены, что хотите удалить эту вершину?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        ObjVm.CanvasObjects.Remove(vertVm);
                    else
                        mainCanvas.SelectedItems.Clear();
                    break;
                default:
                    break;
            }

            
        }
        private void Vertex_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isLeftMouseDownOnVertex)
            {
                var ellipse = (FrameworkElement)sender;
                var vertVm = (Vertex)ellipse.DataContext;

                if (!isDraggingVertex)
                    if (isLeftMouseAndControlDownOnVertex)
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
                isLeftMouseDownOnVertex = false;
                isLeftMouseAndControlDownOnVertex = false;

                e.Handled = true;
            }

            isDraggingVertex = false;
        }
        private void Vertex_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingVertex)
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
            else if (isLeftMouseDownOnVertex)
            {
                Point curMouseDownPoint = e.GetPosition(mainCanvas);
                var dragDelta = curMouseDownPoint - origMouseDownPoint;
                double dragDistance = Math.Abs(dragDelta.Length);
                if (dragDistance > DragThreshold)
                    isDraggingVertex = true;

                e.Handled = true;
            }
        }

        #endregion

        // Canvas Mouse
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            mainCanvas.SelectedItems.Clear();
            isLeftMouseButtonDownOnCanvas = true;
            origMouseDownPoint = e.GetPosition(mainCanvas);

            switch (AppVm.CanvasAction)
            {
                case CanvasAction.Adding:
                    Adding_Canvas_MouseDown(sender, e);
                    break;
                case CanvasAction.Moving:
                    Moving_Canvas_MouseDown(sender, e);
                    break;
                case CanvasAction.Connecting:
                    break;
                case CanvasAction.Deleting:
                    Deleting_Canvas_MouseDown(sender, e);
                    break;
                default:
                    break;  
            }

            
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            switch (AppVm.CanvasAction)
            {
                case CanvasAction.Adding:
                    Adding_Canvas_MouseUp(sender, e);
                    break;
                case CanvasAction.Moving:
                    Moving_Canvas_MouseUp(sender, e);
                    break;
                case CanvasAction.Connecting:
                    break;
                case CanvasAction.Deleting:
                    Deleting_Canvas_MouseUp(sender, e);
                    break;
                default:
                    break;
            }
            
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            switch (AppVm.CanvasAction)
            {
                case CanvasAction.Adding:
                    Adding_Canvas_MouseMove(sender, e);
                    break;
                case CanvasAction.Moving:
                    Moving_Canvas_MouseMove(sender, e);
                    break;
                case CanvasAction.Connecting:
                    break;
                case CanvasAction.Deleting:
                    Deleting_Canvas_MouseMove(sender, e);
                    break;
                default:
                    break;
            }
            
        }


        #region Moving (Selection) Mouse Methods

        private void Moving_Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainCanvas.CaptureMouse();
            e.Handled = true;
        }
        private void Moving_Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bool wasDragSelectionApplied = false;

            if (isDraggingSelectionRect)
            {
                isDraggingSelectionRect = false;
                ApplyDragSelectionRect();
                wasDragSelectionApplied = true;
                e.Handled = true;
            }

            if (isLeftMouseButtonDownOnCanvas)
            {
                isLeftMouseButtonDownOnCanvas = false;
                mainCanvas.ReleaseMouseCapture();
                e.Handled = true;
            }

            if (!wasDragSelectionApplied)
                mainCanvas.SelectedItems.Clear();
        }
        private void Moving_Canvas_MouseMove(object sender, MouseEventArgs e) 
        {
            if (isDraggingSelectionRect)
            {
                Point curMouseDownPoint = e.GetPosition(mainCanvas);
                UpdateDragSelectionRect(origMouseDownPoint, curMouseDownPoint);
                e.Handled = true;
            }
            else if (isLeftMouseButtonDownOnCanvas)
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

        #endregion

        #region Adding Mouse Methods

        private void Adding_Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainCanvas.SelectedItems.Clear();

            var pos = e.GetPosition(mainCanvas);
            ObjVm.CanvasObjects.Add(new Vertex(pos.X - Settings.VertexDiameter / 2, pos.Y - Settings.VertexDiameter / 2, 0, 0));
        }
        private void Adding_Canvas_MouseUp(object sender, MouseButtonEventArgs e) { }
        private void Adding_Canvas_MouseMove(object sender, MouseEventArgs e) { }

        #endregion


        #region Deleting Mouse Methods

        private void Deleting_Canvas_MouseDown(object sender, MouseButtonEventArgs e) => Moving_Canvas_MouseDown(sender, e);
        private void Deleting_Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Moving_Canvas_MouseUp(sender, e);

            if (mainCanvas.SelectedItems.Count > 0 && (!Settings.IsAskingForDelete || MessageBox.Show("Вы уверены, что хотите удалить эту вершину?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes))
            {
                for (int i = mainCanvas.SelectedItems.Count - 1; i >= 0; i--)
                {
                    Vertex vert = (Vertex)mainCanvas.SelectedItems[i];
                    ObjVm.CanvasObjects.Remove(vert);
                }
            }
            else
                mainCanvas.SelectedItems.Clear();
        }
        private void Deleting_Canvas_MouseMove(object sender, MouseEventArgs e) => Moving_Canvas_MouseMove(sender, e);

        #endregion




    }
}
