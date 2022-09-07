using System.Diagnostics;
using Animation = Microsoft.Maui.Controls.Animation;

namespace SampleMaui;

public class MyFlower : Border
{

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(MyFlower));

    public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(nameof(ImageWidth), typeof(double), typeof(MyFlower), default(double));

    public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(nameof(ImageHeight), typeof(double), typeof(MyFlower), default(double));

    public static readonly BindableProperty ScrollDurationProperty = BindableProperty.Create(nameof(ScrollDuration), typeof(double), typeof(MyFlower), default(double), propertyChanged: OnScrollDurationChanged);

    public static readonly BindableProperty ReverseProperty = BindableProperty.Create(nameof(Reverse), typeof(bool), typeof(MyFlower), default(bool));

    public static readonly BindableProperty ScrollDirectionProperty = BindableProperty.Create(nameof(ScrollDirection), typeof(ScrollDirection), typeof(MyFlower), ScrollDirection.Left);


    public double ImageWidth
    {
        get { return (double)GetValue(ImageWidthProperty); }
        set { SetValue(ImageWidthProperty, value); }
    }

    public double ImageHeight
    {
        get { return (double)GetValue(ImageHeightProperty); }
        set { SetValue(ImageHeightProperty, value); }
    }

    public double ScrollDuration
    {
        get { return (double)GetValue(ScrollDurationProperty); }
        set { SetValue(ScrollDurationProperty, value); }
    }

    public bool Reverse
    {
        get { return (bool)GetValue(ReverseProperty); }
        set { SetValue(ReverseProperty, value); }
    }

    public ScrollDirection ScrollDirection
    {
        get { return (ScrollDirection)GetValue(ScrollDirectionProperty); }
        set { SetValue(ScrollDirectionProperty, value); }
    }

    public ImageSource ImageSource
    {
        get { return (ImageSource)GetValue(ImageSourceProperty); }
        set { SetValue(ImageSourceProperty, value); }
    }

    Image _first;
    Image? _second;
    Grid _container;
    public MyFlower()
    {
        _container = new()
        {
            WidthRequest = WidthRequest,
            HeightRequest = HeightRequest,
            Background = Colors.Maroon,
            IsClippedToBounds = true,
        };
        Content = _container;

        Loaded += MyFlower_Loaded;
    }

    private async void MyFlower_Loaded(object sender, EventArgs e)
    {
        _first = new Image { Source = ImageSource };
        _container.Children.Add(_first);
        if (!Reverse)
        {
            _second = new Image { Source = ImageSource };

            switch (ScrollDirection)
            {
                case ScrollDirection.Left:
                    _second.TranslationX = -_second.DesiredSize.Width;
                    break;
                case ScrollDirection.Up:
                    _second.TranslationY = -_second.DesiredSize.Height;
                    break;
                case ScrollDirection.Right:
                    _second.TranslationX = _second.DesiredSize.Width;
                    break;
                case ScrollDirection.Down:
                    _second.TranslationY = _second.DesiredSize.Height;
                    break;
            }

            _container.Children.Add(_second);
        }



        Action<double> callback = null;
        switch (ScrollDirection)
        {
            case ScrollDirection.Left:
                callback = new Action<double>((d) =>
                {
                    var width = _first.DesiredSize.Width;
                    var transX = -width * d;
                    _first.TranslationX = transX;
                    if (!Reverse)
                    {
                        _second.TranslationX = transX + width;
                    }
                });
                break;
            case ScrollDirection.Up:
                callback = new Action<double>((d) =>
                {
                    var height = _first.DesiredSize.Height;
                    var transY = -height * d;
                    _first.TranslationY = transY;
                    if (!Reverse)
                    {
                        _second.TranslationY = transY + height;
                    }
                });
                break;
            case ScrollDirection.Right:
                callback = new Action<double>((d) =>
                {
                    var width = _first.DesiredSize.Width;
                    var transX = -width * d;
                    _first.TranslationX = transX;
                    if (!Reverse)
                    {
                        _second.TranslationX = transX - width;
                    }
                });
                break;
            case ScrollDirection.Down:
                callback = new Action<double>((d) =>
                {
                    var height = _first.DesiredSize.Height;
                    var transY = height * d;
                    _first.TranslationY = transY;
                    if (!Reverse)
                    {
                        _second.TranslationY = transY - height;
                    }
                }); break;
            default:
                callback = new Action<double>((d) =>
                {
                    var width = _first.Width;
                    var transX = width * d;
                    _first.TranslationX = transX;
                    if (!Reverse)
                    {
                        _second.TranslationX = transX - width;
                    }
                });
                break;
        }
        var scrollAnimation = new Animation(callback, 0, 1, Easing.Linear);

        scrollAnimation.Commit(this, "scrolling", 16, (uint)ScrollDuration, Easing.Linear, repeat: () => !Reverse);
        
    }

    private static void OnScrollDurationChanged(BindableObject bindable, object oldValue, object newValue)
    {

    }
}

public enum ScrollDirection
{
    Up,
    Down,
    Left,
    Right
}