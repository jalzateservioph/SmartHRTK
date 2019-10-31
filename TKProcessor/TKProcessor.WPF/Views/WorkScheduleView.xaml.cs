using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for WorkScheduleView.xaml
    /// </summary>
    public partial class WorkScheduleView : UserControl
    {
        public WorkScheduleView()
        {
            InitializeComponent();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = (ComboBox)sender;


            Popup popup = comboBox.Template.FindName("PART_Popup", comboBox) as Popup;

            if (comboBox.Template.FindName("PART_EditableTextBox", comboBox) is TextBox textBox)

            {

                textBox.TextChanged += delegate

                {

                    popup.IsOpen = true;

                    comboBox.Items.Filter += a =>

                    {

                        if (a.ToString().StartsWith(textBox.Text))

                        {

                            return true;

                        }

                        return false;

                    };

                };

            }
        }
    }
}
