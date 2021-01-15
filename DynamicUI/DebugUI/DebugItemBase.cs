using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicUI.DebugUI
{
    public class DebugItemBase
    {
        public string Name { get; set; }

    }


    public class SimpleDebugAction : DebugItemBase
    {
        public Action OnInvoked { get; set; }
    }

    public class ComboDebugItem : DebugItemBase
    {
        public string Selected { get; set; }
        public List<string> Variants { get; set; } = new List<string>();
        public Action<string> OnSelected { get; set; }
    }


}
