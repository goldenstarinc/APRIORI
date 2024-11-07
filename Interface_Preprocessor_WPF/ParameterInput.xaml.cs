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
using System.Windows.Shapes;

namespace Interface_Preprocessor_WPF
{
    /// <summary>
    /// Interaction logic for ParameterInput.xaml
    /// </summary>
    public partial class ParameterInput : Window
    {
        public int value { get; private set; }

        public ParameterInput()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(TB.Text, out int value)) { throw new Exception("Параметр должен быть целочисленным."); }

                if (value < 1) { throw new Exception("Параметр не может быть меньше единицы."); }

                this.value = value;

                this.Close();
            }
            catch (Exception ex)
            {
                // Обработка исключений и вывод сообщения об ошибке
                CustomMessageBox customMessageBox = new CustomMessageBox($"{ex.Message}");
                customMessageBox.ShowDialog();
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TB.Text == "Например: 1")
            {
                TB.Text = "";
                TB.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TB.Text))
            {
                TB.Text = "Например: 1";
                TB.Foreground = Brushes.Gray;
            }
        }
    }
}
