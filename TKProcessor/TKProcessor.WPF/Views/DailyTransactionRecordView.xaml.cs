using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace TKProcessor.WPF.Views
{
    /// <summary>
    /// Interaction logic for DailyTransactionRecordView.xaml
    /// </summary>
    public partial class DailyTransactionRecordView : UserControl
    {
        public DailyTransactionRecordView()
        {
            InitializeComponent();
        }

        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 3)
                e.Handled = false;

            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = (sender as TextBox).Text;

            if (text.Length > 3)
                e.Handled = false;

            e.Handled = !IsTextAllowed(text);
        }
    }
}
