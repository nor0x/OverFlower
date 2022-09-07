using OverFlower;

namespace SampleMaui
{
    public partial class MainPage : ContentPage
    {
        Random _rand;
        public MainPage()
        {
            InitializeComponent();
            _rand = new();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            /*
            var values = Enum.GetValues(typeof(ScrollDirection));
            var scrollDir = (ScrollDirection)values.GetValue(_rand.Next(values.Length));
            MyFlower.ScrollDirection = scrollDir;
            MyFlower.Reverse = _rand.NextDouble() > 0.5;
            MyFlower.ScrollDuration = _rand.Next(100, 6000);
            */
            myimg.Rotation = Convert.ToDouble(zValue.Text);
            myimg.RotationX = Convert.ToDouble(xValue.Text);
            myimg.RotationY = Convert.ToDouble(yValue.Text);
        }
    }
}