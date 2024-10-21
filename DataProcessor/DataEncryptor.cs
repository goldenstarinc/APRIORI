using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor
{
    public  class DataEncryptor
    {
        // Книга Excel
        private Workbook _workbook;

        // Список для хранения возможных значений
        private List<List<string>> _appropriateValues;

        // Список для хранения перечисления имен бинарных свойств
        private List<string> _propertyNames;

        // Словарь, содержащий Имена и Краткие имена в столбцах
        private Dictionary<string, string> _namesAndShortNames;
        
        public DataEncryptor(Workbook workbook, List<List<string>> AppropriateValues, List<string> PropertyNames, Dictionary<string, string> NamesAndShortNames)
        {
            _workbook = workbook;
            _appropriateValues = AppropriateValues;
            _propertyNames = PropertyNames;
            _namesAndShortNames = NamesAndShortNames;
        }


        /// <summary>
        /// Метод, зашифровывающий записи в файле
        /// </summary>
        /// <param name="wb">Файл Excel</param>
        /// <returns>Список зашифрованных записей типа BigInteger</returns>
        public List<BigInteger> GetEncryptedRecords(int mult = 1)
        {
            List<BigInteger> encryptedRecords = new List<BigInteger>();

            Cells cells = _workbook.Worksheets[0].Cells;

            for (int i = 1; i <= cells.MaxDataRow; ++i)
            {
                BigInteger encryptedRecord = BigInteger.Zero;

                for (int j = 1; j <= cells.MaxDataColumn; ++j)
                {

                    int count = FindClassIndexForCellValue(cells[i, j].StringValue, j - 1);

                    // Классовое имя параметра
                    string className = _namesAndShortNames[cells[0, j].StringValue] + count.ToString();

                    // Поиск индекса классового имени параметра в списке классавыхо имен
                    int propertyIndex = _propertyNames.IndexOf(className);

                    // В случае, если индекс найден - добавляем степень двойки
                    if (propertyIndex != -1)
                    {
                        BigInteger powerOfTwo = BigInteger.Pow(2, propertyIndex);
                        encryptedRecord += powerOfTwo;
                    }
                }
                // Добавляем запись в список записей типа 
                for(int j = 0; j <= mult; ++j)
                {
                    encryptedRecords.Add(encryptedRecord);
                }
            }

            return encryptedRecords;
        }

        /// <summary>
        /// Метод, производящий поиск классового индекса для клетки
        /// </summary>
        /// <param name="cellValue">Значение клетки</param>
        /// <param name="columnIndex">Индекс столбца</param>
        /// <returns>Классовый индекс для данного значения</returns>
        private int FindClassIndexForCellValue(string  cellValue, int columnIndex)
        {
            // Список для хранения допустимых значений для столбца j 
            List<string> rangeValues = _appropriateValues[columnIndex];

            // Обозначение для поиска номера подходящего класса (установлено -1, для обработки слуая, в котором значение из таблицы меньше минимально допустимого значения
            int count = -1;

            foreach (string value in rangeValues)
            {
                if (int.TryParse(cellValue, out int parsedCellValue) & int.TryParse(value, out int parsedRangeValue))
                {
                    if (parsedCellValue < parsedRangeValue)
                    {
                        break;
                    }
                    count++;
                }
                else
                {
                    count++;
                    if (cellValue == value)
                    {
                        break;
                    }
                }
            }

            return count;
        }

    }
}
