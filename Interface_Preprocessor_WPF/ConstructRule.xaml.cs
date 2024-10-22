using Aspose.Cells;
using DataProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
using AA = DataProcessor.AprioriAlgorithm;

namespace Interface_Preprocessor_WPF
{
    /// <summary>
    /// Логика взаимодействия для ConstructRule.xaml
    /// </summary>
    public partial class ConstructRule : Window
    {
        private DataEncryptor _encryptedData;
        public ConstructRule(DataEncryptor encryptedData)
        {
            InitializeComponent();
            _encryptedData = encryptedData;
        }

        /// <summary>
        /// Кнопка создания правила
        /// </summary>
        private void CreateRules_Button_Click(object sender, RoutedEventArgs e)
        {
            //ConstrRule_forButton ConstrRule_forButton = new ConstrRule_forButton(_encryptedData._metaFile);
            //ConstrRule_forButton.Show();
        }

        private void HomePage_Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CreateSingleRules_Button_Click(object sender, RoutedEventArgs e)
        {
            List<RuleClass> rules = new List<RuleClass>();

            rules = AA.GenerateSingleRules(2, 0.2, 2, _encryptedData);

            foreach (RuleClass rule in rules)
            {
                OutputRules_TextBox.Text += rule.ToString();
            }

        }

        private void CreateAllRules_Button_Click(object sender, RoutedEventArgs e)
        {
            List<RuleClass> rules = new List<RuleClass>();

            rules = AA.GenerateAllRules(2, 0.5, 3, 2, _encryptedData);

            foreach (RuleClass rule in rules)
            {
                OutputRules_TextBox.Text += rule.ToString();
            }
        }
    }
}
