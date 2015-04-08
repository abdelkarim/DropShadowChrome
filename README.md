# Overview
***DropShadowChrome*** is a library that allows the user to apply a drop-shadow to a window.

![image](https://cloud.githubusercontent.com/assets/1153480/7040224/abf34dd0-ddc4-11e4-9e0c-a1597c979d90.png)

# Features
The **DropShadowChrome** class exposes two properties:
* ShadowBrush: used to render the surrounding border as well as the shadow.
* Density: affects the opacity of the surrounding border and the shadow.

# Installation
To install the library you can directly grab the binaries from the **Releases** section, or use the following nuget package: https://www.nuget.org/packages/DropShadowChrome

# Usage
Below is the XAML snippet used to render the above screenshot:
``` xml
<lib:DropShadowChrome.DropShadowChrome>
    <lib:DropShadowChrome ShadowBrush="DarkGreen" Density="0.6"/>
</lib:DropShadowChrome.DropShadowChrome>
```
