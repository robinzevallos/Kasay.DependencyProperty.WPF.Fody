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
        public Expected()
        {
            //((FrameworkElement)Content).DataContext = this;
        }

        public static readonly DependencyProperty NameProperty2 =
            DependencyProperty.Register("Name2", typeof(String), typeof(Expected));

        public String Name2
        {
            get { return (String)GetValue(NameProperty2); }
            set { SetValue(NameProperty2, value); }
        }


        public int Pollo
        {
            get { return (int)GetValue(PolloProperty); }
            set { SetValue(PolloProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pollo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PolloProperty =
            DependencyProperty.Register("Pollo", typeof(int), typeof(Expected));





    }
}
