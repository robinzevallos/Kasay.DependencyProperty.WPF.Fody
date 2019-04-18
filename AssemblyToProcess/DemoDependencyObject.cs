namespace AssemblyToProcess.pueblo
{
    using System;
    using System.Windows;
    using Kasay;

    public class DemoDependencyObject : DependencyObject
    {
        [Bind] public String SomeName { get; set; }

        [Bind] public Int32 SomeNumber { get; set; }

        [Bind] public Boolean SomeCondition { get; set; }
    }
}
