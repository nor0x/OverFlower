using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#if WINDOWS
using Microsoft.UI.Xaml.Controls;
#endif
using OverFlower;
using System.Diagnostics;
using Border = Microsoft.Maui.Controls.Border;

namespace SampleMaui;

public partial class MauiFlower : Border
{
    #region boilerplate
    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(MauiFlower));

    public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(nameof(ImageWidth), typeof(double), typeof(MauiFlower), default(double));

    public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(nameof(ImageHeight), typeof(double), typeof(MauiFlower), default(double));

    public static readonly BindableProperty ScrollDurationProperty = BindableProperty.Create(nameof(ScrollDuration), typeof(double), typeof(MauiFlower), default(double));

    public static readonly BindableProperty ReverseProperty = BindableProperty.Create(nameof(MauiFlower), typeof(bool), typeof(MauiFlower), default(bool));

    public static readonly BindableProperty ScrollDirectionProperty = BindableProperty.Create(nameof(ScrollDirection), typeof(ScrollDirection), typeof(MauiFlower), ScrollDirection.Left, propertyChanged: OnScrollDirectionChanged);

    private static void OnScrollDirectionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MauiFlower overflower)
        {
            if (overflower._first != null)
            {
                //overflower.InitialOffset();
                //overflower.StartAnimation();
            }
        }
    }

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

    #endregion

    Animation _scrollAnimation;
    double _spaceEliminator = 1;

    public MauiFlower()
	{
		InitializeComponent();
        ScrollDirection = ScrollDirection.Up;
        ScrollDuration = 4000;
        InitialOffset();
    }

    public void Start()
	{

#if WINDOWS10_0_17763_0_OR_GREATER

        Debug.WriteLine("MAUIFLOWER:");

        if (Handler is BorderHandler bh && bh.PlatformView is Panel p)
        {
            foreach (var c in p.Children)
            {
                Debug.WriteLine("c: " + c.GetType());
                if (c is Panel pa && pa.Children is UIElementCollection uec)
                {
                    foreach (var ue in uec)
                    {
                        Debug.WriteLine("uec: " + ue.GetType());
                    }
                }
            }
            if (_container.Handler is LayoutHandler lh && lh.PlatformView is LayoutPanel g)
            {
                g.Clip = null;
            }
            if (_first.Handler is ImageHandler ih && ih.PlatformView is Microsoft.UI.Xaml.Controls.Image i)
            {
                i.Clip = null;
            }
            if (_second.Handler is ImageHandler ih2 && ih2.PlatformView is Microsoft.UI.Xaml.Controls.Image i2)
            {
                i2.Clip = null;
            }
            p.Clip = null;
        }

#endif

        StartAnimation();
	}

    void InitialOffset()
    {

        //initial offset
        _first.TranslationX = 0;
        _first.TranslationY = 0;
        switch (ScrollDirection)
        {
            case ScrollDirection.Left:
                _second.TranslationY = 0;
                _second.TranslationX = -_second.WidthRequest;

                break;
            case ScrollDirection.Up:
                _second.TranslationX = 0;
                _second.TranslationY = -_second.HeightRequest;

                break;
            case ScrollDirection.Right:
                _second.TranslationY = 0;
                _second.TranslationX = -_second.WidthRequest;

                break;
            case ScrollDirection.Down:
                _second.TranslationX = 0;
                _second.TranslationY = -_second.HeightRequest;

                break;
        }
    }

    void StartAnimation()
    {
        this.AbortAnimation("scfrolling");
        Debug.WriteLine("first: z " + _first.ZIndex + " " + _second.ZIndex);

        Action<double> callback = null;
        switch (ScrollDirection)
        {
            case ScrollDirection.Left:
                //_first.ZIndex = _second.ZIndex;
                //_second.ZIndex = _initialZ;
                callback = new Action<double>((d) =>
                {
                    var width = _first.WidthRequest;

                    var transX = -width * d;
                    _first.TranslationX = transX;
                    _second.TranslationX = transX + width - _spaceEliminator;


                });
                break;
            case ScrollDirection.Up:
                //_first.ZIndex = _second.ZIndex;
                //_second.ZIndex = _initialZ;
                callback = new Action<double>((d) =>
                {
                    var height = _first.HeightRequest;

                    var transY = -height * d;
                    _first.TranslationY = transY;
                    _second.TranslationY = transY + height - _spaceEliminator;

                    Debug.WriteLine("D: " + d.ToString("0.00"));
                    Debug.WriteLine("height: " + height);
                    Debug.WriteLine("transy: " + transY);
                    Debug.WriteLine("_first: " + _first.TranslationY);
                    Debug.WriteLine("_second: " + _second.TranslationY);

                });
                break;
            case ScrollDirection.Right:
                //_first.ZIndex = _second.ZIndex + 1;
                callback = new Action<double>((d) =>
                {
                    var width = _first.WidthRequest;
                    var transX = width * d;
                    _first.TranslationX = transX;
                    _second.TranslationX = transX - width + _spaceEliminator;
                });
                break;
            case ScrollDirection.Down:
                //_first.ZIndex = _second.ZIndex + 1;
                callback = new Action<double>((d) =>
                {
                    Debug.WriteLine("D: " + d.ToString("0.00"));
                    var height = _first.HeightRequest;
                    Debug.WriteLine("height: " + height);
                    double transY = height * d;
                    Debug.WriteLine("transy: " + transY);
                    _first.TranslationY = transY;
                    _second.TranslationY = transY - height + _spaceEliminator;
                    Debug.WriteLine("_first: " + _first.TranslationY);
                    Debug.WriteLine("_second: " + _second.TranslationY);

                }); break;
            default:
                callback = new Action<double>((d) =>
                {
                //_first.ZIndex = _second.ZIndex;
                //_second.ZIndex = _initialZ;
                    callback = new Action<double>((d) =>
                    {
                    var width = _first.WidthRequest;

                        var transX = -width * d;
                        _first.TranslationX = transX;
                        _second.TranslationX = transX + width - _spaceEliminator;
                    });
                });
                break;
        }
        _scrollAnimation = new Animation(callback, 0.1, 1.1, Easing.Linear);

        _scrollAnimation.Commit(this, "scfrolling", 16, (uint)ScrollDuration, Easing.Linear, finished: (d, hasFinished) =>
        {
            if (!hasFinished)
            {
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
        repeat: () => !Reverse);
    }
}