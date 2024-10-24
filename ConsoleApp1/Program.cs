﻿using Aspose.Cells;
using DataProcessor;
using System;
using System.Collections.Generic;
using System.Numerics;
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

        //foreach(BigInteger record in dataEncryptor1._transactions)
        //{
        //    Console.WriteLine(record);
        //}

        HashSet<int> itemset = new HashSet<int> { 3, 10 };



        // Генерация правил
        //List<RuleClass> rules = AA.GenerateSingleRules(-1, 0, 0, dataEncryptor1);

        //List<RuleClass> rules = AA.GenerateAllRules(1, 0, 2, 6, dataEncryptor1);


        List<RuleClass> rules = AA.GenerateSpecificRules(-1, itemset, dataEncryptor1);

        // Вывод правил в консоль
        foreach (RuleClass rule in rules)
        {
            Console.WriteLine(rule.ToString());
        }
    }
}
