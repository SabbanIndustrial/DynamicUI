using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace DynamicUI
{
    public class UIDynamicObject
    {
        public string Name { get; set; }
        public Action Action { get; set; }
        public Action<bool> SwitchAction { get; set; }

        public View View { get; set; }

        public UIDynType UIType { get; set; }

        public List<string> PickerVariants { get; set; } = new List<string>();
        public string PickerSelected { get; set; }
        public Action<string> PickerAction;
    }

    public enum UIDynType
    {
        Button,
        Label,
        View,
        Switch,
        Picker,
    }
}
