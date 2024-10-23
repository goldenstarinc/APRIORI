using Aspose.Cells;
using DataProcessor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
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
    /// Логика взаимодействия для ViewPage.xaml
    /// </summary>
    public partial class ViewPage : Page
    {
        public ExcelFile metaFile;
        public string filePath;
        public ViewPage()
        {
            InitializeComponent();
            this.metaFile = SharedData.Instance.MetaFile;
            this.filePath = SharedData.Instance.FilePath;
        }

        private void ShowMetaDB_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (metaFile == null) throw new Exception("Файл с метаданными не был загружен.");
                LoadFile(metaFile.metaFilePath);
            }
            catch (Exception ex)
            {
                // Обработка исключений и вывод сообщения об ошибке
                CustomMessageBox customMessageBox = new CustomMessageBox($"{ex.Message}");
                customMessageBox.ShowDialog();
            }
        }

        private void ShowDB_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (filePath == null) throw new Exception("Файл базы данных не был загружен.");
                LoadFile(filePath);
            }
            catch (Exception ex)
            {
                // Обработка исключений и вывод сообщения об ошибке
                CustomMessageBox customMessageBox = new CustomMessageBox($"{ex.Message}");
                customMessageBox.ShowDialog();
            }
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
    }
}
