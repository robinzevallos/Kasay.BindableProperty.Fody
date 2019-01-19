namespace Sample
{
    using System;
    using Kasay.BindableProperty;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    [AutoBindableProperty]
    public partial class DemoControl : ContentView
	{
        public String Text { get; set; }

        public Color TextColor { get; set; }

        public DemoControl ()
		{
			InitializeComponent ();
		}
	}
}