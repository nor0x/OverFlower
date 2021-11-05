using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OverFlower
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverFlower : ContentView
    {

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(OverFlower), propertyChanged: ImageSourceChanged);

        public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(nameof(ImageWidth), typeof(double), typeof(OverFlower), default(double));

        public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(nameof(ImageHeight), typeof(double), typeof(OverFlower), default(double));

        public static readonly BindableProperty ScrollDurationProperty = BindableProperty.Create(nameof(ScrollDuration), typeof(double), typeof(OverFlower), default(double));

        public static readonly BindableProperty ReverseProperty = BindableProperty.Create(nameof(Reverse), typeof(bool), typeof(OverFlower), default(bool));

        public static readonly BindableProperty ScrollDirectionProperty = BindableProperty.Create(nameof(ScrollDirection), typeof(ScrollDirection), typeof(OverFlower), ScrollDirection.Left);


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

        public OverFlower()
        {
            InitializeComponent();
            ScrollingWrapper.SizeChanged += async (s, e) =>
            {
                ScrollingImage.Source = ImageSource;

                if (ScrollDirection == ScrollDirection.Up || ScrollDirection == ScrollDirection.Down)
                {
                    ScrollingWrapper.Orientation = ScrollOrientation.Vertical;
                }
                else if (ScrollDirection == ScrollDirection.Left || ScrollDirection == ScrollDirection.Right)
                {
                    ScrollingWrapper.Orientation = ScrollOrientation.Horizontal;
                }
                else
                {
                    ScrollingWrapper.Orientation = ScrollOrientation.Neither;
                }

                switch (ScrollDirection)
                {
                    case ScrollDirection.Left:
                        ScrollLeft();
                        break;
                    case ScrollDirection.Up:
                        var maxY = ImageHeight - ScrollingWrapper.Height;
                        await ScrollingWrapper.ScrollToAsync(0, maxY, false);
                        ScrollUp();
                        break;
                    case ScrollDirection.Right:
                        var maxX = ImageWidth - ScrollingWrapper.Width;
                        await ScrollingWrapper.ScrollToAsync(maxX, 0, false);
                        ScrollRight();

                        break;
                    case ScrollDirection.Down:
                        ScrollDown();
                        break;
                }
            };
        }

        public void SetImageSource(ImageSource img)
        {
            ImageSource = img;
        }

        static void ImageSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is OverFlower self && newValue is ImageSource img)
                self.SetImageSource(img);

        }

        const string InitialAnimation = "initial";
        const string ReverseAnimation = "reverse";


        void ScrollRight()
        {
            var animation = new Animation(
                                callback: (scroll) =>
                                {
                                    ScrollingWrapper.ScrollToAsync(scroll, 0, animated: false);
                                },
                                start: ScrollingWrapper.ScrollX,
                                end: 0);
            animation.Commit(
                owner: this,
                name: ReverseAnimation,
                length: (uint)ScrollDuration,
                easing: Easing.Linear,
                finished: (d, b) =>
                {
                    this.AbortAnimation(ReverseAnimation);
                    ScrollLeft();
                });
        }

        void ScrollLeft()
        {
            var maxX = ImageWidth - ScrollingWrapper.Width;
            var animation = new Animation(callback: (scroll) =>
            {
                ScrollingWrapper.ScrollToAsync(scroll, 0, animated: false);
            },
            start: ScrollingWrapper.ScrollX,
            end: maxX);

            animation.Commit(
                owner: this,
                name: InitialAnimation,
                length: (uint)ScrollDuration,
                easing: Easing.Linear,
                finished: (d, b) =>
                {
                    this.AbortAnimation(InitialAnimation);
                    ScrollRight();
                });
        }

        void ScrollUp()
        {
            var animation = new Animation(callback: (scroll) =>
            {
                ScrollingWrapper.ScrollToAsync(0, scroll, animated: false);
            },
            start: ScrollingWrapper.ScrollY,
            end: 0);

            animation.Commit(
                owner: this,
                name: InitialAnimation,
                length: (uint)ScrollDuration,
                easing: Easing.Linear,
                finished: (d, b) =>
                {
                    this.AbortAnimation(InitialAnimation);
                    ScrollDown();
                });
        }

        void ScrollDown()
        {
            var maxY = ImageHeight - ScrollingWrapper.Height;

            var animation = new Animation(callback: (scroll) =>
            {
                ScrollingWrapper.ScrollToAsync(0, scroll, animated: false);
            },
            start: ScrollingWrapper.ScrollY,
            end: maxY);

            animation.Commit(
                owner: this,
                name: InitialAnimation,
                length: (uint)ScrollDuration,
                easing: Easing.Linear,
                finished: (d, b) =>
                {
                    this.AbortAnimation(InitialAnimation);
                    ScrollUp();
                });
        }

    }
}