using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.Mvvm;

namespace Graph_Constructor_FLP.ViewModel
{
    abstract class ViewModel : ViewModelBase
    {
        #region Data Members
        ///
        #endregion

        public ViewModel() { }
        #region INotifyPropertyChanged Members
        //public event PropertyChangedEventHandler? PropertyChanged;
        //protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }

}
