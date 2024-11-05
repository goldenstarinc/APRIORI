using Aspose.Cells;
using DataProcessor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using AA = DataProcessor.AprioriAlgorithm;

class Program
{
    static void Main(string[] args)
    {
        // Чтение данных из Excel файла
        ExcelFile excelFile1 = new ExcelFile("MetaData.xlsx");
        Workbook workbook = new Workbook("BankDB.xlsx");

        // Шифрование данных
        DataEncryptor dataEncryptor1 = new DataEncryptor(workbook, excelFile1, 1000);


        HashSet<int> itemset = new HashSet<int> { 1, 0, 3, 2 };

        Console.WriteLine("Done!");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        //List<RuleClass> rules = AA.GenerateSingleRules(-1, 0, 0, dataEncryptor1);
        //List<RuleClass> rules = AA.GenerateSpecificRules(-1, itemset, dataEncryptor1);
        List<RuleClass> rules = AA.GenerateAllRules(-1, 0.9, 0.9, 5, dataEncryptor1);
        //43 s on 5
        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;


        Console.WriteLine(dataEncryptor1._transactions.Count);
        Console.WriteLine($"Transactions count: {dataEncryptor1._transactions.Count}");
        Console.WriteLine("Time spent: " + elapsedTime);
        Console.WriteLine();

        // Вывод правил в консоль
        foreach (RuleClass rule in rules)
        {
            Console.WriteLine(rule.ToString());
        }


        //var sets = GenerateCombinations(itemset, 2);
        //foreach (var set in sets)
        //{
        //    foreach (var num in set)
        //    {
        //        Console.Write(num + " ");
        //    }
        //    Console.WriteLine();
        //}

    }
}



