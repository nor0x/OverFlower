using Microsoft.Maui.Controls;
using OverFlower;
using System.Diagnostics;

namespace SampleMaui
{
    public partial class MainPage : ContentPage
    {
        Random _rand;
        public MainPage()
        {
            InitializeComponent();
            _rand = new();
            AddMarkupBorder();
            myimg.Loaded += Myimg_Loaded;
            myimg.PropertyChanged += Myimg_PropertyChanged;
            
        }

        private void Myimg_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine("XAML property changed: " + e.PropertyName);
        }

        private void Myimg_Loaded(object? sender, EventArgs e)
        {
            Debug.WriteLine("XAML LOADED: \n" + myimg.GetInfo());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            var x = OverFlower.Extensions.GeneratePropertiesDictionary(myborder);
            var y = OverFlower.Extensions.GeneratePropertiesDictionary(mygrid);
            var z = OverFlower.Extensions.GeneratePropertiesDictionary(myimg);

            var ani1 = new Animation((d) =>
            {
                var height = myimg.HeightRequest;

                var transY = height * d;
                myimg.TranslationY = transY;
                myimg2.TranslationY = transY - height;
            }, 0, 1, Easing.Linear);
            
            ani1.Commit(this, "helloworld", 14, 4000, Easing.Linear);

            var ani2 = new Animation((d) =>
            {
                var height = _markupImg1.HeightRequest;

                double transY = height * d;
                _markupImg1.TranslationY = transY;
                _markupImg2.TranslationY = transY - height;
            }, 0, 1, Easing.Linear);

            ani2.Commit(this, "hellohworld", 14, 4000, Easing.Linear);
        
            mauiflower.Start();

        }
        Image _markupImg1;
        Image _markupImg2;

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            mylabel.Text = "mylabel: " + mylabel.ZIndex;
        }

        void AddMarkupBorder()
        {
            var border = new Border()
            {
                WidthRequest = 400,
                HeightRequest = 500,
                StrokeThickness = 0,
                Background = Colors.LightGreen,
                Content = new Grid()
                {
                    Children =
                    {
                        new Image
                        {
                            Source = "first.jpg",
                            WidthRequest = 987,
                            HeightRequest = 1480,
                            Aspect = Aspect.AspectFit,
                            Background = Colors.Magenta,
                        },
                        new Image
                        {
                            Source = "second.jpg",
                            WidthRequest = 987,
                            HeightRequest = 1480,
                            Aspect = Aspect.AspectFit,
                            TranslationY = -1480,
                            Background = Colors.Magenta,
                        },
                    }
                }
            };
            if(border.Content is Grid g && g.Children.First() is Image fi && g.Children.Last() is Image la)
            {
                _markupImg1 = fi;
                _markupImg2 = la;
            }
            outgrid.Add(border, 0, 0);
        }
    }
}