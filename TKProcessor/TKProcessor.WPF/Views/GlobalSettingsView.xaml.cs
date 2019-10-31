using System;
using System.Collections.Generic;
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

namespace TKProcessor.WPF.Views
{
    /// <summary>
    /// Interaction logic for GlobalSettingsView.xaml
    /// </summary>
    public partial class GlobalSettingsView : UserControl
    {
        string _prevText = string.Empty;

        public GlobalSettingsView()
        {
            InitializeComponent();
        }

        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var cbTest = (ComboBox)sender;

            foreach (var item in cbTest.Items)
            {
                if (item.ToString().StartsWith(cbTest.Text))
                {
                    _prevText = cbTest.Text;
                    return;
                }
            }
            cbTest.Text = _prevText;
        }
    }
}
