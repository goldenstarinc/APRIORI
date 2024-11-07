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

        private int sendingLength;

        private int ruleNumber;

        private double minConfidence;

        private double maxConfidence;

        private double correlation;

        private double frequency;

        private string mode;
        public ConstructRule(double Quality, double MinConfidence, double MaxConfidence, double Correlation, double Frequency, int SendingLength, int RuleNumber, string Mode)
        {
            InitializeComponent();

            try
            {
                OutputRules_TextBox.Text = string.Empty;
                OutputRunningTime_TextBox.Text = string.Empty;
                encryptedData = SharedData.Instance.EncryptedData;
                
                if (encryptedData == null || encryptedData._transactions.Count == 0) { throw new Exception("Зашифрованные записи не обнаружены"); }

                quality = Quality;
                minConfidence = MinConfidence;
                maxConfidence = MaxConfidence;
                correlation = Correlation;
                frequency = Frequency;
                sendingLength = SendingLength;
                ruleNumber = RuleNumber;

                mode = Mode;
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
            ConstrRule_forButton ConstrRule_forButton = new ConstrRule_forButton(sendingLength, ruleNumber);
            ConstrRule_forButton.Show();
        }

        

        private void CreateSingleRules_Button_Click(object sender, RoutedEventArgs e)
        {
            OutputRules_TextBox.Text = string.Empty;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            List<RuleClass> rules = new List<RuleClass>();

            if (mode == "Качество") { rules = AA.GenerateSingleRulesUsingQuality(ruleNumber, quality, encryptedData); }
            else if (mode == "Корреляция") { rules = AA.GenerateSingleRulesUsingCorrelation(ruleNumber, correlation, encryptedData); }
            else if (mode == "Достоверность и частота") { rules = AA.GenerateSingleRulesUsingConfidenceAndFrequency(ruleNumber, minConfidence, frequency, encryptedData); }
            else if (mode == "Достоверность и частота(диапазон)") { rules = AA.GenerateSingleRulesUsingConfidenceAndFrequency(ruleNumber, minConfidence, frequency, encryptedData, maxConfidence); }

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;

            // Вывод правил в консоль
            foreach (RuleClass rule in rules)
            {
                OutputRules_TextBox.Text += rule.ToString();
            }


            if (OutputRules_TextBox.Text.Trim() == string.Empty)
            {
                OutputRules_TextBox.Text = "По заданным характеристикам не было построено ни одного правила.";
            }

            OutputRunningTime_TextBox.Text = elapsedTime.TotalSeconds.ToString() + " сек.";
        }

        private void CreateAllRules_Button_Click(object sender, RoutedEventArgs e)
        {
            OutputRules_TextBox.Text = string.Empty;

            List<RuleClass> rules = new List<RuleClass>();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (mode == "Качество") { rules = AA.GenerateAllRulesUsingQuality(ruleNumber, quality, sendingLength, encryptedData); }
            else if (mode == "Корреляция") { rules = AA.GenerateAllRulesUsingCorrelation(ruleNumber, correlation, sendingLength, encryptedData); }
            else if (mode == "Достоверность и частота") { rules = AA.GenerateAllRulesUsingConfidenceAndFrequency(ruleNumber, minConfidence, frequency, sendingLength, encryptedData); }
            else if (mode == "Достоверность и частота(диапазон)") { rules = AA.GenerateAllRulesUsingConfidenceAndFrequency(ruleNumber, minConfidence, frequency, sendingLength, encryptedData, maxConfidence); }

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;

            // Вывод правил в консоль
            foreach (RuleClass rule in rules)
            {
                OutputRules_TextBox.Text += rule.ToString();
            }

            if (OutputRules_TextBox.Text == string.Empty)
            {
                OutputRules_TextBox.Text = "По заданным характеристикам не было построено ни одного правила.";
            }

            OutputRunningTime_TextBox.Text = elapsedTime.TotalSeconds.ToString() + " сек.";
        }
    }
}
