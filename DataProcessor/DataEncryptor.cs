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
        public Workbook _workbook { get; private set; }

        public ExcelFile _metaFile { get; private set; }

        // Список для хранения возможных значений
        public List<List<string>> _appropriateValues { get; private set; }

        // Список для хранения перечисления имен бинарных свойств
        public List<string> _propertyNames { get; private set; }

        // Словарь, содержащий Имена и Краткие имена в столбцах
        public Dictionary<string, string> _namesAndShortNames { get; private set; }

        // Зашифрованные записи
        public List<BigInteger> _transactions { get; private set; }
        public List<List<BigInteger>> _subsets { get; private set; }
        public int _propertiesCount { get; private set; }

        private int mult;
        public DataEncryptor(Workbook workbook, ExcelFile metaFile, int mult = 1)
        {
            _workbook = workbook;
            _metaFile = metaFile;

            _appropriateValues = metaFile.AppropriateValues;
            _propertyNames = metaFile.PropertyNames;
            _namesAndShortNames = metaFile.NamesAndShortNames;
            _propertiesCount = _propertyNames.Count;

            _transactions = GetEncryptedRecords();

            List<BigInteger> _tempTransactions = new List<BigInteger>();
            _tempTransactions.AddRange(_transactions);

            _subsets = new List<List<BigInteger>>();
            for (int i = 0; i < metaFile.SubsetsCount; i++)
            {
                _subsets.Add(FormSubsets(i, _tempTransactions));
            }
        }


        /// <summary>
        /// Метод, зашифровывающий записи в файле
        /// </summary>
        /// <param name="wb">Файл Excel</param>
        /// <returns>Список зашифрованных записей типа BigInteger</returns>
        public List<BigInteger> GetEncryptedRecords()
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

        /// <summary>
        /// Формирует подмножества на основе принадлежности записи к определенному бинарному свойству целевого столбца
        /// </summary>
        /// <param name="index"> Индекс бинарного свойства целевого столбца</param>
        /// <returns> Подмножество</returns>
        public List<BigInteger> FormSubsets(int index, List<BigInteger> _tempTransactions)
        {
            // Хранение записи для добавления подмножества
            List<BigInteger> temp = new List<BigInteger>();

            // Проходимся по каждой записи
            foreach (var transaction in _tempTransactions)
            {
                string item = DataDecryptor.ToBinaryString(transaction);
                if (item[item.Length - 1 - index] == '1')
                {
                    temp.Add(transaction);
                }
            }

            // Удаляем записанные записи из основного списка
            foreach (var delete in temp)
            {
                _tempTransactions.Remove(delete);
            }

            return temp;
        }

        public static string ToBinaryString(BigInteger bigInteger, int propertyNamesCount)
        {
            return Convert.ToString((long)bigInteger, 2).PadLeft(propertyNamesCount, '0'); // Дополняем нулями слева
        }
    }
}
