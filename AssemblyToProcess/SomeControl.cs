namespace AssemblyToProcess
{
    using Kasay.BindableProperty;
    using System;
    using Xamarin.Forms;

    [AutoBindableProperty]
    public class SomeControl : ContentView
    {
        public String SomeName { get; set; }

        public Int32 SomeNumber { get; set; }

        public Boolean SomeCondition { get; set; }
    }
}
