﻿using System;
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
        }

        private void TestViewControlLoaded(object sender, RoutedEventArgs e)
        {
            TestViewModel testViewModel = new TestViewModel();
            TestViewControl.DataContext = testViewModel;
        }

    }
}
