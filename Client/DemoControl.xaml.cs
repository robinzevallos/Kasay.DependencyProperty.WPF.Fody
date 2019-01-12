namespace Client
{
    using Kasay.DependencyProperty.WPF;
    using System;
    using System.Windows.Controls;

    [AutoDependencyProperty]
    public partial class DemoControl : UserControl
    {
        public String TextButton { get; set; }

        public Int32 FontSizeButton { get; set; }

        public DemoControl()
        {
            InitializeComponent();
        }
    }
}
