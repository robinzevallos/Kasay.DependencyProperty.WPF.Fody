﻿namespace AssemblyToProcess
{
    using Kasay.DependencyProperty;
    using System;
    using System.Windows.Controls;

    public class DemoControl : UserControl
    {
        [Bind] public String SomeName { get; set; }

        [Bind] public Int32 SomeNumber { get; set; }

        [Bind] public Boolean SomeCondition { get; set; }
    }
}
