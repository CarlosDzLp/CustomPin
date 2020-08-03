using Xamarin.Forms;
using System;
using Xamarin.Forms.Platform.iOS;
using MapsDemo.iOS.Controls;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace MapsDemo.iOS.Controls
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Control.Layer.CornerRadius = 0;
                Control.BorderStyle = UIKit.UITextBorderStyle.None;
                Control.Layer.BorderWidth = 0;
                Control.ClipsToBounds = false;
            }
        }
    }
}
