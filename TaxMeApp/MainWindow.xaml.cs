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

        private Loader loader;

        public MainWindow()
        {

            loader = new Loader();



            InitializeComponent();


            TestViewControl.DataContext = loader.testVM;

            //TestViewModel testViewModel = new TestViewModel();
            //TestViewControl.DataContext = testViewModel;

        }

        private void TestViewControl_Loaded(object sender, RoutedEventArgs e)
        {

            //TestViewControl.DataContext = loader.testVM;
        }
    }
}
