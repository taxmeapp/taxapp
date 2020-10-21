using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaxMeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Intro to panels, how to set up basics of XAML
        //https://www.wpf-tutorial.com/panels/introduction-to-wpf-panels/

        public MainWindow()
        {
            InitializeComponent();

            changeSomething();

        }

        // Just an example of how to access named things
        private void changeSomething()
        {

            MinIncomeInput.Text = "changing this programatically";

        }

        // Auto generated event handler as an example
        // Double clicking the item in the .xaml usually gets you the event handler you want
        // Otherwise you can look them up in the docs
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            Slider slider = sender as Slider;

            // Use this to write to output in VS rather than "Console.WriteLine"
            Trace.WriteLine("Slider Value is.." + slider.Value);

        }
    }
}
