using Kasay.DependencyProperty.WPF;
using System;
using System.Windows.Controls;

namespace AssemblyToProcess
{
    [AutoDependencyProperty]
    public class DemoPUTO : UserControl
    {
        public String Demo { get; set; }
    }
}
