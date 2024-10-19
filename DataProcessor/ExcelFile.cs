using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeroesLibrary;
using Aspose.Cells;
using System.Security.Policy;
using System.Numerics;
using System.Data.SqlTypes;
using System.Security.Principal;
using Aspose.Cells.Charts;

namespace DataProcessor
{
    public class ExcelFile
    {
        private Workbook _workbook;

        public int SubsetsCount { get; private set; }
        public List<string> PropertyNames { get; private set; }
        public List<string> ColumnTypes { get; private set; }
        public List<List<string>> AppropriateValues { get; private set; }
        public Dictionary<string, string> NamesAndShortNames { get; private set; }
        public ExcelFile (string filePath)
        {
            try
            {
                _workbook = new Workbook(filePath);
                SubsetsCount = GetSubsetsCount();
                PropertyNames = GetPropertyNames();
                ColumnTypes = GetColumnTypes();
                AppropriateValues = GetAppropriateValues();
                NamesAndShortNames = GenerateNameToShortNameDictionary();
                if (SubsetsCount == -1) { throw new Exception("Количество значений целевого столбца указано неверно.\n:)");}
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при загрузке файла Excel: {ex.Message}");
            }
        }

        public Workbook GetWorkbook()
        {
            return _workbook;
        }

        

        /// <summary>
        /// Возвращает список названий классов, основанных на данных файла Excel из колонки "Краткое имя"
        /// </summary>
        /// <param name="filePath">Путь к файлу Excel</param>
        /// <returns>Список классов бинарных свойств</returns>
        /// <exception cref="Exception">В случае, если в таблице Excel отсутствуют колонки с именем "Краткое имя" или же "Количество значений" выдается ошибка</exception>
        private List<string> GetPropertyNames()
        {
            List<string> properties = new List<string>();

            Worksheet worksheet = _workbook.Worksheets[0];

            Cells cells = worksheet.Cells;

            // Индексы столбцов
            int shortNameColumnIndex = FindColumnIndex("Краткое имя", cells);
            int valueCountColumnIndex = FindColumnIndex("Число значений", cells);


            // Создаем классы в соответствие с названием бинарного свойства и количества его значений  
            for (int i = 1; i <= cells.MaxDataRow; ++i)
            {
                string cellName = worksheet.Cells[i, shortNameColumnIndex].StringValue;
                int propertyAmount = worksheet.Cells[i, valueCountColumnIndex].IntValue;

                for (int j = 0; j < propertyAmount; ++j)
                {
                    properties.Add($"{cellName}{j}");
                }
            }
            return properties;
        }

        /// <summary>
        /// Возвращает список типов столбцов
        /// </summary>
        /// <returns>Список типов столбцов</returns>
        private List<string> GetColumnTypes()
        {
            List<string> types = new List<string>();

            Worksheet worksheet = _workbook.Worksheets[0];
            Cells cells = worksheet.Cells;

            // Индекс столбца 'Тип столбца'
            int columnIndex = FindColumnIndex("Тип столбца", cells);

            // Проходим по столбцу 'Тип столбца' и добавляем считанные типы в список
            for (int i = 1; i <= cells.MaxDataRow; ++i)
            {
                types.Add(cells[i, columnIndex].StringValue);
            }

            return types;
        }

        /// <summary>
        /// Считывает допустимые значения столбцов у данного Excel файла
        /// </summary>
        /// <returns>Список списков, элементами которых являются строки</returns>
        private List<List<string>> GetAppropriateValues()
        {
            List<List<string>> values = new List<List<string>>();

            Worksheet worksheet = _workbook.Worksheets[0];

            Cells cells = worksheet.Cells;

            // Индекс столбца
            int columnIndex = FindColumnIndex("Перечисление значений", cells);


            // Считываем списки значений
            for (int i = 1; i <= cells.MaxDataRow; ++i)
            {
                string temporaryValues = cells[i, columnIndex].StringValue;
                if (!string.IsNullOrWhiteSpace(temporaryValues))  // Проверка на пустую строку
                {
                    List<string> splitValues = temporaryValues.Split(',')
                                                              .Select(v => v.Trim())  // Убираем лишние пробелы
                                                              .ToList();  // Преобразуем в список
                    values.Add(splitValues);  // Добавляем список значений в общий список
                }
            }

            return values;
        }

        /// <summary>
        /// Находит порядковый номер столбца по заданному имени
        /// </summary>
        /// <param name="columnName">Имя столбца для поиска</param>
        /// <param name="worksheet">Рабочий лист Excel, на котором расположен столбец</param>
        /// <returns>Индекс столбца</returns>
        /// <exception cref="Exception">В случае, если индекс столбца не найден выдаётся ошибка</exception>   
        private int FindColumnIndex(string columnName, Cells cells)
        {
            // Индекс столбца для поиска
            int columnIndex = -1;
            
            // Производим поиск индекса
            for (int j = 0; j <= cells.MaxDataColumn; j++)
            {
                if (cells[0, j].StringValue == columnName)
                {
                    columnIndex = j;
                    break;
                }
            }

            // В случае, если индекс не был найден выдаем ошибку
            if (columnIndex == -1)
            {
                throw new Exception($"Столбец '{columnName}' не найден.");
            }

            return columnIndex;
        }

        /// <summary>
        /// Создает словарь, где ключ - полное имя столбца, а значение - соответствующее ему краткое имя
        /// </summary>
        /// <returns>Словарь типа <string, string>, содержащий полное название столбца и его краткую форму</returns>
        private Dictionary<string, string> GenerateNameToShortNameDictionary()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            Cells cells = _workbook.Worksheets[0].Cells;

            int fullNameColumnIndex = FindColumnIndex("Название", cells);
            int shortNameColumnIndex = FindColumnIndex("Краткое имя", cells);

            for (int i = 1; i <= cells.MaxDataRow; ++i)
            {
                string fullName = cells[i, fullNameColumnIndex].StringValue;
                string shortName = cells[i, shortNameColumnIndex].StringValue;
                dict.Add(fullName, shortName);
            }

            return dict;
        }
        
        private int GetSubsetsCount()
        {
            int count = 0;

            Worksheet worksheet = _workbook.Worksheets[0];

            Cells cells = worksheet.Cells;

            int valueCountColumnIndex = FindColumnIndex("Число значений", cells);
            int columnIndex = FindColumnIndex("Тип столбца", cells);

            for (int i = 1; i <= cells.MaxDataRow; ++i)
            {
                if (cells[i, columnIndex].StringValue == "Целевой")
                {
                    count = cells[i, valueCountColumnIndex].IntValue; 
                    
                    return count;
                }
            }

            return -1;
        }
    }
}
