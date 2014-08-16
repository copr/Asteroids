using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Engine
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler lHandler = PropertyChanged;
            if (lHandler != null)
            {
                lHandler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
