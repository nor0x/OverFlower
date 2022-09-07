#if MAUI
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Animation = Microsoft.Maui.Controls.Animation;
#endif

#if FORMS
using Xamarin.Forms;
#endif
using System;
using System.Diagnostics;

namespace OverFlower;

#if MAUI
public class OverFlower : Border
#endif
#if FORMS
public class OverFlower : Frame
#endif
{

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(OverFlower));

    public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(nameof(ImageWidth), typeof(double), typeof(OverFlower), default(double));

    public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(nameof(ImageHeight), typeof(double), typeof(OverFlower), default(double));

    public static readonly BindableProperty ScrollDurationProperty = BindableProperty.Create(nameof(ScrollDuration), typeof(double), typeof(OverFlower), default(double));

    public static readonly BindableProperty ReverseProperty = BindableProperty.Create(nameof(Reverse), typeof(bool), typeof(OverFlower), default(bool));

    public static readonly BindableProperty MirrorProperty = BindableProperty.Create(nameof(Mirror), typeof(bool), typeof(OverFlower), default(bool));

    public static readonly BindableProperty ScrollDirectionProperty = BindableProperty.Create(nameof(ScrollDirection), typeof(ScrollDirection), typeof(OverFlower), ScrollDirection.Left, propertyChanged: OnScrollDirectionChanged);


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

    public bool Mirror
    {
        get { return (bool)GetValue(MirrorProperty); }
        set { SetValue(MirrorProperty, value); }
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
    Image _second;
    Grid _container;
    Animation _scrollAnimation;
    double _spaceEliminator = 1;
    int _initialZ;

    double _width;
    double _height;
    public OverFlower()
    {
        _container = new()
        {
            WidthRequest = WidthRequest,
            HeightRequest = HeightRequest,
#if MAUI
            Background = Colors.Maroon,
#endif
#if FORMS
            Background = Color.Maroon,
#endif
            IsClippedToBounds = true,
        };
        Content = _container;
#if MAUI
        Loaded += OverFlower_Loaded;
#endif
#if FORMS
        HasShadow = false;
        Padding = 0;
        CornerRadius = 0;
        Margin = 0;
        SizeChanged += OverFlower_Loaded;
#endif
    }

    private void OverFlower_Loaded(object sender, EventArgs e)
    {
        Init();
    }

    void Init()
    {
        //TODO:
        //add code to repeat images if they are smaller than the container
        _first = new Image
        {
            Source = "first.jpg",
            WidthRequest = ImageWidth <= this.Width ? this.Width : ImageWidth,
            HeightRequest = ImageHeight <= this.Height ? this.Height : ImageHeight,
            Aspect = Aspect.AspectFit,
#if MAUI
            Background = Colors.Magenta,
#endif
        };
        _second = new Image
        {
            Source = "second.jpg",
            WidthRequest = ImageWidth <= this.Width ? this.Width : ImageWidth,
            HeightRequest = ImageHeight <= this.Height ? this.Height : ImageHeight,
            Aspect = Aspect.AspectFit,

#if MAUI
            Background = Colors.CornflowerBlue,
#endif
        };

        if (Mirror)
        {
            if (ScrollDirection == ScrollDirection.Up || ScrollDirection == ScrollDirection.Down)
            {
                _second.RotationX = 180;
            }
            if (ScrollDirection == ScrollDirection.Left || ScrollDirection == ScrollDirection.Right)
            {
                _second.RotationY = 180;
            }
        }

#if MAUI

#endif

        _container.Children.Add(_first);
        _container.Children.Add(_second);
        Debug.WriteLine("first: " + _first.WidthRequest + " " + _first.HeightRequest);
        Debug.WriteLine("secondns: " + _second.WidthRequest + " " + _second.HeightRequest);
#if MAUI
        _initialZ = _first.ZIndex;
#endif

        InitialOffset();
        StartAnimation();

    }

    void InitialOffset()
    {
#if MAUI
        Debug.WriteLine("initial offset WIDTH: " + _second.Width + " REQUEST: " + _second.WidthRequest + " DESIRED: " + _second.DesiredSize.Width);
        Debug.WriteLine("initial offset HEIGHT: " + _second.Height + " REQUEST: " + _second.HeightRequest + " DESIRED: " + _second.DesiredSize.Height);
#endif
        //initial offset
        _first.TranslationX = 0;
        _first.TranslationY = 0;
        switch (ScrollDirection)
        {
            case ScrollDirection.Left:
                _second.TranslationY = 0;
#if MAUI
                _second.TranslationX = -_second.DesiredSize.Width;
#endif
#if FORMS
                _second.TranslationX = -_second.Width;
#endif

                break;
            case ScrollDirection.Up:
                _second.TranslationX = 0;
#if MAUI
                _second.TranslationY = -_second.DesiredSize.Height;
#endif
#if FORMS
                _second.TranslationY = -_second.Height;

#endif
                break;
            case ScrollDirection.Right:
                _second.TranslationY = 0;
#if MAUI
                _second.TranslationX = -_second.DesiredSize.Width;
#endif
#if FORMS
                _second.TranslationX = -_second.Width;

#endif
                break;
            case ScrollDirection.Down:
                _second.TranslationX = 0;
#if MAUI
                _second.TranslationY = -_second.DesiredSize.Height;
#endif
#if FORMS
                _second.TranslationY = -_second.Height;

#endif
                break;
        }

        Debug.WriteLine("SECOND OFFSET: " + _second.TranslationX + " " + _second.TranslationY);
    }

    void StartAnimation()
    {
        this.AbortAnimation("scrolling");

        Action<double> callback = null;
        switch (ScrollDirection)
        {
            case ScrollDirection.Left:
#if MAUI
                _first.ZIndex = _second.ZIndex;
                _second.ZIndex = _initialZ;
#endif
                callback = new Action<double>((d) =>
                {
#if MAUI
                    var width = _first.DesiredSize.Width;
#endif
#if FORMS
                    var width = _first.Width;
#endif
                    var transX = -width * d;
                    _first.TranslationX = transX;
                    _second.TranslationX = transX + width - _spaceEliminator;
                    Debug.WriteLine("left first " + _first.RotationY);
                    Debug.WriteLine("left second " + _second.RotationY);

                });
                break;
            case ScrollDirection.Up:
#if MAUI
                _first.ZIndex = _second.ZIndex;
                _second.ZIndex = _initialZ;
#endif
                callback = new Action<double>((d) =>
                {
#if MAUI
                    var height = _first.DesiredSize.Height;
#endif
#if FORMS
                    var height = _first.Height;
#endif
                    var transY = -height * d;
                    _first.TranslationY = transY;
                    _second.TranslationY = transY + height - _spaceEliminator;
                });
                break;
            case ScrollDirection.Right:
#if MAUI
                _first.ZIndex = _second.ZIndex + 1;
#endif
                callback = new Action<double>((d) =>
                {
#if MAUI
                    var width = _first.DesiredSize.Width;
#endif
#if FORMS
                    var width = _first.Width;
#endif
                    var transX = width * d;
                    _first.TranslationX = transX;
                    _second.TranslationX = transX - width + _spaceEliminator;
                    Debug.WriteLine("right");
                });
                break;
            case ScrollDirection.Down:
#if MAUI
                _first.ZIndex = _second.ZIndex + 1;
#endif
                callback = new Action<double>((d) =>
                {
#if MAUI
                    var height = _first.DesiredSize.Height;
#endif
#if FORMS
                    var height = _first.Height;
#endif
                    var transY = height * d;
                    _first.TranslationY = transY;
                    _second.TranslationY = transY - height + _spaceEliminator;

                }); break;
            default:
                callback = new Action<double>((d) =>
                {
#if MAUI
                _first.ZIndex = _second.ZIndex;
                _second.ZIndex = _initialZ;
#endif
                    callback = new Action<double>((d) =>
                    {
#if MAUI
                    var width = _first.DesiredSize.Width;
#endif
#if FORMS
                        var width = _first.Width;
#endif
                        var transX = -width * d;
                        _first.TranslationX = transX;
                        _second.TranslationX = transX + width - _spaceEliminator;
                    });
                });
                break;
        }
        _scrollAnimation = new Animation(callback, 0, 1, Easing.Linear);

        _scrollAnimation.Commit(this, "scrolling", 16, (uint)ScrollDuration, Easing.Linear, finished: (d, hasFinished) =>
        {
            if (!hasFinished)
            {
                if(Mirror)
                {
                    if (ScrollDirection == ScrollDirection.Up || ScrollDirection == ScrollDirection.Down)
                    {
                        _first.RotationX = 180;
                        _second.RotationX = 0;
                    }
                    if (ScrollDirection == ScrollDirection.Left || ScrollDirection == ScrollDirection.Right)
                    {
                        Debug.WriteLine("HAS ENDED: " + _first.RotationY + " " + _second.RotationY);
                        if(_first.RotationY == 0)
                        {
                            _first.RotationY = 180;
                            _second.RotationY = 0;
                        }
                        else if(_first.RotationY == 180)
                        {
                            _first.RotationY = 0;
                            _second.RotationY = 180;
                        }

                    }
                    InitialOffset();
                    StartAnimation();
                }
                if (Reverse)
                {
                    switch (ScrollDirection)
                    {
                        case ScrollDirection.Left:
                            ScrollDirection = ScrollDirection.Right;
                            break;
                        case ScrollDirection.Up:
                            ScrollDirection = ScrollDirection.Down;
                            break;
                        case ScrollDirection.Right:
                            ScrollDirection = ScrollDirection.Left;
                            break;
                        case ScrollDirection.Down:
                            ScrollDirection = ScrollDirection.Up;
                            break;
                    }
                }
            }
        },
        repeat: () =>
        {
            if(Reverse)
            {
                return false;
            }
            else
            {
                if(Mirror)
                {
                    return false;
                }
                return true;
            }
        });
    }


    private static void OnScrollDirectionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is OverFlower overflower)
        {
            if (overflower._first != null)
            {
                overflower.InitialOffset();
                overflower.StartAnimation();
            }
        }
    }
}

public enum ScrollDirection
{
    Up,
    Down,
    Left,
    Right
}