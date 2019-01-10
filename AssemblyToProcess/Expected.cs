using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AssemblyToProcess
{
    public class Expected : UserControl
    {
        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty2 =
            DependencyProperty.Register("Name2", typeof(String), typeof(Expected), null);

        public String Name2
        {
            get { return (String)GetValue(NameProperty2); }
            set { SetValue(NameProperty2, value); }
        }



    }
}
