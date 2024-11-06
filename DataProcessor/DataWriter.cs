using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor
{
    public class DataWriter
    {
        /// <summary>
        /// Записывает бинарные классы и зашифрованные записи в файл.
        /// </summary>
        /// <param name="propertyNames">Список имен бинарных классов</param>
        /// <param name="encryptedRecords">Список зашифрованных записей</param>
        /// <param name="filePath">Путь к выходному файлу</param>
        public static void WriteEncryptedDataToFile(List<string> propertyNames, List<string> encryptedRecords, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Записываем все бинарные классы на первой строке
                writer.WriteLine("Binary classes:");
                foreach (var propertyName in propertyNames)
                {
                    writer.Write(propertyName + " ");
                }
                writer.WriteLine();  // Переход на новую строку

                // Записываем зашифрованные записи с каждой новой строки
                writer.WriteLine("Encrypted records list:");
                foreach (var encryptedRecord in encryptedRecords)
                {
                    writer.WriteLine(encryptedRecord.ToString());
                }
            }
        }
    }
}
