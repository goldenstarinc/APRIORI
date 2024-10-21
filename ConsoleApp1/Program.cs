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

        // Шифрование данных
        DataEncryptor dataEncryptor1 = new DataEncryptor(workbook, excelFile1);

        // Создание экземпляра AprioriAlgorithm

        HashSet<int> itemset = new HashSet<int>();
        itemset.Add(16);
        itemset.Add(20);

        List<RuleClass> rules = new List<RuleClass>();
        //rules = AA.GenerateSingleRules(-1, 0.2, 2, dataEncryptor1);

        //rules = AA.GenerateSpecificRules(2, itemset, dataEncryptor1);

        rules = AA.GenerateAllRules(-1, 0.3, 2, 6, dataEncryptor1);
        foreach (RuleClass rule in rules)
        {
            Console.WriteLine(rule.ToString());
        }

    }


    
}