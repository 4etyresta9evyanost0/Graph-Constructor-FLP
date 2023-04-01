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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SettingsWindow _settingsWindow;
        ApplicationViewModel AppVm => ViewModelController.ApplicationViewModel;

        public MainWindow()
        {
            InitializeComponent();

            // Потом реализовать, наверное
            //
            //Closing += (x, ev) =>
            //{
            //    var closingWindow = new OnClosingDialogWindow();
            //    ev.Cancel = !closingWindow.ShowDialog()!.Value;
            //};

        }

        private void LoadSettings_Click(object sender, RoutedEventArgs e)
        {
            _settingsWindow.Show();
            _settingsWindow.Owner = this;
        }

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //
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


        // Vertex Mouse
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
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            var ellipse = (FrameworkElement)sender;
            var vertVm = (Vertex)ellipse.DataContext;

            isLeftMouseDownOnRectangle = true;

            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                //
                // Control key was held down.
                // This means that the rectangle is being added to or removed from the existing selection.
                // Don't do anything yet, we will act on this later in the MouseUp event handler.
                //
                isLeftMouseAndControlDownOnRectangle = true;
            }
            else
            {
                //
                // Control key is not held down.
                //
                isLeftMouseAndControlDownOnRectangle = false;

                if (this.mainCanvas.SelectedItems.Count == 0)
                {
                    this.mainCanvas.SelectedItems.Add(vertVm);
                }
                else if (this.mainCanvas.SelectedItems.Contains(vertVm))
                {
                    // 
                    // Item is already selected, do nothing.
                    // We will act on this in the MouseUp if there was no drag operation.
                    //
                }
                else
                {
                    this.mainCanvas.SelectedItems.Clear();
                    this.mainCanvas.SelectedItems.Add(vertVm);
                }
            }

            ellipse.CaptureMouse();
            origMouseDownPoint = e.GetPosition(this);

            e.Handled = true;
        }
        private void Vertex_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isLeftMouseDownOnRectangle)
            {
                var ellipse = (FrameworkElement)sender;
                var vertVm = (Vertex)ellipse.DataContext;

                if (!isDraggingRectangle)
                {
                    //
                    // Execute mouse up selection logic only if there was no drag operation.
                    //
                    if (isLeftMouseAndControlDownOnRectangle)
                    {
                        //
                        // Control key was held down.
                        // Toggle the selection.
                        //
                        if (this.mainCanvas.SelectedItems.Contains(vertVm))
                        {
                            //
                            // Item was already selected, control-click removes it from the selection.
                            //
                            this.mainCanvas.SelectedItems.Remove(vertVm);
                        }
                        else
                        {
                            // 
                            // Item was not already selected, control-click adds it to the selection.
                            //
                            this.mainCanvas.SelectedItems.Add(vertVm);
                        }
                    }
                    else
                    {
                        //
                        // Control key was not held down.
                        //
                        if (this.mainCanvas.SelectedItems.Count == 1 &&
                            this.mainCanvas.SelectedItem == vertVm)
                        {
                            //
                            // The item that was clicked is already the only selected item.
                            // Don't need to do anything.
                            //
                        }
                        else
                        {
                            //
                            // Clear the selection and select the clicked item as the only selected item.
                            //  
                            this.mainCanvas.SelectedItems.Clear();
                            this.mainCanvas.SelectedItems.Add(vertVm);
                        }
                    }
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
                //
                // Drag-move selected rectangles.
                //
                Point curMouseDownPoint = e.GetPosition(this);
                var dragDelta = curMouseDownPoint - origMouseDownPoint;

                origMouseDownPoint = curMouseDownPoint;

                foreach (CanvasObj rectangle in this.mainCanvas.SelectedItems)
                {
                    if (rectangle is not Vertex)
                        continue;
                    rectangle.X += dragDelta.X;
                    rectangle.Y += dragDelta.Y;

                }
            }
            else if (isLeftMouseDownOnRectangle)
            {
                //
                // The user is left-dragging the rectangle,
                // but don't initiate the drag operation until
                // the mouse cursor has moved more than the threshold value.
                //
                Point curMouseDownPoint = e.GetPosition(this);
                var dragDelta = curMouseDownPoint - origMouseDownPoint;
                double dragDistance = Math.Abs(dragDelta.Length);
                if (dragDistance > DragThreshold)
                {
                    //
                    // When the mouse has been dragged more than the threshold value commence dragging the rectangle.
                    //
                    isDraggingRectangle = true;
                }

                e.Handled = true;
            }
        }

        // Canvas Mouse
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
