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
using System.Windows.Shapes;

namespace TaxMeApp.dialog
{
    /// <summary>
    /// Interaction logic for DialogBox.xaml
    /// </summary>
    public partial class DialogBox : Window
    {
        public DialogBox()
        {
            InitializeComponent();
        }

        public bool Saved { get; private set; } = false;
        public string Filename { get; private set; } = "";

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Saved = true;
            Filename = FilenameSaveAs.Text;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Saved = false;
            Filename = "";
            Close();
        }

        private void FilenameSaveAs_TextChanged(object sender, TextChangedEventArgs e)
        {

            string text = FilenameSaveAs.Text;
            WarningText.Visibility = Visibility.Hidden;

            if (text == "")
            {
                SaveBtn.IsEnabled = false;
                return;
            }

            if (text.All(c => Char.IsLetterOrDigit(c) || c == '_' || c == '(' || c == ')' || c == ' ')){
                SaveBtn.IsEnabled = true;
                return;
            }

            SaveBtn.IsEnabled = false;
            WarningText.Visibility = Visibility.Visible;
            return;

        }
    }
}
