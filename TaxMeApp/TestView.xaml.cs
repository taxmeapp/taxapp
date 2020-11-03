using System;
using System.Windows.Controls;

namespace TaxMeApp
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestView : UserControl
    {
        public TestView()
        {
            InitializeComponent();
        }

        private void PolicyRateSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            Console.WriteLine("New rate: {0}", e.NewValue);    
        }

        private void CartesianChart_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void PolicyRateSlider_ValueChanged_1(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
