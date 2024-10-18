using DataProcessor;
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
    /// Логика взаимодействия для ConstructRule.xaml
    /// </summary>
    public partial class ConstructRule : Window
    {
        private ExcelFile _excelFile;
        public ConstructRule(ExcelFile File)
        {
            InitializeComponent();
            _excelFile = File;
        }

        /// <summary>
        /// Кнопка создания правила
        /// </summary>
        private void CreateRules_Button_Click(object sender, RoutedEventArgs e)
        {
            ConstrRule_forButton ConstrRule_forButton = new ConstrRule_forButton(_excelFile);
            ConstrRule_forButton.Show();
        }

        private void HomePage_Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
