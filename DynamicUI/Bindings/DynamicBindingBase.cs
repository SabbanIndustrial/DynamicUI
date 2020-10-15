using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DynamicUI.Bindings
{
    public class DynamicBindingBase: INotifyPropertyChanged
    {
        public string Label { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
