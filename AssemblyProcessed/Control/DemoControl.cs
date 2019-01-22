namespace AssemblyProcessed
{
    using Kasay.BindableProperty;
    using System;
    using Xamarin.Forms;

    public class DemoControl : ContentView
    {
        [Bind] public String SomeName { get; set; }

        [Bind] public Int32 SomeNumber { get; set; }

        [Bind] public Boolean SomeCondition { get; set; }
    }
}
