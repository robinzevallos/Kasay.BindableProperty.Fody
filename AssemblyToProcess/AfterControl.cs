namespace AssemblyToProcess
{
    using Kasay.BindableProperty;
    using System;
    using Xamarin.Forms;

    [AutoBindableProperty]
    public class AfterControl : ContentView
    {
        public String SomeName { get; set; }

        public Int32 SomeNumber { get; set; }

        public Boolean SomeCondition { get; set; }
    }
}
