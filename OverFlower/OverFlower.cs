#if MAUI
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Animation = Microsoft.Maui.Controls.Animation;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#if WINDOWS10_0_17763_0_OR_GREATER
using Border = Microsoft.Maui.Controls.Border;
using Image = Microsoft.Maui.Controls.Image;
using Grid = Microsoft.Maui.Controls.Grid;
using Microsoft.UI.Xaml.Controls;

#endif
#endif

#if FORMS
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
#endif
using System;
using System.Linq;

namespace OverFlower;

#if MAUI
public class OverFlower : Border
#endif
#if FORMS
public class OverFlower : Frame
#endif

{

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(OverFlower), propertyChanged: OnImageSourceChanged);
    public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(nameof(ImageWidth), typeof(double), typeof(OverFlower), default(double), propertyChanged: OnImageWidthChanged);
    public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(nameof(ImageHeight), typeof(double), typeof(OverFlower), default(double), propertyChanged: OnImageHeightChanged);
    public static readonly BindableProperty ScrollDurationProperty = BindableProperty.Create(nameof(ScrollDuration), typeof(double), typeof(OverFlower), default(double), propertyChanged: OnScrollDurationChanged);
    public static readonly BindableProperty ReverseProperty = BindableProperty.Create(nameof(Reverse), typeof(bool), typeof(OverFlower), default(bool), propertyChanged: OnReverseChanged);
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
#if FORMS
    ContentPage _page;
#endif

    double _spaceEliminator = 1;
    int _initialZ;
    bool _initialized;

    public OverFlower()
    {
        InputTransparent = true;

        //TODO:
        //add code to repeat images if they are smaller than the container
        Content = new Grid
        {
            Children =
            {
                new Image
                {
                    Source = ImageSource,
                    WidthRequest = ImageWidth,
                    HeightRequest = ImageHeight,
                    Aspect = Aspect.AspectFit,
                },
                // IMPORTANT
                // DO NOT!!!! SET BACKGROUND COLOR
                // IT NESTS THE IMAGE IN A WRAPPERVIEW
                // AND BREAKS CLIPPING

                new Image
                {
                    Source = ImageSource,
                    WidthRequest = ImageWidth,
                    HeightRequest = ImageHeight,
                    Aspect = Aspect.AspectFit,
                }
            }
        };
        _container = (Grid)Content;
        _first = (Image)_container.Children.First();
        _second = (Image)_container.Children.Last();


#if MAUI
        _initialZ = _first.ZIndex;
        SizeChanged += OverFlower_SizeChanged;
#endif

#if FORMS
        HasShadow = false;
        Padding = 0;
        CornerRadius = 0;
        Margin = 0;

        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == "Renderer")
            {
                if (_page is not ContentPage)
                {
                    var parent = Parent;
                    while (parent?.Parent != null && !(parent is ContentPage))
                    {
                        parent = parent.Parent;
                    }

                    if (parent is ContentPage page)
                    {
                        page.SizeChanged += async (s, e) =>
                        {
                            Initialize();
                        };
                    }
                }
            }
        };
#endif

    }

    private void OverFlower_SizeChanged(object sender, EventArgs e)
    {
        Initialize();
    }

    void Initialize()
    {
        if (_first.WidthRequest <= 0 || _first.HeightRequest <= 0 || _second.WidthRequest <= 0 || _second.HeightRequest <= 0 || ScrollDuration <= 0 || Width <= 0 || Height <= 0)
        {
            return;
        }

        if (!_initialized)
        {
            _initialized = true;
            //TODO:
            //add code to repeat images if they are smaller than the container
            Content = new Grid
            {
                Children =
            {
                new Image
                {
                    Source = ImageSource,
                    WidthRequest = ImageWidth,
                    HeightRequest = ImageHeight,
                    Aspect = Aspect.AspectFit,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,

                },
                // IMPORTANT
                // DO NOT!!!! SET BACKGROUND COLOR
                // IT NESTS THE IMAGE IN A WRAPPERVIEW
                // AND BREAKS CLIPPING

                new Image
                {
                    Source = ImageSource,
                    WidthRequest = ImageWidth,
                    HeightRequest = ImageHeight,
                    Aspect = Aspect.AspectFit,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                }
            }
            };
            _container = (Grid)Content;
            _first = (Image)_container.Children.First();
            _second = (Image)_container.Children.Last();

#if MAUI
            _initialZ = _first.ZIndex;
#endif



#if FORMS
            HasShadow = false;
            Padding = 0;
            CornerRadius = 0;
            Margin = 0;
#endif

        }
        else
        {
            if (_first.WidthRequest != ImageWidth)
            {
                _first.WidthRequest = ImageWidth;
                _first.HeightRequest = ImageHeight;

                _second.WidthRequest = ImageWidth;
                _second.HeightRequest = ImageHeight;
            }
        }
        if (WidthRequest < Width || HeightRequest < Height)
        {
            WidthRequest = Width;
            HeightRequest = Height;

        }

        Clip = new RectangleGeometry(new Rect(0, 0, WidthRequest <= 0 ? Width : WidthRequest, HeightRequest <= 0 ? Height : HeightRequest));

        InitialOffset();
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
        Action<double> callback = null;
        switch (ScrollDirection)
        {
            case ScrollDirection.Left:
#if MAUI
                //_first.ZIndex = _second.ZIndex;
                //_second.ZIndex = _initialZ;
#endif
                callback = new Action<double>(async (d) =>
                {
                    var width = _first.WidthRequest;

                    var transX = -width * d;
                    _first.TranslationX = transX;
                    _second.TranslationX = transX + width - _spaceEliminator;


                });
                break;
            case ScrollDirection.Up:
#if MAUI
                //_first.ZIndex = _second.ZIndex;
                //_second.ZIndex = _initialZ;
#endif
                callback = new Action<double>((d) =>
                {
                    var height = _first.HeightRequest;

                    var transY = -height * d;
                    _first.TranslationY = transY;
                    _second.TranslationY = transY + height - _spaceEliminator;
                });
                break;
            case ScrollDirection.Right:
#if MAUI
                //_first.ZIndex = _second.ZIndex + 1;
#endif
                callback = new Action<double>((d) =>
                {
                    var width = _first.WidthRequest;

                    var transX = width * d;
                    _first.TranslationX = transX;
                    _second.TranslationX = transX - width + _spaceEliminator;
                });
                break;
            case ScrollDirection.Down:
#if MAUI
                //_first.ZIndex = _second.ZIndex + 1;
#endif
                callback = new Action<double>((d) =>
                {
                    var height = _first.HeightRequest;

                    var transY = height * d;
                    _first.TranslationY = transY;
                    _second.TranslationY = transY - height + _spaceEliminator;

                }); break;
            default:
                callback = new Action<double>((d) =>
                {
#if MAUI
                //_first.ZIndex = _second.ZIndex;
                //_second.ZIndex = _initialZ;
#endif
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
        _scrollAnimation = new Animation(callback, 0, 1, Easing.Linear);

        _scrollAnimation.Commit(this, "scrolling", 16, (uint)ScrollDuration, Easing.Linear, finished: (d, hasFinished) =>
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

    public void ChangeScrollDuration(double duration)
    {
        ScrollDuration = duration;
        if (!_initialized)
        {
            Initialize();
        }
    }

    public void UpdateImageWidth(double width)
    {
        if (width < WidthRequest) width = WidthRequest;
        ImageWidth = width;
        _first.WidthRequest = width;
        _second.WidthRequest = width;
        Initialize();

    }

    public void UpdateImageHeight(double height)
    {
        if (height < HeightRequest) height = HeightRequest;

        ImageHeight = height;
        _first.HeightRequest = height;
        _second.HeightRequest = height;
        Initialize();
    }

    public void UpdateImageSource(ImageSource source)
    {
        _first.Source = source;
        _second.Source = source;
        Initialize();
    }

    public void ChangeScrollDirection(ScrollDirection direction)
    {
        ScrollDirection = direction;
        Initialize();
    }

    static void OnScrollDirectionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var overFlower = (OverFlower)bindable;
        overFlower.ChangeScrollDirection((ScrollDirection)newValue);
    }


    static void OnReverseChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var overFlower = (OverFlower)bindable;
        overFlower.Reverse = (bool)newValue;
    }

    static void OnScrollDurationChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var overFlower = (OverFlower)bindable;
        overFlower.ChangeScrollDuration((double)newValue);
    }

    static void OnImageHeightChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var overFlower = (OverFlower)bindable;
        overFlower.UpdateImageHeight((double)newValue);
    }

    static void OnImageWidthChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var overFlower = (OverFlower)bindable;
        overFlower.UpdateImageWidth((double)newValue);
    }

    static void OnImageSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var overFlower = (OverFlower)bindable;
        overFlower.UpdateImageSource((ImageSource)newValue);
    }
}

public enum ScrollDirection
{
    Up,
    Down,
    Left,
    Right
}