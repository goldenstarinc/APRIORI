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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Interface_Preprocessor_WPF
{
    /// <summary>
    /// Логика взаимодействия для RuleConstructionPage.xaml
    /// </summary>
    public partial class RuleConstructionPage : Page
    {
        private string filePath; // Путь к файлу
        private ExcelFile metaFile; // Объект для работы с данными Excel
        private DataEncryptor encryptedData;

        private bool areParametersSet;

        private int quality;
        private int confidence;
        private int sendingLength;
        private int ruleNumber;
        public RuleConstructionPage()
        {
            InitializeComponent();
            metaFile = SharedData.Instance.MetaFile;
            encryptedData = SharedData.Instance.EncryptedData;
            ParametrsList_ListBox.ItemsSource = metaFile.PropertyNames;
        }

        /// <summary>
        /// Считываение параметров правила
        /// </summary>
        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                quality = int.Parse(qualityTB.Text);
                confidence = int.Parse(confidenceTB.Text);
                sendingLength = int.Parse(sendingLengthTB.Text);
                ruleNumber = int.Parse(ruleNumberTB.Text);




                OutputMessage_TextBox.Text = "Заданы следующие параметры: \n" +
                                             $"Качество = {quality}\n" +
                                             $"Достоверность = {confidence}\n" +
                                             $"Длина посылки = {sendingLength}\n" +
                                             $"Номер правила = {ruleNumber}";

                if (quality < 0) throw new Exception("Качество не может быть меньше нуля.");

                if (confidence < 0) throw new Exception("Достоверность не может быть меньше нуля.");
                if (confidence > 1) throw new Exception("Достоверность не может быть больше единицы.");

                if (sendingLength < 1) throw new Exception($"Длина посылки не может быть меньше единицы.");
                if (sendingLength > metaFile.ColumnTypes.Count) throw new Exception($"Длина посылки не может быть больше количества стоблцов: [{metaFile.ColumnTypes.Count}] в выбранной базе данных.");

                if (ruleNumber < -1) throw new Exception($"Номер правила не может быть меньше -1.");
                if (ruleNumber > metaFile.SubsetsCount - 1) throw new Exception($"Максимальный разрешенный номер правила относительно выбранной базы данных: [{metaFile.SubsetsCount - 1}].");

                DonePage dp = new DonePage("Параметры заданы!");
                dp.Show();

                areParametersSet = true;
            }
            catch (Exception ex)
            {
                areParametersSet = false;
                OutputMessage_TextBox.Text = $"Ошибка! {ex.Message}";
                CustomMessageBox customMessageBox = new CustomMessageBox($"Ошибка: {ex.Message}");
                customMessageBox.ShowDialog();
            }
        }

        /// <summary>
        /// Создание правила
        /// </summary>
        private void CreateRule_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!areParametersSet) { throw new Exception("Задайте параметры!"); }

                ConstructRule ConstructRule = new ConstructRule(quality, confidence, sendingLength, ruleNumber);
                ConstructRule.ShowDialog();
            }
            catch (Exception ex)
            {
                OutputMessage_TextBox.Text = $"Ошибка! {ex.Message}";
                CustomMessageBox customMessageBox = new CustomMessageBox($"Ошибка: {ex.Message}");
                customMessageBox.ShowDialog();
            }
        }
    }
}
