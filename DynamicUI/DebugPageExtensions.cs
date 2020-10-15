using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace DynamicUI
{
    public static class DebugPageExtensions
    {

        public static void WriteString(this DebugPageBase debugPage, string toWrite)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Label temp = new Label()
                {
                    Text = $"{toWrite}",
                    FontFamily = DebugPageBase.MonoFont,

                };

                debugPage.InfoLayout.Children.Add(temp);
                debugPage.AutoScroll.ScrollToAsync(0, debugPage.AutoScroll.ContentSize.Height, true);
            });
        }
    }
}
