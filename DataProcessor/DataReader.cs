using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace DataProcessor
{
    public class DataReader
    {
        /// <summary>
        /// Читает зашифрованные данные из файла
        /// </summary>
        /// <param name="filePath">Путь к входному файлу</param>
        /// <returns>Список зашифрованных записей</returns>
        public static List<BigInteger> ReadEncryptedDataFromFile(string filePath)
        {
            List<BigInteger> encryptedRecords = new List<BigInteger>();
            bool isReadingRecords = false;

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Пропускаем строки до "Encrypted records list:"
                        if (line.StartsWith("Encrypted records list:"))
                        {
                            isReadingRecords = true; // Начинаем читать зашифрованные записи
                            continue;
                        }

                        if (isReadingRecords)
                        {
                            // Читаем и парсим зашифрованные записи
                            if (BigInteger.TryParse(line, out BigInteger encryptedRecord))
                            {
                                encryptedRecords.Add(encryptedRecord);
                            }
                            else
                            {
                                throw new FormatException($"Не удалось разобрать зашифрованную запись: {line}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new IOException($"Ошибка при чтении файла: {ex.Message}", ex);
            }

            return encryptedRecords;
        }
    }
}
