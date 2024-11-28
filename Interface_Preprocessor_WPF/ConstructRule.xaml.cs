using Aspose.Cells;
using DataProcessor;
using System;
using System.Collections.Generic;
using System.Data;
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
        private DataEncryptor encryptedData; // Объект типа DataEncryptor

        private double quality; // Качество

        private int sendingLength; // Длина посылки

        private int ruleNumber; // Номер правила

        private double minConfidence; // Минимальная граница достаточной достоверности

        private double maxConfidence; // Максимальная граница достаточной достоверности

        private double correlation; // Корреляция

        private double frequency; // Частота

        private string mode; // Режим работы программы, зависящий от введенных параметров
        public ConstructRule(double Quality, double MinConfidence, double MaxConfidence, double Correlation, double Frequency, int SendingLength, int RuleNumber, string Mode)
        {
            InitializeComponent();

            try
            {
                // Очищаем текстбоксы
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

        
        /// <summary>
        /// Генерирует единичные правила
        /// </summary>
        private void CreateSingleRules_Button_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<RuleClass> SingleRules = AA.GenerateSingleRules(ruleNumber, encryptedData);
            ShowRules(SingleRules);

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;

            OutputRunningTime_TextBox.Text = elapsedTime.TotalSeconds.ToString() + " сек.";
        }

        /// <summary>
        /// Генерирует все правила
        /// </summary>
        private void CreateAllRules_Button_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<RuleClass> rules = AA.GenerateAllRules(ruleNumber, sendingLength, encryptedData, mode, quality, correlation, frequency, minConfidence, maxConfidence);
            ShowRules(rules);

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;

            OutputRunningTime_TextBox.Text = elapsedTime.TotalSeconds.ToString() + " сек.";
        }

        /// <summary>
        /// Отображает правила в текстбоксе
        /// </summary>
        /// <param name="rules">Список правил</param>
        public void ShowRules(List<RuleClass> rules)
        {
            OutputRules_TextBox.Text = string.Empty;
            OutputRunningTime_TextBox.Text = string.Empty;


            if (mode == "Качество") { rules = AA.FilterRulesByQuality(rules, quality); }
            else if (mode == "Корреляция") { rules = AA.FilterRulesByCorrelation(rules, correlation); }
            else if (mode == "Достоверность и частота") { rules = AA.FilterRulesByFrequencyAndConfidence(rules, frequency, minConfidence); }
            else if (mode == "Достоверность и частота(диапазон)") { rules = AA.FilterRulesByFrequencyAndConfidence(rules, frequency, minConfidence, maxConfidence); }


            // Вывод правил в консоль
            foreach (RuleClass rule in rules)
            {
                OutputRules_TextBox.Text += rule.ToString();
            }

            if (OutputRules_TextBox.Text == string.Empty)
            {
                OutputRules_TextBox.Text = "По заданным характеристикам не было построено ни одного правила.";
            }
        }
    }
}
