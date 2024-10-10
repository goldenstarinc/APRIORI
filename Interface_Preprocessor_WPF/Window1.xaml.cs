using System;
using System.Collections.Generic;
using System.Data;
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
using Aspose.Cells;
using DataProcessor;
using System.Numerics;
using Microsoft.Win32;
using System.Data.Common;

namespace Interface_Preprocessor_WPF
{
    /// <summary>
    /// Класс для взаимодействия с основным окном приложения
    /// </summary>
    public partial class Window1 : Window
    {
        private string _filePath; // Путь к файлу
        private ExcelFile _excelFile; // Объект для работы с данными Excel

        // Конструктор класса
        public Window1(string filePath, ExcelFile ExcelFile)
        {
            InitializeComponent();
            _filePath = filePath; // Инициализация пути к файлу
            _excelFile = ExcelFile; // Инициализация объекта ExcelFile
        }

        // Обработчик события нажатия кнопки загрузки файла
        private void LoadFile_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Чтение Excel файла с помощью Aspose.Cells
                Workbook workbook = new Workbook(_filePath);
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

        // Обработчик события нажатия кнопки шифрования файла
        private void EncryptFile_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получение данных для шифрования
                List<List<string>> appropriateValues = _excelFile.AppropriateValues;
                List<string> propertyNames = _excelFile.PropertyNames;
                Dictionary<string, string> namesAndShortNames = _excelFile.NamesAndShortNames;

                // Открытие диалогового окна для выбора файла
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";

                if (openFileDialog.ShowDialog() == true)
                {
                    Workbook wb = new Workbook(openFileDialog.FileName);

                    // Создание объекта шифрования данных
                    DataEncryptor dataEncryptor = new DataEncryptor(wb, appropriateValues, propertyNames, namesAndShortNames);

                    // Получение зашифрованных записей
                    List<BigInteger> encryptedRecords = dataEncryptor.GetEncryptedRecords();

                    // Предложение пользователю выбрать место для сохранения зашифрованных данных
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Text Files|*.txt|All Files|*.*",
                        Title = "Сохранить зашифрованные данные",
                        FileName = "encryptedData.txt"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filePath = saveFileDialog.FileName;

                        if (encryptedRecords.Count == 0)
                        {
                            throw new Exception("Ни один файл не был зашифрован. Пожалуйста, проверьте выбранный файл!");
                        }

                        // Запись данных в файл
                        DataWriter.WriteEncryptedDataToFile(propertyNames, encryptedRecords, filePath);
                        MessageBox.Show("Файл успешно сохранён!");
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений и вывод сообщения об ошибке
                CustomMessageBox customMessageBox = new CustomMessageBox($"Ошибка: {ex.Message}");
                customMessageBox.ShowDialog();
            }
        }

        // Обработчик события нажатия кнопки расшифрования файла
        private void DecryptFile_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Открытие диалогового окна для выбора зашифрованного файла
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    List<BigInteger> encryptedRecords = DataReader.ReadEncryptedDataFromFile(filePath);

                    List<string> propertyNames = _excelFile.PropertyNames;
                    DataDecryptor dataDecryptor = new DataDecryptor(encryptedRecords, propertyNames);
                    List<List<string>> decryptedRecords = dataDecryptor.GetDecryptedRecords();

                    if (decryptedRecords.Count == 0)
                    {
                        throw new Exception("Ни один файл не был расшифрован. Пожалуйста, проверьте выбранный файл!");
                    }

                    // Открытие окна для отображения расшифрованных данных
                    DecryptedDataWindow decryptedDataWindow = new DecryptedDataWindow(decryptedRecords);
                    decryptedDataWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений и вывод сообщения об ошибке
                CustomMessageBox customMessageBox = new CustomMessageBox($"Ошибка: {ex.Message}");
                customMessageBox.ShowDialog();
            }
        }

        private void OpenInfo_Click(object sender, RoutedEventArgs e)
        {
            InfoPage infoPage = new InfoPage();
            infoPage.Show();
        }
    }
}
