using System;
using System.Collections.Generic;
using System.Text;
using DynamicUI;

namespace DynamicUIExample
{
    public class DebugPage : DebugPageBase
    {

        public DebugPage() : base()
        {


            AddAction("TEST", () =>
            {
                WriteString($"TEST");
            });

        }

    }
}
