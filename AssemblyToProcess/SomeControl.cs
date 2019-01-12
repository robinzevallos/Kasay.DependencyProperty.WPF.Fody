namespace AssemblyToProcess
{
    using Kasay.DependencyProperty.WPF;
    using System;
    using System.Windows.Controls;

    [AutoDependencyProperty]
    public class SomeControl : UserControl
    {
        public String SomeName { get; set; }

        public Int32 SomeNumber { get; set; }

        public Boolean SomeCondition { get; set; }
    }
}
