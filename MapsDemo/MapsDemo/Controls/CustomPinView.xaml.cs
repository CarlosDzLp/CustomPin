using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MapsDemo.Controls
{
    public partial class CustomPinView : ContentView
    {
        public static BindableProperty LoadingProperty =
            BindableProperty.Create(nameof(Loading),
                typeof(bool),
                typeof(CustomPinView),
                defaultValue: false,
                propertyChanged: (bindable, oldVal, newVal) =>
                {
                    ((CustomPinView)bindable).UpdateLoading((bool)newVal);
                });


        public static BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title),
                typeof(string),
                typeof(CustomPinView),
                defaultValue: string.Empty,
                propertyChanged: (bindable, oldVal, newVal) =>
                {
                    ((CustomPinView)bindable).UpdateTitle((string)newVal);
                });

        private void UpdateTitle(string newVal)
        {
            lbltext.Text = newVal;
        }

        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }
        private void UpdateLoading(bool newVal)
        {
            framestack.IsVisible = true;
            if (newVal)
            {
                framestack.WidthRequest = 50;
                activity.IsVisible = true;
                lbltext.IsVisible = false;
            }
            else
            {
                framestack.WidthRequest = 200;
                activity.IsVisible = false;
                lbltext.IsVisible = true;
            }
        }

        public bool Loading
        {
            get
            {
                return (bool)GetValue(LoadingProperty);
            }
            set
            {
                SetValue(LoadingProperty, value);
            }
        }
        public CustomPinView()
        {
            InitializeComponent();
            activity.IsVisible = false;
            lbltext.IsVisible = false;
            framestack.IsVisible = false;
        }
    }
}
