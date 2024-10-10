using Aspose.Cells;
using DataProcessor;
using System;
using System.Collections.Generic;
using System.Numerics;

class Program
{
    static void Main(string[] args)
    {
        // Чтение данных из Excel файла
        ExcelFile excelFile1 = new ExcelFile("Database1_Data.xlsx");
        Workbook workbook = new Workbook("Database1.xlsx");

        // Получение данных для Apriori
        List<List<string>> AppropriateValues = excelFile1.AppropriateValues;
        List<string> PropertyNames = excelFile1.PropertyNames; // Имена бинарных свойств
        Dictionary<string, string> NamesAndShortNames = excelFile1.NamesAndShortNames;

        // Шифрование данных
        var dataEncryptor1 = new DataEncryptor(workbook, AppropriateValues, PropertyNames, NamesAndShortNames);
        List<BigInteger> encryptedData = dataEncryptor1.GetEncryptedRecords();

        // Создание экземпляра AprioriAlgorithm
        var apriori = new AprioriAlgorithm(encryptedData, minSupport: 2, PropertyNames); // minSupport: минимальная поддержка

        // Нахождение частых наборов с порогом поддержки 20%
        List<HashSet<int>> frequentItemsets = apriori.FindFrequentItemsets(2);

        // Вывод частых наборов
        Console.WriteLine("Frequent Itemsets:");
        foreach (var freq in frequentItemsets)
        {
            foreach (var item in freq)
            {
                Console.Write(PropertyNames[item] + " "); // Используем названия свойств для вывода
            }
            Console.WriteLine();
        }

        // Генерация ассоциативных правил
        Console.WriteLine("\nAssociation Rules:");
        var associationRules = apriori.GenerateAssociationRules(frequentItemsets);
        foreach (var rule in associationRules)
        {
            string antecedent = string.Join(", ", rule.Item1.Select(i => PropertyNames[i]));
            string consequent = string.Join(", ", rule.Item2.Select(i => PropertyNames[i]));

            Console.WriteLine($"{antecedent} => {consequent} | Confidence: {rule.Item3:F2}, Lift: {rule.Item4:F2}");
        }
    }
}