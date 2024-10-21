using Aspose.Cells;
using DataProcessor;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Diagnostics;
using System.Xml.Linq;

namespace Interface_Preprocessor_WPF
{
    /// <summary>
    /// Логика взаимодействия для ConstructionWindow.xaml
    /// </summary>
    public partial class ConstructionWindow : Window
    {
        private string _filePath; // Путь к файлу
        private ExcelFile _excelFile; // Объект для работы с данными Excel

        private bool isLoaded;
        private bool areParametersSet;

        private int quality;
        private int confidence;
        private int sendingLength;
        private int ruleNumber;

        private DataEncryptor encryptedData;
        public ConstructionWindow(string filePath, ExcelFile ExcelFile)
        {
            InitializeComponent();
            _excelFile = ExcelFile;
            ParametrsList_ListBox.ItemsSource = _excelFile.PropertyNames;
            OutputMessage_TextBox.Text = "Метаданные успешно считаны!";
        }

        /// <summary>
        ///  Создание правила
        /// </summary>
        private void CreateRule_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isLoaded) { throw new Exception("Загрузите файл базы данных!"); }
                if (!areParametersSet) { throw new Exception("Задайте параметры!"); }
                
                ConstructRule ConstructRule = new ConstructRule(encryptedData);
                ConstructRule.ShowDialog();
            }
            catch (Exception ex)
            {
                OutputMessage_TextBox.Text = $"Ошибка! {ex.Message}";
                CustomMessageBox customMessageBox = new CustomMessageBox($"Ошибка: {ex.Message}");
                customMessageBox.ShowDialog();
            }

        }

        /// <summary>
        /// Открывает диалоговое окно для выбора файла базы данных
        /// </summary>
        private void View_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Открытие диалогового окна для выбора файла
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;

                    if (!IsAdequeteDatabase(_excelFile, filePath))
                    {
                        throw new Exception("Ваша база данных не соответствует файлу с метаданными.");
                    }

                    // Шифрование записей
                    this.encryptedData = new DataEncryptor(new Workbook(filePath), _excelFile);



                    OuputPathDB_TextBox.Text = filePath;

                    TimeSpan elapsedTime = TimerUtility.MeasureTime(() => LoadFile(filePath));
                    OutputMessage_TextBox.Text = $"Время чтения базы данных: {elapsedTime.TotalSeconds} секунд!";
                    isLoaded = true;
                }
            }

            catch (Exception ex)
            {
                // Обработка исключений и вывод сообщения об ошибке
                CustomMessageBox customMessageBox = new CustomMessageBox($"{ex.Message}");
                customMessageBox.ShowDialog();
            }
        }

        /// <summary>
        /// Проверяет соответствие выбранной базы данных загруженным ранее метаданным
        /// </summary>
        /// <param name="metaFile">Файл с метаданными</param>
        /// <param name="filePath">Путь к файлу с базой данных</param>
        /// <returns>Если база данных соответствует метаданным - возвращает true, иначе - false</returns>
        private bool IsAdequeteDatabase(ExcelFile metaFile, string filePath)
        {
            Workbook wb = new Workbook(filePath);
            Worksheet worksheet = wb.Worksheets[0];
            Cells cells = worksheet.Cells;

            Dictionary<string, string> NamesAndShortNames = metaFile.NamesAndShortNames;

            string[] names = new string[NamesAndShortNames.Count];

            int c = 0;
            // Заполняем массив именами столбцов из файла с мета-данными
            foreach (KeyValuePair<string, string> pair in NamesAndShortNames)
            {
                names[c] = pair.Key;
                ++c;
            }

            int count = 0;

            for (int i = 0; i <= cells.MaxDataColumn; ++i)
            {
                if (names.Contains(cells[0, i].StringValue))
                {
                    ++count;
                }
            }

            if (count == NamesAndShortNames.Count)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Считывает и выводит в объект типа "DataGrid" базу данных
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        private void LoadFile(string filePath)
        {
            try
            {

                // Чтение Excel файла с помощью Aspose.Cells
                Workbook workbook = new Workbook(filePath);
                Worksheet worksheet = workbook.Worksheets[0]; // Чтение первого листа

                // Преобразование данных Excel в DataTable
                DataTable dataTable = new DataTable();
                Cells cells = worksheet.Cells;

                // Проверяем, есть ли данные в файле
                if (cells.MaxDataRow == -1 && cells.MaxDataColumn == -1)
                {
                    throw new Exception("Файл не содержит строк и столбов");
                }

                // Добавляем столбцы в DataTable
                for (int col = 0; col <= worksheet.Cells.MaxDataColumn; col++)
                {
                    string columnName = worksheet.Cells[0, col].StringValue;
                    dataTable.Columns.Add(columnName);
                }

                // Добавляем строки в DataTable
                for (int row = 1; row <= worksheet.Cells.MaxDataRow; row++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int col = 0; col <= worksheet.Cells.MaxDataColumn; col++)
                    {
                        dataRow[col] = worksheet.Cells[row, col].StringValue;
                    }
                    dataTable.Rows.Add(dataRow);
                }

                // Привязываем DataTable к DataGrid для отображения
                dataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                // Обработка исключений и вывод сообщения об ошибке
                CustomMessageBox customMessageBox = new CustomMessageBox($"{ex.Message}");
                customMessageBox.ShowDialog();
            }
        }

        /// <summary>
        /// Открывает окно справки
        /// </summary>
        private void OpenInfo_Button_Click(object sender, RoutedEventArgs e)
        {
            InfoPage InfoPage = new InfoPage();
            InfoPage.Show();
        }

        /// <summary>
        /// Возвращает пользователя на главную 
        /// </summary>
        private void HomePage_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            this.Close();
            MainWindow.Show();
        }

        /// <summary>
        /// Задание фильтров
        /// </summary>
        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isLoaded)
                {
                    throw new Exception("Пожалуйста, загрузите файл базы данных!");
                }
                quality = int.Parse(qualityTB.Text);
                confidence = int.Parse(confidenceTB.Text);
                sendingLength = int.Parse(sendingLengthTB.Text);
                ruleNumber = int.Parse(ruleNumberTB.Text);

                OutputMessage_TextBox.Text = "Заданы следующие параметры: \n" +
                                             $"Качество = {quality}\n" +
                                             $"Достоверность = {confidence}\n" +
                                             $"Длина посылки = {sendingLength}\n" +
                                             $"Номер правила = {ruleNumber}";
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
    }
}
