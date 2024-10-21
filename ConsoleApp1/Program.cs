using Aspose.Cells;
using DataProcessor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;
using AA = DataProcessor.AprioriAlgorithm;
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
        List<BigInteger> encryptedData = dataEncryptor1.GetEncryptedRecords(5);

        // Создание экземпляра AprioriAlgorithm
        DB_Data db_data = new DB_Data(encryptedData, PropertyNames, excelFile1.SubsetsCount);

        HashSet<int> itemset = new HashSet<int>();
        itemset.Add(16);
        itemset.Add(20);

        //GenerateSpecificRules(2, itemset, excelFile1);
        List<RuleClass> rules = new List<RuleClass>();
        rules = AA.GenerateAllRules(-1, 0.3, 2, 6, excelFile1);
        foreach(RuleClass rule in rules)
        {
            Console.WriteLine(rule.ToString());
        }
        //GenerateAllRules(-1, 0.3, 2, 6, excelFile1);

    }


    
}