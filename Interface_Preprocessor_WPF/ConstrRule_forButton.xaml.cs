//using DataProcessor;
using Aspose.Cells;
using DataProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using controls = System.Windows.Controls;
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
    /// Логика взаимодействия для ConstrRule_forButton.xaml
    /// </summary>
    public partial class ConstrRule_forButton : Window
    {
        private string[] parameters; // Бинарные свойства

        private SortedSet<int> selectedIndexes; // Хранит индексы выбранных плиток

        private DataEncryptor encryptedData; // Объект DataEncryptor

        private int sendingLength; // Длина посылки

        private int ruleNumber; // Номер правила

        private ExcelFile metaFile; // Файл с метаданными

        private List<controls.Border> selectedTiles = new List<controls.Border>();  // Хранит выбранные плитки
        public ConstrRule_forButton(int SendingLength, int RuleNumber)
        {
            InitializeComponent();
            try
            {
                selectedIndexes = new SortedSet<int>();
                encryptedData = SharedData.Instance.EncryptedData;
                metaFile = SharedData.Instance.MetaFile;

                sendingLength = SendingLength;
                ruleNumber = RuleNumber;

                parameters = new string[metaFile.PropertyNames.Count];

                // Заполняем массив бинарными свойствами
                for (int i = 0; i < metaFile.PropertyNames.Count; i++)
                {
                    parameters[i] = metaFile.PropertyNames[i];
                }
                CreateParameterTiles();
            }
            catch (Exception ex)
            {
                CustomMessageBox customMessageBox = new CustomMessageBox($"Ошибка: {ex.Message}");
                customMessageBox.ShowDialog();
            }
        }

        /// <summary>
        /// Создание плиток из параметров
        /// </summary>
        private void CreateParameterTiles()
        {
            try
            {
                if (parameters == null || parameters.Length == 0) { throw new Exception("Параметры не заданы или пусты."); }
                // Очищаем все элементы внутри WrapPanel на случай, если они уже есть
                parametersPanel.Children.Clear();

                // Добавляем плитки для каждого параметра
                foreach (string param in parameters)
                {
                    if (Array.IndexOf(parameters, param) >= encryptedData._subsets.Count)
                    {
                        // Создаём Border как плитку
                        controls.Border tile = new controls.Border
                        {
                            Width = 150,      // Задаём ширину плитки
                            Height = 50,     // Задаём высоту плитки
                            Margin = new Thickness(10),  // Отступы между плитками
                            CornerRadius = new CornerRadius(10),  // Скруглённые углы плитки
                            BorderBrush = new SolidColorBrush(Colors.AliceBlue), // Цвет рамки плитки
                            BorderThickness = new Thickness(2),  // Толщина рамки
                            Background = new SolidColorBrush(Colors.LightCyan) // Цвет фона
                        };

                        // Внутри плитки размещаем TextBlock с параметром
                        controls.TextBlock textBlock = new controls.TextBlock
                        {
                            Text = param,  // Текст параметра
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 16,
                            FontWeight = FontWeights.Bold,
                            TextWrapping = TextWrapping.Wrap,
                            TextAlignment = TextAlignment.Center
                        };

                        // Добавляем TextBlock внутрь Border
                        tile.Child = textBlock;

                        // Добавляем обработчик нажатия на плитку
                        tile.MouseLeftButtonDown += (s, e) => OnTileClick(tile);

                        // Добавляем плитку в WrapPanel
                        parametersPanel.Children.Add(tile);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox customMessageBox = new CustomMessageBox($"Ошибка: {ex.Message}");
                customMessageBox.ShowDialog();
            }
        }

        private void OnTileClick(controls.Border tile)
        {
            controls.TextBlock textBlock = tile.Child as controls.TextBlock;

            if (textBlock != null)
            {// Находим индекс параметра
                int paramIndex = Array.IndexOf(parameters, textBlock.Text);

                // Если плитка уже выбрана, снимаем выбор
                if (selectedIndexes.Contains(paramIndex))
                {
                    selectedIndexes.Remove(paramIndex);
                    tile.Background = new SolidColorBrush(Colors.LightCyan);  // Устанавливаем цвет по умолчанию
                }
                else
                {
                    // Извлекаем базовое имя параметра без цифр (например, "Credit_History" из "Credit_History0")
                    string basePropertyName = GetBasePropertyName(textBlock.Text);

                    // Проверяем, не выбрано ли свойство с таким же базовым именем, но другой цифрой
                    if (IsBasePropertyAlreadySelected(basePropertyName))
                    {
                        CustomMessageBox customMessageBox = new CustomMessageBox($"Свойство {basePropertyName} уже выбрано.");
                        customMessageBox.ShowDialog();
                        return;
                    }


                    selectedIndexes.Add(paramIndex);  // Добавляем индекс в SortedSet, автоматически сортируется
                    tile.Background = new SolidColorBrush(Colors.LightGreen);  // Подсвечиваем выбранную плитку
                }
            }
        }

        /// <summary>
        /// Извлекает базовое имя параметра (без цифр в конце)
        /// </summary>
        /// <param name="propertyName">Полное имя параметра</param>
        /// <returns>Базовое имя параметра без цифр</returns>
        private string GetBasePropertyName(string propertyName)
        {
            // Удаляем любые цифры в конце строки
            return System.Text.RegularExpressions.Regex.Replace(propertyName, @"\d+$", "");
        }

        /// <summary>
        /// Проверяет, выбрано ли уже свойство с таким же базовым именем
        /// </summary>
        /// <param name="basePropertyName">Базовое имя параметра</param>
        /// <returns>True, если свойство с таким базовым именем уже выбрано</returns>
        private bool IsBasePropertyAlreadySelected(string basePropertyName)
        {
            foreach (int index in selectedIndexes)
            {
                string selectedPropertyName = parameters[index];
                string selectedBaseName = GetBasePropertyName(selectedPropertyName);

                if (selectedBaseName == basePropertyName)
                {
                    return true;  // Найдено свойство с таким же базовым именем
                }
            }

            return false;
        }

        /// <summary>
        /// Построение правила с указанной посылкой
        /// </summary>
        private void Do_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Очищение текстбокса
                OutputAllRules_TextBox_AddPage.Text = string.Empty;

                List<RuleClass> rules = AA.GenerateSpecificRules(ruleNumber, selectedIndexes.ToHashSet(), encryptedData);

                // Вывод правил в консоль
                foreach (RuleClass rule in rules)
                {
                    OutputAllRules_TextBox_AddPage.Text += rule.ToString();
                }


                // Если ни одно правило не было построено
                if (OutputAllRules_TextBox_AddPage.Text == string.Empty)
                {
                    OutputAllRules_TextBox_AddPage.Text = "По заданным характеристикам не было построено ни одного правила.";
                }

            }
            catch (Exception ex)
            {
                CustomMessageBox customMessageBox = new CustomMessageBox($"Ошибка: {ex.Message}");
                customMessageBox.ShowDialog();
            }
        }

    }
}
