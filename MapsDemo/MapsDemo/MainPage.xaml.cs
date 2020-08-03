using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapsDemo
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        double Height { get; set; }
        public MainPage()
        {
            InitializeComponent();
            LocationLoad();
            map.CameraIdled += Map_CameraIdled;
            map.CameraMoveStarted += Map_CameraMoveStarted;
            Height = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height / Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
            custompin.TranslationY = -Height + 50;
        }
        private void Map_CameraMoveStarted(object sender, CameraMoveStartedEventArgs e)
        {
            if (e.IsGesture)
                custompin.Loading = true;
        }
        private void Map_CameraIdled(object sender, Xamarin.Forms.GoogleMaps.CameraIdledEventArgs e)
        {
            Console.WriteLine($"---------------{custompin.TranslationY}--------------");
            if (custompin.TranslationY == 0)
            {
                custompin.Loading = false;
                var location = map.CameraPosition.Target.Latitude + "--" + map.CameraPosition.Target.Longitude;
                custompin.Title = location.ToString();//"Carlos Evelin Diaz Lopez Carlos Evelin diaz Lopez Carlos Evelin diaz Lopez Carlos Evelin diaz Lopez";
                //Console.WriteLine($"---------------Longitud:{location.Longitude}  , Latitude:{location.Latitude}--------------");
            }
        }
        private async void LocationLoad()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.High);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(location.Latitude, location.Longitude), Distance.FromMiles(0.3)));

            }
        }


        int gesture = 0;
        async void TapGestureRecognizerNavigationArrow(System.Object sender, System.EventArgs e)
        {
            gesture = 0;
            await stacknavigation.FadeTo(1, 250, Easing.BounceOut);
            await stacknavigation.FadeTo(0, 250, Easing.BounceIn);
            stacksearch.BackgroundColor = Color.Transparent;
            PopupDialog.HideDialog();
        }

        int location = 0;
        void TapGestureRecognizerPin(System.Object sender, System.EventArgs e)
        {
            if (location == 0)
            {
                location = 1;
                AnimateExpandPin(custompin, (int)-Height, 0);
            }
            else
            {
                location = 0;
                AnimateExpandPin(custompin, 0, (int)-Height + 50);
            }
        }
        public void AnimateExpandPin(View view, int init, int finish)
        {
            var animation = new Animation(c => view.TranslationY = c, init, finish);
            animation.Commit(this, "ScaleIt", length: 100, easing: Easing.Linear,
                finished: (v, c) => view.TranslationY = finish, repeat: () => false);
        }

        async void Entry_FocusedOrigen(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            if (gesture == 0)
            {
                gesture = 1;
                //have la animacion
                await stacknavigation.FadeTo(0, 250, Easing.BounceIn);
                await stacknavigation.FadeTo(1, 250, Easing.BounceOut);
                stacksearch.BackgroundColor = Color.White;
                PopupDialog.ShowDialog();
            }
        }

        async void entrydos_FocusedDestino(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            if (gesture == 0)
            {
                gesture = 1;
                //have la animacion
                await stacknavigation.FadeTo(0, 250, Easing.BounceIn);
                await stacknavigation.FadeTo(1, 250, Easing.BounceOut);
                stacksearch.BackgroundColor = Color.White;
                PopupDialog.ShowDialog();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (gesture == 1)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    gesture = 0;
                    await stacknavigation.FadeTo(1, 250, Easing.BounceOut);
                    await stacknavigation.FadeTo(0, 250, Easing.BounceIn);
                    stacksearch.BackgroundColor = Color.Transparent;
                    PopupDialog.HideDialog();
                });
                return true;
            }
            return base.OnBackButtonPressed();
        }
    }
}
