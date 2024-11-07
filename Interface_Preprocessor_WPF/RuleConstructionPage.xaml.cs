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
using System.Windows.Resources;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Interface_Preprocessor_WPF
{
    /// <summary>
    /// Логика взаимодействия для RuleConstructionPage.xaml
    /// </summary>
    public partial class RuleConstructionPage : Page
    {
        private bool showAdditionalTextBoxes = false;
        private string filePath; // Путь к файлу
        private ExcelFile metaFile; // Объект для работы с данными Excel
        private DataEncryptor encryptedData;

        private bool areParametersSet;

        private double quality;
        private double minConfidence;
        private double maxConfidence;
        private double correlation;
        private double frequency;
        private int sendingLength;
        private int ruleNumber;

        private string mode;
        public RuleConstructionPage()
        {
            InitializeComponent();
            SetTextBoxVisibility();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            metaFile = SharedData.Instance.MetaFile;
            encryptedData = SharedData.Instance.EncryptedData;
            ParametrsList_ListBox.ItemsSource = metaFile.PropertyNames;

            // Основные TextBox всегда видимы
            firstTB.Visibility = Visibility.Visible;
            secondTB.Visibility = Visibility.Visible;
            thirdTB.Visibility = Visibility.Visible;
            fourthTB.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Считываение параметров правила
        /// </summary>
        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (inputParamsCB.Text == "Качество")
                {
                    quality = double.Parse(firstTB.Text);

                    if (quality < 0) throw new Exception("Качество не может быть меньше нуля.");

                    if (!int.TryParse(secondTB.Text, out sendingLength)) { throw new Exception("Длина посылки должна быть целым числом."); }
                    if (!int.TryParse(thirdTB.Text, out ruleNumber)) { throw new Exception("Номер правила должен быть целым числом."); }
                    
                    OutputMessage_TextBox.Text = "Заданы следующие параметры: \n" +
                                                 $"Качество = {quality}\n" +
                                                 $"Длина посылки = {sendingLength}\n" +
                                                 $"Номер правила = {ruleNumber}";

                    mode = inputParamsCB.Text;
                }
                else if(inputParamsCB.Text == "Корреляция")
                {
                    correlation = double.Parse(firstTB.Text);

                    if (correlation < -1 || correlation > 1) throw new Exception("Корреляция должна находиться в диапазоне от -1 до 1.");

                    if (!int.TryParse(secondTB.Text, out sendingLength)) { throw new Exception("Длина посылки должна быть целым числом."); }
                    if (!int.TryParse(thirdTB.Text, out ruleNumber)) { throw new Exception("Номер правила должен быть целым числом."); }

                    OutputMessage_TextBox.Text = "Заданы следующие параметры: \n" +
                                             $"Корреляция = {correlation}\n" +
                                             $"Длина посылки = {sendingLength}\n" +
                                             $"Номер правила = {ruleNumber}";

                    mode = inputParamsCB.Text;
                }
                else if(inputParamsCB.Text == "Достоверность и частота")
                {
                    if (!showAdditionalTextBoxes)
                    {
                        minConfidence = double.Parse(firstTB.Text);
                        frequency = double.Parse(secondTB.Text);

                        if (minConfidence < 0) throw new Exception("Достоверность не может быть меньше нуля.");
                        if (minConfidence > 1) throw new Exception("Достоверность не может быть больше единицы.");

                        if (frequency < 0) throw new Exception("Частота не может быть меньше нуля.");
                        if (frequency > 1) throw new Exception("Частота не может быть больше единицы.");

                        if (!int.TryParse(thirdTB.Text, out sendingLength)) { throw new Exception("Длина посылки должна быть целым числом."); }
                        if (!int.TryParse(fourthTB.Text, out ruleNumber)) { throw new Exception("Номер правила должен быть целым числом."); }

                        OutputMessage_TextBox.Text = "Заданы следующие параметры: \n" +
                                             $"Достоверность = {minConfidence}\n" +
                                             $"Частота = {frequency}\n" +
                                             $"Длина посылки = {sendingLength}\n" +
                                             $"Номер правила = {ruleNumber}";

                        mode = inputParamsCB.Text;
                    }
                    else
                    {
                        minConfidence = double.Parse(firstTB.Text);
                        maxConfidence = double.Parse(confidenceTB_2.Text);

                        frequency = double.Parse(secondTB.Text);

                        if (minConfidence < 0 || maxConfidence < 0) throw new Exception("Достоверность не может быть меньше нуля.");
                        if (minConfidence > 1 || maxConfidence > 1) throw new Exception("Достоверность не может быть больше единицы.");

                        if (frequency < 0) throw new Exception("Частота не может быть меньше нуля.");
                        if (frequency > 1) throw new Exception("Частота не может быть больше единицы.");

                        if (!int.TryParse(thirdTB.Text, out sendingLength)) { throw new Exception("Длина посылки должна быть целым числом."); }
                        if (!int.TryParse(fourthTB.Text, out ruleNumber)) { throw new Exception("Номер правила должен быть целым числом."); }

                        OutputMessage_TextBox.Text = "Заданы следующие параметры: \n" +
                                                 $"Нижний предел достоверности = {minConfidence}\n" +
                                                 $"Верхний предел достоверности = {maxConfidence}\n" +
                                                 $"Частота = {frequency}\n" +
                                                 $"Длина посылки = {sendingLength}\n" +
                                                 $"Номер правила = {ruleNumber}";

                        mode = inputParamsCB.Text + "(диапазон)";
                    }
                }

                

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

                ConstructRule ConstructRule = new ConstructRule(quality, minConfidence, maxConfidence, correlation, frequency, sendingLength, ruleNumber, mode);
                ConstructRule.ShowDialog();
            }
            catch (Exception ex)
            {
                OutputMessage_TextBox.Text = $"Ошибка! {ex.Message}";
                CustomMessageBox customMessageBox = new CustomMessageBox($"Ошибка: {ex.Message}");
                customMessageBox.ShowDialog();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                switch (comboBox.SelectedIndex)
                {
                    case 0:
                        TextB.Text = "Качество\nДлина\nНомер правила";
                        fourthTB.Visibility = Visibility.Collapsed;
                        break;
                    case 1:
                        TextB.Text = "Корреляция\nДлина\nНомер правила";
                        fourthTB.Visibility = Visibility.Collapsed;
                        break;
                    case 2:
                        TextB.Text = "Достоверность\nЧастота\nДлина\nНомер правила";
                        fourthTB.Visibility = Visibility.Visible;
                        break;
                    default:
                        TextB.Text = "Произошла ошибка. Перезагрузите страницу."; 
                        break;
                }
            }
        }

        private void ToggleBtn_Click(object sender, RoutedEventArgs e)
        {
            if (inputParamsCB.Text == "Достоверность и частота")
            {
                showAdditionalTextBoxes = !showAdditionalTextBoxes;
                SetTextBoxVisibility();
            }
            else
            {
                CustomMessageBox customMessageBox = new CustomMessageBox($"Ошибка: Диапазон допустимых значений можно задать только для достоверности.");
                customMessageBox.ShowDialog();
            }
        }
        private void SetTextBoxVisibility()
        {
            // TextBox с суффиксом _2 показываются или скрываются в зависимости от флага
            Visibility visibility = showAdditionalTextBoxes ? Visibility.Visible : Visibility.Collapsed;
            
            confidenceTB_2.Visibility = visibility;

        }

    }
}