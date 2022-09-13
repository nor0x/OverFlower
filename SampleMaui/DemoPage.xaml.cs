namespace SampleMaui;

public partial class DemoPage : ContentPage
{
    public DemoPage()
    {
        InitializeComponent();
        DirectionPicker.SelectedIndex = 0;
    }

    private void Image0Tapped(object sender, EventArgs e)
    {
        DemoOverFlower.ImageSource = "monsters.png";
    }
    private void Image1Tapped(object sender, EventArgs e)
    {
        DemoOverFlower.ImageSource = "pattern.jpg";
    }
    private void Image2Tapped(object sender, EventArgs e)
    {
        DemoOverFlower.ImageSource = "choco.png";
    }
    private void Image3Tapped(object sender, EventArgs e)
    {
        DemoOverFlower.ImageSource = "flatiron.jpg";
    }
    private void Image4Tapped(object sender, EventArgs e)
    {
        DemoOverFlower.ImageSource = "icons.png";
    }
    private void Image5Tapped(object sender, EventArgs e)
    {
        DemoOverFlower.ImageSource = "pano.jpg";
    }

    private void DirectionPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (DirectionPicker.SelectedIndex)
        {
            case 0:
                DemoOverFlower.ScrollDirection = OverFlower.ScrollDirection.Left;
                break;
            case 1:
                DemoOverFlower.ScrollDirection = OverFlower.ScrollDirection.Up;
                break;
            case 2:
                DemoOverFlower.ScrollDirection = OverFlower.ScrollDirection.Right;
                break;
            case 3:
                DemoOverFlower.ScrollDirection = OverFlower.ScrollDirection.Down;
                break;
            default:
                DemoOverFlower.ScrollDirection = OverFlower.ScrollDirection.Left;
                break;

        }
    }

    void ReverseSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        if (DemoOverFlower != null)
        {
            DemoOverFlower.Reverse = e.Value;
        }
    }

    private void WidthSavedClicked(object sender, EventArgs e)
    {
        DemoOverFlower.WidthRequest = Convert.ToDouble(WidthEntry.Text);
    }

    private void HeightSavedClicked(object sender, EventArgs e)
    {
        DemoOverFlower.HeightRequest = Convert.ToDouble(HeightEntry.Text);
    }

    private void ImageWidthSavedClicked(object sender, EventArgs e)
    {
        DemoOverFlower.ImageWidth = Convert.ToDouble(ImageWidthEntry.Text);
    }

    private void ImageHeightSavedClicked(object sender, EventArgs e)
    {
        DemoOverFlower.ImageHeight = Convert.ToDouble(ImageHeightEntry.Text);
    }

    private void DurationSavedClicked(object sender, EventArgs e)
    {
        DemoOverFlower.ScrollDuration = Convert.ToDouble(DurationEntry.Text);
    }

    private void ImageUrlSavedClicked(object sender, EventArgs e)
    {
        DemoOverFlower.ImageSource = ImageSource.FromUri(new Uri(ImageEntry.Text));
    }
}