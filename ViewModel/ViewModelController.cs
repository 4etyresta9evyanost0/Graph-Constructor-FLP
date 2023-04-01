using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Constructor_FLP.ViewModel
{
    static class ViewModelController
    {
        public static SettingsViewModel SettingsViewModel { get => App.Current.Resources["settingsVm"] as SettingsViewModel; }
        public static ApplicationViewModel ApplicationViewModel { get => App.Current.Resources["applicationVm"] as ApplicationViewModel; }

        public static ObjectsViewModel ObjectsViewModel { get => App.Current.Resources["objectsVm"] as ObjectsViewModel; }
    }
}
