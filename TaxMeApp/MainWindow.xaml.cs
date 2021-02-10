using System.Windows;
using TaxMeApp.Helpers;

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

            AutoUpdater autoUpdater = new AutoUpdater();

            // Turn this on only for production version:
            //autoUpdater.Update();

            if (autoUpdater.RestartRequired)
            {
                autoUpdater.Restart();
            }
            else
            {

                loader = new Loader();

                InitializeComponent();

                DataContext = loader.MainVM;
            }

        }


    }
}
