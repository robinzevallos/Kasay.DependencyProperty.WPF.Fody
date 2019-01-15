[![NuGet Status](http://img.shields.io/nuget/v/PropertyChanged.Fody.svg?style=flat&max-age=86400)](https://www.nuget.org/packages/Kasay.DependencyProperty.WPF.Fody/)

![Icon](https://raw.githubusercontent.com/robinzevallos/Kasay.DependencyProperty.WPF.Fody/master/kasay_icon.png)
      
Implement automatically [`DependencyPropety`](https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/how-to-implement-a-dependency-property) in WPF.

## Usage

See also [Fody usage](https://github.com/Fody/Fody#usage).

### NuGet installation

Install the [Kasay.DependencyProperty.WPF.Fody NuGet package](https://www.nuget.org/packages/Kasay.DependencyProperty.WPF.Fody/):

```powershell
PM> Install-Package Kasay.DependencyProperty.WPF.Fody -Version 1.0.0	
```
### Add to FodyWeavers.xml
it's generated automatically in the second build, after the installation

Add `<Kasay.DependencyProperty.WPF/>` to [FodyWeavers.xml](https://github.com/Fody/Fody#add-fodyweaversxml)

```xml
<?xml version="1.0" encoding="utf-8"?>
<Weavers xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="FodyWeavers.xsd">
  <Kasay.DependencyProperty.WPF />
</Weavers>
```
