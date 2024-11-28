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
        ExcelFile excelFile1 = new ExcelFile("C:\\Users\\notso\\OneDrive\\Desktop\\Meta.xlsx");
        Workbook workbook = new Workbook("C:\\Users\\notso\\OneDrive\\Desktop\\Diabetes.xlsx");

        // Шифрование данных
        DataEncryptor dataEncryptor1 = new DataEncryptor(workbook, excelFile1, 1000);

        HashSet<int> itemset = new HashSet<int> { 1, 0, 3, 2 };

        Console.WriteLine("Done!");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        //List<RuleClass> rules = AA.GenerateSingleRulesUsingConfidenceAndFrequency(-1, 0.5, 0.6, dataEncryptor1, 0.6);
        //List<RuleClass> rules = AA.GenerateSpecificRules(-1, itemset, dataEncryptor1);
        List<RuleClass> rules = AA.GenerateAllRules(-1, 3, dataEncryptor1, "Достоверность и частота", null, null, 0.1, 0.1, null);
        rules = AA.FilterRulesByFrequencyAndConfidence(rules, 0.1, 0.1);

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
    }
}



