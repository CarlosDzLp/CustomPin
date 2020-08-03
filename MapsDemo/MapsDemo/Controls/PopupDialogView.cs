using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
namespace MapsDemo.Controls
{
    [ContentProperty("DialogContent")]
    public class PopupDialogView : ContentView
    {
        private readonly StackLayout _popUpBgLayout;
        private readonly ContentView _popUpDialogLayout;
        private readonly ContentView _dialogContent;
        private int _yTranslation = 5000;

        public event EventHandler DialogClosed;
        public event EventHandler DialogShow;
        public event EventHandler DialogClosing;
        public event EventHandler DialogShowing;

        public static readonly BindableProperty DialogMarginProperty = BindableProperty.Create(nameof(DialogMargin), typeof(Thickness), typeof(PopupDialogView), new Thickness(10), BindingMode.TwoWay);
        public static readonly BindableProperty BackgroundOpacityProperty = BindableProperty.Create(nameof(BackgroundOpacity), typeof(double), typeof(PopupDialogView), 1.0, BindingMode.TwoWay);
        public static readonly BindableProperty IsShowBackgroundProperty = BindableProperty.Create(nameof(IsShowBackground), typeof(bool), typeof(PopupDialogView), true, BindingMode.TwoWay);
        public static readonly BindableProperty ShowDialogCommandProperty = BindableProperty.Create(nameof(ShowDialogCommand), typeof(ICommand), typeof(PopupDialogView), null, BindingMode.OneWayToSource);
        public static readonly BindableProperty HideDialogCommandProperty = BindableProperty.Create(nameof(HideDialogCommand), typeof(ICommand), typeof(PopupDialogView), null, BindingMode.OneWayToSource);


        public static BindableProperty IsShowDialogProperty =
            BindableProperty.Create(nameof(IsShowDialog),
                typeof(bool),
                typeof(PopupDialogView),
                propertyChanged: (bindable, oldVal, newVal) =>
                {
                    ((PopupDialogView)bindable).UpdateShowDialog((bool)newVal);
                });
        public void UpdateShowDialog(bool IsShow)
        {
            if (IsShow)
            {
                ShowDialog();
            }
            else
            {
                HideDialog();
            }
            //BodyLabel.Text = text;
        }
        public bool IsShowDialog
        {
            get
            {
                return (bool)GetValue(IsShowDialogProperty);
            }
            set
            {
                SetValue(IsShowDialogProperty, value);
            }
        }





        public ICommand ShowDialogCommand
        {
            get => (ICommand)GetValue(ShowDialogCommandProperty);
            set => SetValue(ShowDialogCommandProperty, value);
        }

        public ICommand HideDialogCommand
        {
            get => (ICommand)GetValue(HideDialogCommandProperty);
            set => SetValue(HideDialogCommandProperty, value);
        }

        public Thickness DialogMargin
        {
            get => (Thickness)GetValue(DialogMarginProperty);
            set => SetValue(DialogMarginProperty, value);
        }

        public double BackgroundOpacity
        {
            get => (double)GetValue(BackgroundOpacityProperty);
            set => SetValue(BackgroundOpacityProperty, value);
        }

        public bool IsShowBackground
        {
            get => (bool)GetValue(IsShowBackgroundProperty);
            set => SetValue(IsShowBackgroundProperty, value);
        }


        public PopupDialogView()
        {
            var mainGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            _popUpBgLayout = new StackLayout
            {
                BackgroundColor = Color.Transparent,
                Opacity = BackgroundOpacity,
                Padding = new Thickness(0),
                Spacing = 0,
                Margin = new Thickness(0),
            };
            _popUpDialogLayout = new ContentView()
            {
                BackgroundColor = Color.White,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0),
                Margin = new Thickness(0),
            };

            _dialogContent = new ContentView();

            _popUpDialogLayout.Content = new StackLayout
            {
                Children = { _dialogContent }
            };

            mainGrid.Children.Add(_popUpBgLayout);
            mainGrid.Children.Add(_popUpDialogLayout);

            Content = new StackLayout
            {
                Children = {
                    mainGrid
                }
            };

            _popUpBgLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(HideDialog)
            });

            //BINDINGS FROM PROPERTY
            _popUpDialogLayout.SetBinding(Frame.MarginProperty, new Binding(nameof(DialogMargin), BindingMode.TwoWay, null, null, null, this));
            _popUpBgLayout.SetBinding(StackLayout.OpacityProperty, new Binding(nameof(BackgroundOpacity), BindingMode.TwoWay, null, null, null, this));

            this.VerticalOptions = LayoutOptions.FillAndExpand;
            this.TranslationY = _yTranslation;

            //COMMAND
            ShowDialogCommand = new Command(ShowDialog);
            HideDialogCommand = new Command(HideDialog);
        }

        public View DialogContent
        {
            get => _dialogContent.Content;
            set => _dialogContent.Content = value;
        }

        public void ShowDialog()
        {
            ShowDialogAnimation(PopUpDialogLayout, _popUpBgLayout);
        }

        public void HideDialog()
        {
            HideDialogAnimation(PopUpDialogLayout, _popUpBgLayout);
            IsShowDialog = false;
        }

        public ContentView PopUpDialogLayout => _popUpDialogLayout;

        protected virtual void OnDialogClosed(EventArgs e)
        {
            DialogClosed?.Invoke(this, e);
        }


        protected virtual void OnDialogShow(EventArgs e)
        {
            DialogShow?.Invoke(this, e);
        }

        protected virtual void OnDialogClosing(EventArgs e)
        {
            DialogClosing?.Invoke(this, e);
        }

        protected virtual void OnDialogShowing(EventArgs e)
        {
            DialogShowing?.Invoke(this, e);
        }

        private void ShowDialogAnimation(VisualElement dialog, VisualElement bg)
        {
            this.TranslationY = 0;
            dialog.TranslationY = bg.Height;
            _popUpBgLayout.IsVisible = IsShowBackground;

            ////ANIMATIONS 
            var showDialogAnimation = TransLateYAnimation(dialog, bg.Height, 0);

            ////EXECUTE ANIMATIONS
            this.Animate("showMenu", showDialogAnimation, 16, 250, Easing.Linear, (d, f) =>
            {
                if (IsShowBackground)
                {
                    var showBgAnimation = OpacityAnimation(bg, 0, 0.5);
                    this.Animate("showBg", showBgAnimation, 16, 300, Easing.Linear, (x, y) => { });
                }

                OnDialogShow(new EventArgs());
            });

            OnDialogShowing(new EventArgs());
        }

        private void HideDialogAnimation(VisualElement dialog, VisualElement bg)
        {
            dialog.TranslationY = 0;

            ////ANIMATIONS     
            var showDialogAnimation = TransLateYAnimation(dialog, 0, bg.Height);

            ////EXECUTE ANIMATIONS
            this.Animate("hideMenu", showDialogAnimation, 16, 250, Easing.Linear, (x, y) =>
            {
                if (IsShowBackground)
                {
                    var hideBgAnimation = OpacityAnimation(bg, 0.5, 0);
                    this.Animate("hideBg", hideBgAnimation, 16, 200, Easing.Linear,
                        (d, f) => { Device.BeginInvokeOnMainThread(() => { this.TranslationY = _yTranslation; }); });
                }
                else
                {
                    this.TranslationY = _yTranslation;
                }

                OnDialogClosed(new EventArgs());
            });

            OnDialogClosing(new EventArgs());
        }

        private static Animation TransLateYAnimation(VisualElement element, double from, double to)
        {
            return new Animation(d => { element.TranslationY = d; }, from, to);
        }

        private static Animation TransLateXAnimation(VisualElement element, double from, double to)
        {
            return new Animation(d => { element.TranslationX = d; }, from, to);
        }

        private static Animation OpacityAnimation(VisualElement element, double from, double to)
        {
            var result = element.Opacity;
            return new Animation(d => { element.Opacity = result; }, from, to);
        }

        //HIDE CONTENT PROPERTY
        private new View Content
        {
            get => base.Content;
            set => base.Content = value;
        }

    }
}
