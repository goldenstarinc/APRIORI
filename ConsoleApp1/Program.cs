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
        List<BigInteger> encryptedData = dataEncryptor1.GetEncryptedRecords();

        // Создание экземпляра AprioriAlgorithm
        DB_Data db_data = new DB_Data(encryptedData, PropertyNames, excelFile1.SubsetsCount);

        HashSet<int> itemset = new HashSet<int>();
        itemset.Add(16);
        itemset.Add(20);

        //GenerateSpecificRules(2, itemset, excelFile1);

        //GenerateSingleRules(-1, 0.3, 2, 2, excelFile1);

    }


    /// <summary>
    /// Генерация правил по выбранной посылке
    /// </summary>
    /// <param name="target">Номер подмножества</param>
    /// <param name="itemset">Посылка</param>
    /// <param name="excelFile1">Файл</param>
    public static void GenerateSpecificRules(int target, HashSet<int> itemset, ExcelFile excelFile1)
    {
        List<RuleClass> rules = new List<RuleClass>();

        if (target == -1)
        {
            for (int i = 0; i < excelFile1.SubsetsCount; ++i)
            {
                RuleClass rule = new RuleClass(itemset, i);
                rules.Add(rule);
            }
        }
        else
        {
            RuleClass rule = new RuleClass(itemset, target);
            rules.Add(rule);
        }

        foreach (RuleClass rule in rules)
        {
            Console.WriteLine(rule.ToString());
        }
    }

    /// <summary>
    /// Генерация единых правил согласно заданным параметрам
    /// </summary>
    /// <param name="target">Номер подмножества</param>
    /// <param name="needConfidence">Достаточная уверенность</param>
    /// <param name="needQuality">Достаточное качество</param>
    /// <param name="sendingLength">Достаточная длина посылки</param>
    /// <param name="excelFile1">Файл с записями</param>
    public static void GenerateSingleRules(int target, double needConfidence, double needQuality, double sendingLength, ExcelFile excelFile1)
    {
        List<RuleClass> rules = new List<RuleClass>();

        HashSet<int> itemset = new HashSet<int>();

        if (target == -1)
        {
            for (int j = 0; j < excelFile1.SubsetsCount; ++j)
            {
                for (int i = excelFile1.SubsetsCount; i < excelFile1.PropertyNames.Count; i++)
                {
                    itemset.Add(i);
                    RuleClass rule = new RuleClass(itemset, j);
                    if (rule.confidence > needConfidence & rule.quality > needQuality & rule.itemset.Count <= sendingLength)
                    {
                        rules.Add(rule);
                        Console.WriteLine(rule.ToString());
                    }
                    itemset.Remove(i);
                }
            }
        }
        else
        {
            for (int i = excelFile1.SubsetsCount; i < excelFile1.PropertyNames.Count; i++)
            {
                itemset.Add(i);
                RuleClass rule = new RuleClass(itemset, target);
                if (rule.confidence > needConfidence & rule.quality > needQuality & rule.itemset.Count <= sendingLength)
                {
                    rules.Add(rule);
                    Console.WriteLine(rule.ToString());
                }
                itemset.Remove(i);
            }
        }
    }

    public static void GenerateAllRules(int target, double needConfidence, double needQuality, double sendingLength, ExcelFile excelFile1)
    {
        List<RuleClass> rules = new List<RuleClass>();

        HashSet<int> itemset = new HashSet<int>();

        if (target == -1)
        {
            for (int j = 0; j < excelFile1.SubsetsCount; ++j)
            {
                for (int i = excelFile1.SubsetsCount; i < excelFile1.PropertyNames.Count; i++)
                {
                    itemset.Add(i);
                    RuleClass rule = new RuleClass(itemset, j);
                    if (rule.confidence > needConfidence & rule.quality > needQuality & rule.itemset.Count <= sendingLength)
                    {
                        rules.Add(rule);
                        Console.WriteLine(rule.ToString());
                    }
                    itemset.Remove(i);
                }
            }
        }
        else
        {
            for (int i = excelFile1.SubsetsCount; i < excelFile1.PropertyNames.Count; i++)
            {
                RuleClass rule = new RuleClass(itemset, target);
                if (rule.confidence > needConfidence & rule.quality > needQuality & rule.itemset.Count <= sendingLength)
                {
                    rules.Add(rule);
                    Console.WriteLine(rule.ToString());
                }
            }
        }
    }
    public static HashSet<int> GenerateLargerItemsets(HashSet<int> previousItemsets, ExcelFile excelFile1, int currentLength)
    {
        if (previousItemsets.Count == currentLength)
        {
            return previousItemsets;
        }

        HashSet<int> largerItemsets = new HashSet<int>();
        for (int i = previousItemsets.Last() + 1; i < excelFile1.PropertyNames.Count; i++)
        {
            if (AA.CountSupport(previousItemsets, ))
        }

        if (largerItemsets.Count > 0)
        {
            largerItemsets.AddRange(GenerateLargerItemsets(largerItemsets));
        }

        return largerItemsets;
    }
}


