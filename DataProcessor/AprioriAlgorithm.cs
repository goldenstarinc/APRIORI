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
using System.Collections.Concurrent;
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

        public static ConcurrentDictionary<(HashSet<int>, int), int> ItemsetSupportCache = new ConcurrentDictionary<(HashSet<int>, int), int>();
        public static int CountSupport(HashSet<int> itemset, List<string> subset, int target, int propertiesCount)
        {
            if (ItemsetSupportCache.ContainsKey((itemset, target)))
                return ItemsetSupportCache[(itemset, target)];

            // Счетчик 
            int count = 0;

            // Проходим по каждой записи в подмножестве
            foreach (var binarySet in subset)
            {
                bool isAdequate = true;

                // Проверяем наличие каждого свойства в itemset
                foreach (int item in itemset)
                {
                    if (binarySet[binarySet.Length - 1 - item] != '1')
                    {
                        isAdequate = false;
                        break;
                    }
                }

                if (isAdequate) count++;
            }

            ItemsetSupportCache[(itemset, target)] = count;
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
        public static double CalculateConfidence(HashSet<int> itemset, List<string> subset, List<string> transactions, int propertiesCount, int target)
        {
            int subsetSupport = CountSupport(itemset, subset, target, propertiesCount);
            int setSupport = CountSupport(itemset, transactions, -1, propertiesCount);
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
        public static List<RuleClass> GenerateSingleRules(int selcetedTarget, DataEncryptor encryptedData)
        {
            List<RuleClass> rules = new List<RuleClass>();

            int subsetsCount = encryptedData._subsets.Count; // Количество подмножеств
            int propertiesCount = encryptedData._propertiesCount; // Количество бинарных свойств

            // Работаем со всем множеством
            if (selcetedTarget == -1)
            {
                // Распараллеливаем построение правил
                Parallel.For(0, encryptedData._subsets.Count, target =>
                {
                    // Проходим по каждому бинарному свойству исключая целовой столбец и строим правила
                    for (int i = subsetsCount; i < propertiesCount; ++i)
                    {
                        // Строим правило
                        RuleClass rule = new RuleClass(new HashSet<int> { i }, target, encryptedData);

                        rules.Add(rule);
                    }
                });
            }
            // В ином случае, работаем с выбранным пожмножеством
            else
            {
                // Проходим по каждому бинарному свойству исключая целовой столбец и строим правила
                for (int i = subsetsCount; i < propertiesCount; ++i)
                {
                    // Строим правило
                    RuleClass rule = new RuleClass(new HashSet<int> { i }, selcetedTarget, encryptedData);

                    rules.Add(rule);
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
        public static List<RuleClass> GenerateAllRules(int target, int sendingLength, DataEncryptor encryptedData, string mode,
                                                                double? needQuality = null,
                                                                double? needCorrelation = null,
                                                                double? needFrequency = null, double? needMinConfidence = null, double? needMaxConfidence = null)
        {
            List<RuleClass> rules = new List<RuleClass>();
            
            // Работаем со всем множеством
            if (target == -1)
            {
                // Распараллеливаем построение правил
                Parallel.For(0, encryptedData._subsets.Count, i =>
                {
                    // Генерируем список всех возможных комбинаций(сетов)
                    var sets = GenerateLargerItemsets(sendingLength, i, encryptedData, mode, needQuality, needCorrelation, needFrequency, needMinConfidence, needMaxConfidence);

                    // Проходим по каждому сету
                    foreach (var set in sets)
                    {
                        // Создаем правило
                        RuleClass rule = new RuleClass(set, i, encryptedData);

                        rules.Add(rule);
                    }
                });
            }
            else
            {
                // Генерируем список всех возможных комбинаций(сетов)
                var sets = GenerateLargerItemsets(sendingLength, target, encryptedData, mode, needQuality, needCorrelation, needFrequency, needMinConfidence, needMaxConfidence);

                // Проходим по каждому сету
                foreach (var set in sets)
                {
                    // Создаем правило
                    RuleClass rule = new RuleClass(set, target, encryptedData);

                    rules.Add(rule);
                }
            }

            return rules;
        }

        /// <summary>
        /// Фильтрует список правил на основе заданного значения качества
        /// </summary>
        /// <param name="rules">Список правил</param>
        /// <param name="needQuality">Достаточное качество</param>
        /// <returns>Список отфильтрованных правил по качеству</returns>
        public static List<RuleClass> FilterRulesByQuality(List<RuleClass> rules, double needQuality)
        {
            List<RuleClass> FilteredRules = new List<RuleClass>();

            // Проходим по каждому правилу
            foreach (RuleClass rule in rules)
            {
                // Если значение качества правила ниже заданного порога, удаляем правило из списка
                if (rule.quality >= needQuality)
                {
                    FilteredRules.Add(rule);
                }
            }

            return FilteredRules;
        }

        /// <summary>
        /// Фильтрует список правил на основе заданного значения корреляции
        /// </summary>
        /// <param name="rules">Список правил</param>
        /// <param name="needCorrelation">Достаточное качество</param>
        /// <returns>Список отфильтрованных правил по корреляции</returns>
        public static List<RuleClass> FilterRulesByCorrelation(List<RuleClass> rules, double needCorrelation)
        {
            List<RuleClass> FilteredRules = new List<RuleClass>();

            // Проходим по каждому правилу
            foreach (RuleClass rule in rules)
            {
                // Если значение корреляции правила ниже заданного порога, удаляем правило из списка
                if (rule.correlation >= needCorrelation)
                {
                    FilteredRules.Add(rule);
                }
            }

            return FilteredRules;
        }

        /// <summary>
        /// Фильтрует список правил на основе заданного значения частоты и достоверности
        /// </summary>
        /// <param name="rules">Список правил</param>
        /// <param name="needFrequency">Достаточная частота</param>
        /// <param name="needMinConfidence">Минимальный порог достоверности</param>
        /// <param name="needMaxConfidence">Максимальный порог достоверности</param>
        /// <returns>Список отфильтрованных правил по частоте и достоверности</returns>
        public static List<RuleClass> FilterRulesByFrequencyAndConfidence(List<RuleClass> rules, double needFrequency, double needMinConfidence, double needMaxConfidence = 1)
        {
            List<RuleClass> FilteredRules = new List<RuleClass>();

            // Проходим по каждому правилу
            foreach (RuleClass rule in rules)
            {
                // Если значение частоты правила или его достоверности ниже заданного порога, удаляем правило из списка
                if (rule.f_g >= needFrequency && rule.confidence >= needMinConfidence && rule.confidence <= needMaxConfidence)
                {
                    FilteredRules.Add(rule);
                }
            }

            return FilteredRules;
        }

        /// <summary>
        /// Генерирует наборы элементов большего размера
        /// </summary>
        /// <param name="sendingLength">Максимальная длина наборов</param>
        /// <param name="target">Подмножество</param>
        /// <param name="ed">Объект, хранящий информацию о зашифрованном файле</param>
        /// <returns>Наборы элементов большего размера</returns>
        public static List<HashSet<int>> GenerateLargerItemsets(int sendingLength, int target, DataEncryptor ed, string mode, 
                                                                double? needQuality = null, 
                                                                double? needCorrelation = null, 
                                                                double? needFrequency = null, double? needMinConfidence = null, double? needMaxConfidence = null)
        {
            // Список, хранящий все возможные наборы, прошедшие через заданный фильтр
            List<HashSet<int>> largerItemsets = new List<HashSet<int>>();

            HashSet<int> singleSets = new HashSet<int>();


            // Генерируем всевозможные единичные наборы на основе заданных фильтров

            for (int i = ed._subsets.Count; i < ed._propertiesCount; ++i)
            {
                switch (mode)
                {
                    case "Качество":
                        if (CompareByQuality(needQuality, new HashSet<int>() { i }, target, ed))
                        {
                            singleSets.Add(i);
                            largerItemsets.Add(new HashSet<int>() { i });
                        }
                        break;
                    case "Корреляция":
                        if (CompareByCorrelation(needCorrelation, new HashSet<int>() { i }, target, ed))
                        {
                            singleSets.Add(i);
                            largerItemsets.Add(new HashSet<int>() { i });
                        }
                        break;
                    case "Достоверность и частота":
                        if (CompareByConfidenceAndFrequency(new HashSet<int>() { i }, target, ed, needFrequency, needMinConfidence))
                        {
                            singleSets.Add(i);
                            largerItemsets.Add(new HashSet<int>() { i });
                        }
                        break;
                    case "Достоверность и частота(диапазон)":
                        if (CompareByConfidenceAndFrequency(new HashSet<int>() { i }, target, ed, needFrequency, needMinConfidence, needMaxConfidence))
                        {
                            singleSets.Add(i);
                            largerItemsets.Add(new HashSet<int>() { i });
                        }
                        break;
                }
            }

            // Счетчик построенных правил
            int previousCount = 0;

            // Генерируем наборы размера sendingLength
            for (int y = 2; y <= sendingLength; ++y)
            {
                largerItemsets.AddRange(GenerateCombinations(singleSets, y, ed, target, mode, needQuality, needCorrelation, needFrequency, needMinConfidence, needMaxConfidence));

                // Если число наборов изменилось продолжаем строить правила
                if (largerItemsets.Count != previousCount)
                {
                    previousCount = largerItemsets.Count;
                } // Иначе выходим
                else { break; }
            }

            return largerItemsets;
        }

        /// <summary>
        /// Создает все возможные комбинации заданной длины из элементов, представленных в items
        /// </summary>
        /// <param name="items">Список, содержащий единичные комбинации, прошедшие порог</param>
        /// <param name="combinationLength">Длина комбинации</param>
        /// <param name="ed">Объект, хранящий информацию о зашифрованном файле</param>
        /// <param name="target">Подмножество</param>
        /// <returns>Все возможные комбинации заданной длины из элементов, представленных в items</returns>
        public static List<HashSet<int>> GenerateCombinations(HashSet<int> items, int combinationLength, DataEncryptor ed, int target, string mode,
                                                              double? needQuality = null,
                                                              double? needCorrelation = null,
                                                              double? needFrequency = null, double? needMinConfidence = null, double? needMaxConfidence = null)
        {
            List<HashSet<int>> combinations = new List<HashSet<int>>();
            List<int> currentCombination = new List<int>();

            // Создаем комбинации нужной длины
            GenerateCombinationsRecursive(items.ToList(), combinationLength, 0, currentCombination, combinations, ed, target, mode, needQuality, needCorrelation, needFrequency, needMinConfidence, needMaxConfidence);
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
                                                          List<int> currentCombination, List<HashSet<int>> combinations, DataEncryptor ed, int target, string mode,
                                                          double? needQuality = null,
                                                          double? needCorrelation = null,
                                                          double? needFrequency = null, double? needMinConfidence = null, double? needMaxConfidence = null)
        {
            // Если комбинация достигла нужной длины
            if (currentCombination.Count == combinationLength)
            {
                HashSet<int> combination = new HashSet<int>(currentCombination);

                switch (mode)
                {
                    case "Качество":
                        if (CompareByQuality(needQuality, combination, target, ed))
                        {
                            combinations.Add(combination);
                        }
                        break;
                    case "Корреляция":
                        if (CompareByCorrelation(needCorrelation, combination, target, ed))
                        {
                            combinations.Add(combination);
                        }
                        break;
                    case "Достоверность и частота":
                        if (CompareByConfidenceAndFrequency(combination, target, ed, needFrequency, needMinConfidence))
                        {
                            combinations.Add(combination);
                        }
                        break;
                    case "Достоверность и частота(диапазон)":
                        if (CompareByConfidenceAndFrequency(combination, target, ed, needFrequency, needMinConfidence, needMaxConfidence))
                        {
                            combinations.Add(combination);
                        }
                        break;
                }
                return;
            }

            // Генерируем комбинацию
            for (int i = start; i < items.Count; i++)
            {
                currentCombination.Add(items[i]);
                GenerateCombinationsRecursive(items, combinationLength, i + 1, currentCombination, combinations, ed, target, mode, needQuality, needCorrelation, needFrequency, needMinConfidence, needMaxConfidence);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }

        /// <summary>
        /// Проверяет, подходит ли заданная комбинация по качеству
        /// </summary>
        /// <param name="needQuality">Достаточное качество</param>
        /// <param name="combination">Комбинация</param>
        /// <param name="target">Номер правила</param>
        /// <param name="ed">Зашифрованные данные</param>
        /// <returns>True - если комбинация подходит, false - если нет</returns>
        public static bool CompareByQuality(double? needQuality, HashSet<int> combination, int target, DataEncryptor ed)
        {
            int TC = ed._transactions.Count; // количество записей в базе данных
            int SC = ed._subsets[target].Count; // количество записей в подмножестве
            int PC = ed._propertiesCount; // количество бинарных свойств

            int Support = CountSupport(combination, ed._subsets[target], target, PC);

            if (Support == 0)
            {
                return false;
            }

            double F_all = GetFrequency(Support, TC);
            double F_g = GetFrequency(Support, SC);
            double Confidence = CalculateConfidence(combination, ed._subsets[target], ed._transactions, PC, target);
            double Lift = CalculateLift(Confidence, SC, TC);
            double Correlation = CalculateCorrelation(Lift, SC, TC);
            double Quality = CalculateQuality(F_g, F_all, Confidence, Correlation);

            // Проверка значения
            if (Quality >= needQuality)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Проверяет, подходит ли заданная комбинация по корреляции
        /// </summary>
        /// <param name="needCorrelation">Достаточная корреляция</param>
        /// <param name="combination">Комбинация</param>
        /// <param name="target">Номер правила</param>
        /// <param name="ed">Зашифрованные данные</param>
        /// <returns>True - если комбинация подходит, false - если нет</returns>
        public static bool CompareByCorrelation(double? needCorrelation, HashSet<int> combination, int target, DataEncryptor ed)
        {
            int TC = ed._transactions.Count; // количество записей в базе данных
            int SC = ed._subsets[target].Count; // количество записей в подмножестве
            int PC = ed._propertiesCount; // количество бинарных свойств


            int Support = CountSupport(combination, ed._subsets[target], target, PC);

            if (Support == 0)
            {
                return false;
            }

            double F_all = GetFrequency(Support, TC);
            double F_g = GetFrequency(Support, SC);
            double Confidence = CalculateConfidence(combination, ed._subsets[target], ed._transactions, PC, target);
            double Lift = CalculateLift(Confidence, SC, TC);
            double Correlation = CalculateCorrelation(Lift, SC, TC);

            // Проверка значения
            if (Correlation >= 0 && Correlation >= needCorrelation)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Проверяет, подходит ли заданная комбинация по частоте и достоверности
        /// </summary>
        /// <param name="combination">Комбинация</param>
        /// <param name="target">Номер правила</param>
        /// <param name="ed">Зашифрованные данные</param>
        /// <param name="needFrequency">Достаточная частота</param>
        /// <param name="needMinConfidence">Нижняя граница значения достаточной достоверности</param>
        /// <param name="needMaxConfidence">Верхняя граница значения достаточной достоверности</param>
        /// <returns>True - если комбинация подходит, false - если нет</returns>
        public static bool CompareByConfidenceAndFrequency(HashSet<int> combination, int target, DataEncryptor ed, double? needFrequency, double? needMinConfidence, double? needMaxConfidence = 1)
        {
            int TC = ed._transactions.Count; // количество записей в базе данных
            int SC = ed._subsets[target].Count; // количество записей в подмножестве 
            int PC = ed._propertiesCount; // количество бинарных свойств

            int Support = CountSupport(combination, ed._subsets[target], target, PC);

            if (Support == 0)
            {
                return false;
            }

            double F_g = GetFrequency(Support, SC);
            double Confidence = CalculateConfidence(combination, ed._subsets[target], ed._transactions, PC, target);

            // Проверка значения
            if (F_g >= needFrequency && Confidence >= needMinConfidence && Confidence <= needMaxConfidence)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// В разработке
        /// </summary>
        public static List<RuleClass> GenerateAllRulesIteratively(int target, int maxLength, DataEncryptor encryptedData, string mode,
                                                         double? needQuality = null,
                                                         double? needCorrelation = null,
                                                         double? needFrequency = null, double? needMinConfidence = null, double? needMaxConfidence = null)
        {
            List<RuleClass> Rules = new List<RuleClass>();

            // Если target равен -1, перебираем все подмножества
            if (target == -1)
            {
                Parallel.For(0, encryptedData._subsets.Count, t =>
                {
                    // Генерируем правила для каждого подмножества
                    Rules.AddRange(GenerateRulesForTarget(t, maxLength, encryptedData, mode, needQuality, needCorrelation, needFrequency, needMinConfidence, needMaxConfidence));
                });
            }
            else
            {
                // Генерируем правила для указанного подмножества
                Rules.AddRange(GenerateRulesForTarget(target, maxLength, encryptedData, mode, needQuality, needCorrelation, needFrequency, needMinConfidence, needMaxConfidence));
            }

            return Rules;
        }


        /// <summary>
        /// В разработке
        /// </summary>
        private static List<RuleClass> GenerateRulesForTarget(int target, int maxLength, DataEncryptor encryptedData, string mode,
                                                              double? needQuality, double? needCorrelation, double? needFrequency, double? needMinConfidence, double? needMaxConfidence)
        {
            // Структуры данных
            List<HashSet<int>> Candidates = new List<HashSet<int>>();
            List<HashSet<int>> Current = new List<HashSet<int>>();
            List<HashSet<int>> NextCurrent = new List<HashSet<int>>();
            List<RuleClass> Rules = new List<RuleClass>();

            // Инициализация кандидатов для комбинаций длины 1
            for (int i = encryptedData._subsets.Count; i < encryptedData._propertiesCount; i++)
            {
                HashSet<int> candidate = new HashSet<int> { i };
                if (IsValidCandidate(candidate, target, encryptedData, mode, needQuality, needCorrelation, needFrequency, needMinConfidence, needMaxConfidence))
                {
                    Candidates.Add(candidate);
                }
            }

            int k = 1; // Начинаем с длины наборов равной 1

            while (k <= maxLength)
            {
                // Если найдены действительные комбинации длины k, продолжаем
                if (Candidates.Count > 0)
                {
                    // Сохраняем текущие наборы как текущие
                    Current = new List<HashSet<int>>(Candidates);

                    // Создаем новые наборы длины k + 1
                    NextCurrent.Clear();
                    for (int i = 0; i < Current.Count; i++)
                    {
                        for (int j = i + 1; j < Current.Count; j++)
                        {
                            HashSet<int> newSet = new HashSet<int>(Current[i]);
                            newSet.UnionWith(Current[j]);

                            if (newSet.Count == k + 1 && IsValidCandidate(newSet, target, encryptedData, mode, needQuality, needCorrelation, needFrequency, needMinConfidence, needMaxConfidence))
                            {
                                NextCurrent.Add(newSet);
                            }
                        }
                    }

                    // Добавляем правила из текущих наборов
                    foreach (var set in Current)
                    {
                        Rules.Add(new RuleClass(set, target, encryptedData));
                    }

                    // Обновляем Candidates на следующий шаг
                    Candidates = new List<HashSet<int>>(NextCurrent);
                    k++;
                }
                else
                {
                    break; // Если Candidates пусты, завершаем
                }
            }

            return Rules;
        }

        /// <summary>
        /// В разработке
        /// </summary>
        private static bool IsValidCandidate(HashSet<int> candidate, int target, DataEncryptor encryptedData, string mode,
            double? needQuality, double? needCorrelation, double? needFrequency, double? needMinConfidence, double? needMaxConfidence)
        {
            switch (mode)
            {
                case "Качество":
                    return CompareByQuality(needQuality, candidate, target, encryptedData);
                case "Корреляция":
                    return CompareByCorrelation(needCorrelation, candidate, target, encryptedData);
                case "Достоверность и частота":
                    return CompareByConfidenceAndFrequency(candidate, target, encryptedData, needFrequency, needMinConfidence);
                case "Достоверность и частота(диапазон)":
                    return CompareByConfidenceAndFrequency(candidate, target, encryptedData, needFrequency, needMinConfidence, needMaxConfidence);
                default:
                    throw new ArgumentException("Неверный режим фильтрации.");
            }
        }
    }
}





