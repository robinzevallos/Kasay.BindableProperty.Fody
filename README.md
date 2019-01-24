[![NuGet Status](http://img.shields.io/nuget/v/Kasay.BindableProperty.Fody.svg?style=flat&max-age=86400)](https://www.nuget.org/packages/Kasay.BindableProperty.Fody/)

![Icon](https://raw.githubusercontent.com/robinzevallos/Kasay.BindableProperty.Fody/master/kasay_icon.png)

Implement automatically [`BindableProperty`](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/xaml/bindable-properties) in Xamarin.Forms.

## Usage

See also [Fody usage](https://github.com/Fody/Fody#usage).

### NuGet installation

Install the [Kasay.BindableProperty.Fody NuGet package](https://www.nuget.org/packages/Kasay.BindableProperty.Fody/):

```powershell
PM> Install-Package Kasay.BindableProperty.Fody -Version 1.0.3	
```

### Add to FodyWeavers.xml
** it's generated automatically after build.

Add `<Kasay.BindableProperty/>` to [FodyWeavers.xml](https://github.com/Fody/Fody#add-fodyweaversxml)

```xml
<?xml version="1.0" encoding="utf-8"?>
<Weavers xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="FodyWeavers.xsd">
  <Kasay.BindableProperty />
</Weavers>
```

## Overview

Before code:

```csharp
public class DemoControl : UserControl
{
  [Bind] public String SomeName { get; set; }

  [Bind] public Int32 SomeNumber { get; set; }

  [Bind] public Boolean SomeCondition { get; set; }
}
```

What gets compiled:

```csharp
public class DemoControl : UserControl
{
  public static readonly BindableProperty SomeNameProperty
    = BindableProperty.Create(
        "SomeName",
        typeof(String),
        typeof(DemoControl));

  public String SomeName
  {
      get => (String)GetValue(SomeNameProperty);
      set => SetValue(SomeNameProperty, value);
  }
  
  public static readonly BindableProperty SomeNumberProperty
    = BindableProperty.Create(
        "SomeName",
        typeof(Int32),
        typeof(DemoControl));

  public Int32 SomeNumber
  {
      get => (Int32)GetValue(SomeNumberProperty);
      set => SetValue(SomeNumberProperty, value);
  }
    
  public static readonly BindableProperty SomeConditionProperty
    = BindableProperty.Create(
        "SomeCondition",
        typeof(Boolean),
        typeof(DemoControl));

  public Boolean SomeCondition
  {
      get => (Boolean)GetValue(SomeConditionProperty);
      set => SetValue(SomeConditionProperty, value);
  }

  public DemoControl()
  {
      DataContext = this;
  }
}
```
The implementation of BindableProperty in the generation of custom controls in Xamarin.Forms always leaves us with reduntant and repetitive code, but all this can be avoided using the Bind attribute of [Kasay.BindableProperty.Fody](https://www.nuget.org/packages/Kasay.BindableProperty.Fody/).

### [Sample](https://github.com/robinzevallos/Sample.BindableProperty)
