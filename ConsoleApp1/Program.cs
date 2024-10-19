using Aspose.Cells;
using DataProcessor;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Transactions;

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
        AprioriAlgorithm._transactions = encryptedData;
        AprioriAlgorithm._propertyNames = PropertyNames;
        AprioriAlgorithm._subsets = excelFile1.SubsetsCount;
    }
}