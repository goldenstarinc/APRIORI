using Aspose.Cells.Drawing;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor
{
    public static class AprioriAlgorithm
    {
        /// <summary>
        /// Считает количество вхождений определенного набора бинарных свойств в подмножестве
        /// </summary>
        /// <param name="itemset"> Определенный набор бинарных свойств</param>
        /// <param name="subset"> Подмножество</param>
        /// <param name="propertiesCount"> Количество бинарных свойств</param>
        /// <returns> Количество вхождений определенного набора бинарных свойств в подмножестве</returns>
        public static int CountSupport(HashSet<int> itemset, List<string> subset, int propertiesCount)
        {
            // Счетчик 
            int count = 0;

            // Проходим по каждой записи в подмножестве
            foreach (var binarySet in subset) //O(n)
            {
                bool isAdequate = true;

                // Проверяем наличие каждого свойства в itemset
                foreach (int item in itemset)//O(m)
                {
                    if (binarySet[binarySet.Length - 1 - item] != '1')
                    {
                        isAdequate = false;
                        break;
                    }
                }

                if (isAdequate) count++;
            }

            return count;
        }

        /// <summary>
        /// Вычисляет частоту вхождения определенного набора бинарных свойств в множестве/подмножестве
        /// </summary>
        /// <param name="support"> Число вхождения определенного набора бинарных свойств</param>
        /// <param name="transactionsCount"> Количество записей в множестве/подмножестве</param>
        /// <returns> Частоту</returns>
        public static double GetFrequency(int support, int transactionsCount)
        {
            return ((double)support / transactionsCount);
        }

        /// <summary>
        /// Вычисляет достоверность правила
        /// </summary>
        /// <param name="itemset"> Набор определенных бинарных свойств</param>
        /// <param name="subset"> Подмножество</param>
        /// <returns> Достоверность правила</returns>
        public static double CalculateConfidence(HashSet<int> itemset, List<string> subset, List<string> transactions, int propertiesCount)
        {
            int subsetSupport = CountSupport(itemset, subset, propertiesCount);
            int setSupport = CountSupport(itemset, transactions, propertiesCount);
            return (double)subsetSupport / setSupport;
        }

        /// <summary>
        /// Вычисляет вероятность существования посылки в определённом подмножестве
        /// </summary>
        /// <param name="confidence"> Достоверность правила</param>
        /// <param name="subsetCount"> Количество записей в подмножестве</param>
        /// <param name="transactionsCount"> Количество записей множестве</param>
        /// <returns>Вероятность существования посылки в определённом подмножестве</returns>
        public static double CalculateLift(double confidence, int subsetCount, int transactionsCount)
        {
            return confidence * ((double)transactionsCount / subsetCount);
        }

        /// <summary>
        /// Вычисляет корреляцию между посылкой правила и его значением
        /// </summary>
        /// <param name="lift"> Лифт правила</param>
        /// <param name="subsetCount"> Количество записей в подмножестве</param>
        /// <param name="transactionsCount"> Количество записей множестве</param>
        /// <returns> Корреляция между посылкой правила и его значением</returns>
        public static double CalculateCorrelation(double lift, int subsetCount, int transactionsCount)
        {
            double correlation = 0;
            double maxlift = (double)transactionsCount / subsetCount;

            correlation = lift - 1;

            if ((lift >= 1) && (lift <= maxlift))
            {
                correlation = (double)(lift - 1) / (maxlift - 1);
            }

            return correlation;
        }

        /// <summary>
        /// Вычисляет качество правило
        /// </summary>
        /// <param name="f_g"> Вычисляет частоту вхождения определенного набора бинарных свойств в подмножестве</param>
        /// <param name="f_all"> Вычисляет частоту вхождения определенного набора бинарных свойств в множестве</param>
        /// <param name="confidence"> Достоверность правила</param>
        /// <param name="correlation"> Корреляция между посылкой правила и его значением</param>
        /// <param name="p1"> Вес 1</param>
        /// <param name="p2"> Вес 2</param>
        /// <param name="p3"> Вес 3</param>
        /// <param name="p4"> Вес 4</param>
        /// <returns> Качество правила</returns>
        public static double CalculateQuality(double f_g, double f_all, double confidence, double correlation, double p1 = 1, double p2 = 1, double p3 = 1, double p4 = 1)
        {
            return f_all * p1 + f_g * p2 + confidence * p3 + correlation * p4;
        }

        /// <summary>
        /// Генерация правил по выбранной посылке
        /// </summary>
        /// <param name="target">Номер подмножества</param>
        /// <param name="itemset">Посылка</param>
        /// <param name="encryptedData">Объект, хранящий информацию о зашифрованном файле</param>
        public static List<RuleClass> GenerateSpecificRules(int target, HashSet<int> itemset, DataEncryptor encryptedData)
        {
            List<RuleClass> rules = new List<RuleClass>();

            ExcelFile excelFile1 = encryptedData._metaFile;

            // В случае, когда target = -1 работаем со всеми подмножествами
            if (target == -1)
            {
                // Проходим по каждому подмножеству
                for (int i = 0; i < excelFile1.SubsetsCount; ++i)
                {
                    RuleClass rule = new RuleClass(new HashSet<int>(itemset), i, encryptedData);
                    rules.Add(rule);
                }
            }
            else
            {
                // Проходим по заданному подмножеству
                RuleClass rule = new RuleClass(new HashSet<int>(itemset), target, encryptedData);
                rules.Add(rule);
            }
            return rules;
        }

        /// <summary>
        /// Генерация единичных правил согласно заданным параметрам
        /// </summary>
        /// <param name="selcetedTarget">Номер подмножества</param>
        /// <param name="needQuality">Достаточное качество</param>
        /// <param name="encryptedData">Объект, хранящий информацию о зашифрованном файле</param>
        public static List<RuleClass> GenerateSingleRulesUsingQuality(int selcetedTarget, double needQuality, DataEncryptor encryptedData)
        {
            List<RuleClass> rules = new List<RuleClass>();

            int subsetsCount = encryptedData._subsets.Count; // Количество подмножеств
            int propertiesCount = encryptedData._propertiesCount; // Количество бинарных свойств

            // В случае, когда target = -1 работаем со всем множеством
            if (selcetedTarget == -1)
            {
                for (int target = 0; target < subsetsCount; ++target)
                {
                    // Проходим по каждому бинарному свойству исключая целовой столбец и строим правила
                    for (int i = subsetsCount; i < propertiesCount; ++i)
                    {
                        RuleClass rule = new RuleClass(new HashSet<int> { i }, target, encryptedData);

                        if (rule.quality >= needQuality)
                        {
                            rules.Add(rule);
                        }
                    }
                }
            }
            // В ином случае, работаем с выбранным пожмножеством
            else
            {
                // Проходим по каждому бинарному свойству исключая целовой столбец и строим правила
                for (int i = subsetsCount; i < propertiesCount; ++i)
                {
                    RuleClass rule = new RuleClass(new HashSet<int> { i }, selcetedTarget, encryptedData);
                    if (rule.quality >= needQuality)
                    {
                        rules.Add(rule);
                    }
                }
            }

            return rules;
        }



        /// <summary>
        /// Генерация единичных правил согласно заданным параметрам
        /// </summary>
        /// <param name="selcetedTarget">Номер подмножества</param>
        /// <param name="needCorrelation">Достаточная корреляция</param>
        /// <param name="encryptedData">Объект, хранящий информацию о зашифрованном файле</param>
        public static List<RuleClass> GenerateSingleRulesUsingCorrelation(int selcetedTarget, double needCorrelation, DataEncryptor encryptedData)
        {
            List<RuleClass> rules = new List<RuleClass>();

            int subsetsCount = encryptedData._subsets.Count; // Количество подмножеств
            int propertiesCount = encryptedData._propertiesCount; // Количество бинарных свойств

            // В случае, когда target = -1 работаем со всем множеством
            if (selcetedTarget == -1)
            {
                for (int target = 0; target < subsetsCount; ++target)
                {
                    // Проходим по каждому бинарному свойству исключая целовой столбец и строим правила
                    for (int i = subsetsCount; i < propertiesCount; ++i)
                    {
                        RuleClass rule = new RuleClass(new HashSet<int> { i }, target, encryptedData);

                        if (rule.correlation >= needCorrelation)
                        {
                            rules.Add(rule);
                        }
                    }
                }
            }
            // В ином случае, работаем с выбранным пожмножеством
            else
            {
                // Проходим по каждому бинарному свойству исключая целовой столбец и строим правила
                for (int i = subsetsCount; i < propertiesCount; ++i)
                {
                    RuleClass rule = new RuleClass(new HashSet<int> { i }, selcetedTarget, encryptedData);
                    if (rule.correlation >= needCorrelation)
                    {
                        rules.Add(rule);
                    }
                }
            }

            return rules;
        }



        /// <summary>
        /// Генерация единичных правил согласно заданным параметрам
        /// </summary>
        /// <param name="selcetedTarget">Номер подмножества</param>
        /// <param name="needConfidenceMin">Достаточная достоверность</param>
        /// <param name="needFrequency">Достаточная частота</param>
        /// <param name="encryptedData">Объект, хранящий информацию о зашифрованном файле</param>
        /// <param name="needConfidenceMax">Максимально допустимое значение достоверности</param>
        public static List<RuleClass> GenerateSingleRulesUsingConfidenceAndFrequency(int selcetedTarget, double needConfidenceMin, double needFrequency, DataEncryptor encryptedData, double needConfidenceMax = 1)
        {
            List<RuleClass> rules = new List<RuleClass>();

            int subsetsCount = encryptedData._subsets.Count; // Количество подмножеств
            int propertiesCount = encryptedData._propertiesCount; // Количество бинарных свойств

            // В случае, когда target = -1 работаем со всем множеством
            if (selcetedTarget == -1)
            {
                for (int target = 0; target < subsetsCount; ++target)
                {
                    // Проходим по каждому бинарному свойству исключая целовой столбец и строим правила
                    for (int i = subsetsCount; i < propertiesCount; ++i)
                    {
                        RuleClass rule = new RuleClass(new HashSet<int> { i }, target, encryptedData);

                        if (rule.f_g >= needFrequency && rule.confidence >= needConfidenceMin && rule.confidence <= needConfidenceMax)
                        {
                            rules.Add(rule);
                        }
                    }
                }
            }
            // В ином случае, работаем с выбранным пожмножеством
            else
            {
                // Проходим по каждому бинарному свойству исключая целовой столбец и строим правила
                for (int i = subsetsCount; i < propertiesCount; ++i)
                {
                    RuleClass rule = new RuleClass(new HashSet<int> { i }, selcetedTarget, encryptedData);
                    if (rule.f_g >= needFrequency && rule.confidence >= needConfidenceMin && rule.confidence <= needConfidenceMax)
                    {
                        rules.Add(rule);
                    }
                }
            }

            return rules;
        }

        /// <summary>
        /// Генерация всех правил согласно заданным параметрам
        /// </summary>
        /// <param name="target">Номер подмножества</param>
        /// <param name="needQuality">Достаточное качество</param>
        /// <param name="sendingLength">Достаточная длина посылки</param>
        /// <param name="encryptedData">Объект, хранящий информацию о зашифрованном файле</param>
        public static List<RuleClass> GenerateAllRulesUsingQuality(int target, double needQuality, int sendingLength, DataEncryptor encryptedData)
        {
            List<RuleClass> rules = new List<RuleClass>();

            if (target == -1)
            {
                for (int i = 0; i < encryptedData._subsets.Count; ++i) //O(n)
                {
                    var sets = GenerateLargerItemsets(sendingLength, i, encryptedData); //O()

                    foreach (var set in sets)
                    {
                        RuleClass rule = new RuleClass(set, i, encryptedData);

                        if (rule.quality >= needQuality)
                        {
                            rules.Add(rule);
                        }
                    }
                }
            }
            else
            {
                var sets = GenerateLargerItemsets(sendingLength, target, encryptedData);
                
                foreach (var set in sets)
                {
                    RuleClass rule = new RuleClass(set, target, encryptedData);

                    if (rule.quality >= needQuality)
                    {
                        rules.Add(rule);
                    }
                }
            }
            
            return rules;
        }



        /// <summary>
        /// Генерация всех правил согласно заданным параметрам
        /// </summary>
        /// <param name="target">Номер подмножества</param>
        /// <param name="needCorrelation">Достаточная корреляция</param>
        /// <param name="sendingLength">Достаточная длина посылки</param>
        /// <param name="encryptedData">Объект, хранящий информацию о зашифрованном файле</param>
        public static List<RuleClass> GenerateAllRulesUsingCorrelation(int target, double needCorrelation, int sendingLength, DataEncryptor encryptedData)
        {
            List<RuleClass> rules = new List<RuleClass>();

            if (target == -1)
            {
                for (int i = 0; i < encryptedData._subsets.Count; ++i) //O(n)
                {
                    var sets = GenerateLargerItemsets(sendingLength, i, encryptedData); //O()

                    foreach (var set in sets)
                    {
                        RuleClass rule = new RuleClass(set, i, encryptedData);

                        if (rule.correlation >= needCorrelation)
                        {
                            rules.Add(rule);
                        }
                    }
                }
            }
            else
            {
                var sets = GenerateLargerItemsets(sendingLength, target, encryptedData);

                foreach (var set in sets)
                {
                    RuleClass rule = new RuleClass(set, target, encryptedData);

                    if (rule.correlation >= needCorrelation)
                    {
                        rules.Add(rule);
                    }
                }
            }

            return rules;
        }



        /// <summary>
        /// Генерация всех правил согласно заданным параметрам
        /// </summary>
        /// <param name="target">Номер подмножества</param>
        /// <param name="needConfidenceMin">Минимальное значение достоверности</param>
        /// <param name="needFrequency">Достаточная частота</param>
        /// <param name="sendingLength">Достаточная длина посылки</param>
        /// <param name="encryptedData">Объект, хранящий информацию о зашифрованном файле</param>
        /// <param name="needConfidenceMax">Максимально допустимое значение достоверности</param>
        public static List<RuleClass> GenerateAllRulesUsingConfidenceAndFrequency(int target, double needConfidenceMin, double needFrequency, int sendingLength, DataEncryptor encryptedData, double needConfidenceMax = 1)
        {
            List<RuleClass> rules = new List<RuleClass>();

            if (target == -1)
            {
                for (int i = 0; i < encryptedData._subsets.Count; ++i) //O(n)
                {
                    var sets = GenerateLargerItemsets(sendingLength, i, encryptedData); //O()

                    foreach (var set in sets)
                    {
                        RuleClass rule = new RuleClass(set, i, encryptedData);

                        if (rule.confidence >= needConfidenceMin && rule.confidence <= needConfidenceMax && rule.f_g >= needFrequency)
                        {
                            rules.Add(rule);
                        }
                    }
                }
            }
            else
            {
                var sets = GenerateLargerItemsets(sendingLength, target, encryptedData);

                foreach (var set in sets)
                {
                    RuleClass rule = new RuleClass(set, target, encryptedData);

                    if (rule.confidence >= needConfidenceMin && rule.confidence <= needConfidenceMax && rule.f_g >= needFrequency)
                    {
                        rules.Add(rule);
                    }
                }
            }

            return rules;
        }

        /// <summary>
        /// Генерирует наборы элементов большего размера
        /// </summary>
        /// <param name="sendingLength">Максимальная длина наборов</param>
        /// <param name="target">Подмножество</param>
        /// <param name="ed">Объект, хранящий информацию о зашифрованном файле</param>
        /// <returns>Наборы элементов большего размера</returns>
        public static List<HashSet<int>> GenerateLargerItemsets(int sendingLength, int target, DataEncryptor ed)
        {
            // Список, хранящий все возможные наборы, прошедшие через заданный фильтр
            List<HashSet<int>> largerItemsets = new List<HashSet<int>>();

            HashSet<int> singleSets = new HashSet<int>();

            // Минимальная частота
            double minF = 30;
            double currentF;

            for (int i = ed._subsets.Count; i < ed._propertiesCount; ++i) //O(n)
            {

                currentF = GetFrequency(CountSupport(new HashSet<int>() { i }, ed._subsets[target], ed._propertiesCount), ed._subsets[target].Count) * 100;//O(m)

                if (currentF > minF)
                {
                    singleSets.Add(i);
                    largerItemsets.Add(new HashSet<int>() { i });
                }
            }

            int previousCount = 0;

            // Генерируем наборы размера sendingLength
            for (int y = 2; y <= sendingLength; ++y)//O(n)
            {
                largerItemsets.AddRange(GenerateCombinations(singleSets, y, ed, target));
                if (largerItemsets.Count != previousCount)
                {
                    previousCount = largerItemsets.Count;
                }
                else { break; }
            }

            return largerItemsets;
        }

        /// <summary>
        /// Создает все возможные комбинации заданной длины из элементов, представленных в items
        /// </summary>
        /// <param name="items">Посылка</param>
        /// <param name="combinationLength">Длина комбинации</param>
        /// <param name="ed">Объект, хранящий информацию о зашифрованном файле</param>
        /// <param name="target">Подмножество</param>
        /// <returns>Все возможные комбинации заданной длины из элементов, представленных в items</returns>
        public static List<HashSet<int>> GenerateCombinations(HashSet<int> items, int combinationLength, DataEncryptor ed, int target)
        {
            List<HashSet<int>> combinations = new List<HashSet<int>>();
            List<int> currentCombination = new List<int>();

            GenerateCombinationsRecursive(items.ToList(), combinationLength, 0, currentCombination, combinations, ed, target);
            return combinations;
        }

        /// <summary>
        /// Рекурсивно формирует комбинации заданной длины
        /// </summary>
        /// <param name="items">Посылка</param>
        /// <param name="combinationLength">Длина требуемой комбинации</param>
        /// <param name="start">Индекс, с которого следует начинать поиск комбинаций</param>
        /// <param name="currentCombination">Текущая комбинация элементов</param>
        /// <param name="combinations">Список, в который добавляются корректные комбинации</param>
        /// <param name="ed">Объект, хранящий информацию о зашифрованном файле</param>
        /// <param name="target">Подмножество</param>

        private static void GenerateCombinationsRecursive(List<int> items, int combinationLength, int start,
                                                          List<int> currentCombination, List<HashSet<int>> combinations, DataEncryptor ed, int target)//O()
        {
            if (currentCombination.Count == combinationLength)
            {
                HashSet<int> combination = new HashSet<int>(currentCombination);
                double minF = 30;
                double currentF = GetFrequency(CountSupport(combination, ed._subsets[target], ed._propertiesCount), ed._subsets[target].Count) * 100;

                if (currentF > minF)
                {
                    combinations.Add(combination);
                }
                return;
            }

            for (int i = start; i < items.Count; i++)//O(n)
            {
                currentCombination.Add(items[i]);
                GenerateCombinationsRecursive(items, combinationLength, i + 1, currentCombination, combinations, ed, target);//O()
                currentCombination.RemoveAt(currentCombination.Count - 1); // Backtrack
            }
        }
    }
}





