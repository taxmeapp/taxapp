using System.Windows;


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
        }

        private void TestViewControlLoaded(object sender, RoutedEventArgs e)
        {
            TestViewModel testViewModel = new TestViewModel();
            TestViewControl.DataContext = testViewModel;
        }

    }
}
