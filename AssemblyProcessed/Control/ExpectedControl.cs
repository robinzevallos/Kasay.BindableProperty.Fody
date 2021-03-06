﻿namespace AssemblyProcessed
{
    using System;
    using Xamarin.Forms;

    public class ExpectedControl : ContentView
    {
        public static readonly BindableProperty SomeNameProperty =
           BindableProperty.Create(
               "SomeName", 
               typeof(String),
               typeof(ExpectedControl));

        public String SomeName
        {
            get => (String)GetValue(SomeNameProperty);
            set => SetValue(SomeNameProperty, value);
        }

        public static readonly BindableProperty SomeNumberProperty =
          BindableProperty.Create(
              "SomeNumber", 
              typeof(Int32),
              typeof(ExpectedControl));

        public Int32 SomeNumber
        {
            get => (Int32)GetValue(SomeNumberProperty);
            set => SetValue(SomeNumberProperty, value);
        }

        public static readonly BindableProperty SomeConditionProperty =
          BindableProperty.Create(
              "SomeCondition", 
              typeof(Boolean), 
              typeof(ExpectedControl));

        public Boolean SomeCondition
        {
            get => (Boolean)GetValue(SomeConditionProperty);
            set => SetValue(SomeConditionProperty, value);
        }

        public ExpectedControl()
        {
            BindingContext = this;
        }
    }
}
