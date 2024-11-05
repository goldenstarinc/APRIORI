using Aspose.Cells;
using DataProcessor;
using Microsoft.Win32;
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
    /// Логика взаимодействия для SelectPage.xaml
    /// </summary>
    public partial class SelectPage : Page
    {
        // Файл, содержащий метаданные
        public ExcelFile metaFile { get; private set; }
        public string filePath { get; private set; }
        public DataEncryptor encryptedData { get; private set; }

        public SelectPage()
        {
            InitializeComponent();
            MetaDBPath_TextBox.Text = string.Empty;
            PathDB_TextBox.Text = string.Empty;
        }

        /// <summary>
        /// Функция для выбора файла с метаданными
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseMetaDB_Button_Click(object sender, RoutedEventArgs e)
        {
            // Открытие диалогового окна для выбора файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    this.metaFile = new ExcelFile(openFileDialog.FileName);
                    MetaDBPath_TextBox.Text = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    CustomMessageBox customMessageBox = new CustomMessageBox($"Ошибка: {ex.Message}");
                    customMessageBox.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Функция для выбора файла базы данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseDB_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Открытие диалогового окна для выбора файла
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;

                    if (metaFile == null)
                    {
                        throw new Exception("Загрузите файл, содержащий метаданные.");
                    }

                    if (!IsAdequeteDatabase(metaFile, filePath))
                    {
                        throw new Exception("Ваша база данных не соответствует файлу с метаданными.");
                    }

                    // Шифрование записей
                    this.filePath = filePath;


                    ParameterInput pi  = new ParameterInput();
                    pi.ShowDialog();

                    int mult = pi.value;

                    this.encryptedData = new DataEncryptor(new Workbook(filePath), metaFile, mult);

                    PathDB_TextBox.Text = filePath;
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
        /// Функция сохранения данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (metaFile == null) throw new Exception("Пожалуйста, загрузите файл, содержащий метаданные.");
                if (filePath == string.Empty) throw new Exception("Пожалуйста, загрузите базу данных.");

                SharedData.Instance.MetaFile = metaFile;
                SharedData.Instance.FilePath = filePath;
                SharedData.Instance.EncryptedData = encryptedData;

                DonePage dp = new DonePage("Файлы сохранены!");
                dp.Show();

            }
            catch (Exception ex)
            {
                // Обработка исключений и вывод сообщения об ошибке
                CustomMessageBox customMessageBox = new CustomMessageBox($"{ex.Message}");
                customMessageBox.ShowDialog();
            }
        }
    }
}
