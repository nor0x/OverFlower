
<img src="https://raw.githubusercontent.com/nor0x/OverFlower/main/imgs/icon.png" width="100px" />

# OverFlower
[![.NET](https://github.com/nor0x/OverFlower/actions/workflows/dotnet.yml/badge.svg)](https://github.com/nor0x/OverFlower/actions/workflows/dotnet.yml)
[![](https://img.shields.io/nuget/dt/OverFlower)](https://www.nuget.org/packages/OverFlower)

a simple control to display scrolling overflow content!

<img src="https://raw.githubusercontent.com/nor0x/OverFlower/main/imgs/appstore.gif" />


| Platform      | Support       |
| :-------------: |:-------------:|
| Android      | ✔      |
| iOS        | ✔      |
| UWP| ⏳      |

## Getting started

OverFlower is available on NuGet and GitHub

[https://www.nuget.org/packages/OverFlower ](https://www.nuget.org/packages/OverFlower )
[https://github.com/nor0x/OverFlower/packages](https://github.com/nor0x/OverFlower/packages)

Start by adding a the XAML namespace  `xmlns:over="clr-namespace:OverFlower;assembly=OverFlower"` and use it like this:
```xml
<over:OverFlower
    ImageSource="yourimage.png"
    ImageWidth="2500"
    ImageHeight="1000"
    ScrollDirection="Left"
    ScrollDuration="20000" />
```

## API

### `ImageSource`
is a regular Xamarin.Forms ImageSource and 
### `ImageWidth`
width of the overflow image
### `ImageHeight`
height of the overflow image
### `ScrollDirection`
scrolling direction of the overflow image
### `ScrollDuration`
duration of the scrolling operation (in milliseconds)
### `Reverse`
coming soon™️


## Demo


## Contribution
feel free to create issues and PRs 👋