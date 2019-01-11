using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Client
{
    [Kasay.DependencyProperty.WPF.AutoDependencyProperty]
    public class Demo : UserControl
    {
        public String Lalo { get; set; }

        public Int32 Memo { get; set; }

    }
}
