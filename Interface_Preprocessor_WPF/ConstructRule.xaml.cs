using Aspose.Cells;
using DataProcessor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private DataEncryptor encryptedData;

        private double quality;

        private double confidence;

        private int sendingLength;

        private int ruleNumber;

        public ConstructRule(double Quality, double Confidence, int SendingLength, int RuleNumber)
        {
            InitializeComponent();

            try
            {
                OutputRules_TextBox.Text = string.Empty;
                OutputRunningTime_TextBox.Text = string.Empty;
                encryptedData = SharedData.Instance.EncryptedData;
                
                if (encryptedData == null || encryptedData._transactions.Count == 0) { throw new Exception("Зашифрованные записи не обнаружены"); }

                quality = Quality;
                confidence = Confidence;
                sendingLength = SendingLength;
                ruleNumber = RuleNumber;
            }
            catch (Exception ex)
            {
                CustomMessageBox customMessageBox = new CustomMessageBox($"Ошибка: {ex.Message}");
                customMessageBox.ShowDialog();
            }
        }

        /// <summary>
        /// Кнопка создания правила
        /// </summary>
        private void CreateRules_Button_Click(object sender, RoutedEventArgs e)
        {
            ConstrRule_forButton ConstrRule_forButton = new ConstrRule_forButton(quality, confidence, sendingLength, ruleNumber);
            ConstrRule_forButton.Show();
        }

        

        private void CreateSingleRules_Button_Click(object sender, RoutedEventArgs e)
        {
            OutputRules_TextBox.Text = string.Empty;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            List<RuleClass> rules = AA.GenerateSingleRules(ruleNumber, confidence, quality, encryptedData);

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;

            // Вывод правил в консоль
            foreach (RuleClass rule in rules)
            {
                OutputRules_TextBox.Text += rule.ToString();
            }


            if (OutputRules_TextBox.Text.Trim() == string.Empty)
            {
                OutputRules_TextBox.Text = "По заданным характеристикам не было построено ни одного правила:\n" +
                                                                              $"Качество: {quality}\n" +
                                                                              $"Достаточная достоверность: {confidence}\n" +
                                                                              $"Номер правила: {ruleNumber}";
            }

            OutputRunningTime_TextBox.Text = elapsedTime.TotalSeconds.ToString() + " сек.";
        }

        private void CreateAllRules_Button_Click(object sender, RoutedEventArgs e)
        {
            OutputRules_TextBox.Text = string.Empty;

            List<RuleClass> rules = new List<RuleClass>();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            rules = AA.GenerateAllRules(ruleNumber, confidence, quality, sendingLength, encryptedData);

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;


            // Вывод правил в консоль
            foreach (RuleClass rule in rules)
            {
                OutputRules_TextBox.Text += rule.ToString();
            }

            if (OutputRules_TextBox.Text == string.Empty)
            {
                OutputRules_TextBox.Text = "По заданным характеристикам не было построено ни одного правила:\n" +
                                                                              $"Качество: {quality}\n" +
                                                                              $"Достаточная достоверность: {confidence}\n" +
                                                                              $"Длина посылки: {sendingLength}\n" +
                                                                              $"Номер правила: {ruleNumber}";
            }

            OutputRunningTime_TextBox.Text = elapsedTime.TotalSeconds.ToString() + " сек.";
        }
    }
}
