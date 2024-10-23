using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        public static int CountSupport(HashSet<int> itemset, List<BigInteger> subset, int propertiesCount)
        {
            // Счетчик 
            int count = 0;

            // Проходим по каждой записи в подмножестве
            foreach (var set in subset)
            {
                bool isAdequate = true;

                // Расшифровываем запись
                string binarySet = ToBinaryString(set, propertiesCount);

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
        public static double CalculateConfidence(HashSet<int> itemset, List<BigInteger> subset, List<BigInteger> transactions, int propertiesCount)
        {
            int subsetSupport = CountSupport(itemset, subset, propertiesCount);
            int setSupport = CountSupport(itemset, transactions, propertiesCount);
            return (double)subsetSupport / setSupport;
        }

        /// <summary>
        /// Вычисляет вероятность существования посылки в определённом подмножестве
        /// </summary>
        /// <param name="confidence"> Достоверность правила</param>
        /// <param name="subset"> Подмножество</param>
        /// <returns>Вероятность существования посылки в определённом подмножестве</returns>
        public static double CalculateLift(double confidence, List<BigInteger> subset, List<BigInteger> transactions)
        {
            return confidence * ((double)transactions.Count / subset.Count);
        }

        /// <summary>
        /// Вычисляет корреляцию между посылкой правила и его значением
        /// </summary>
        /// <param name="lift"> Лифт правила</param>
        /// <param name="subset"> Подмножество</param>
        /// <returns> Корреляция между посылкой правила и его значением</returns>
        public static double CalculateCorrelation(double lift, int subsetCount, int transactionsCount)
        {
            double correlation = 0;
            double maxlift = (double)transactionsCount / subsetCount;

            correlation = lift - 1;

            if ((lift >= 1) && (lift <= maxlift))
            {
                correlation = (lift - 1) / (maxlift - 1);
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
        /// <param name="target">Номер подмножества</param>
        /// <param name="needConfidence">Достаточная уверенность</param>
        /// <param name="needQuality">Достаточное качество</param>
        /// <param name="encryptedData">Объект, хранящий информацию о зашифрованном файле</param>
        public static List<RuleClass> GenerateSingleRules(int target, double needConfidence, double needQuality, DataEncryptor encryptedData)
        {
            List<RuleClass> rules = new List<RuleClass>();

            List<HashSet<int>> largerItemsets = new List<HashSet<int>>();

            ExcelFile excelFile1 = encryptedData._metaFile;

            HashSet<int> itemset = new HashSet<int>();

            // В случае, когда target = -1 работаем со всеми подмножествами
            if (target == -1)
            {
                // Проходим по каждому подмножеству
                for (int j = 0; j < excelFile1.SubsetsCount; ++j)
                {
                    // Вычисляем необходимую корреляцию для каждого подмножества относительно введенной достоверной уверенности
                    double lift = CalculateLift(needConfidence, encryptedData._subsets[j], encryptedData._transactions);
                    double needCorrelation = CalculateCorrelation(lift, encryptedData._subsets[j].Count, encryptedData._transactions.Count);

                    // Создаем все возможные посылки для j-ого подмножества
                    largerItemsets.AddRange(GenerateLargerItemsets(excelFile1, 1, encryptedData._transactions, needCorrelation, target, encryptedData));

                    foreach (var largeItemset in largerItemsets)
                    {
                        RuleClass rule = new RuleClass(new HashSet<int>(largeItemset), j, encryptedData);
                        // Проверяем правило на соответствие заданным характеристикам
                        if (rule.confidence >= needConfidence & rule.quality >= needQuality)
                        {
                            rules.Add(rule);
                        }
                    }
                    largerItemsets.Clear();
                }
            }
            else
            {
                // Вычисляем необходимую корреляцию для выбранного подмножества относительно введенной достоверной уверенности
                double lift = CalculateLift(needConfidence, encryptedData._subsets[target], encryptedData._transactions);
                double needCorrelation = CalculateCorrelation(lift, encryptedData._subsets[target].Count, encryptedData._transactions.Count);

                // Создаем все возможные посылки для выбранного подмножества
                largerItemsets.AddRange(GenerateLargerItemsets(excelFile1, 1, encryptedData._transactions, needCorrelation, target, encryptedData));

                foreach (var largeItemset in largerItemsets)
                {
                    RuleClass rule = new RuleClass(new HashSet<int>(largeItemset), target, encryptedData);
                    // Проверяем правило на соответствие заданным характеристикам
                    if (rule.confidence >= needConfidence & rule.quality >= needQuality)
                    {
                        rules.Add(rule);
                    }
                }
                largerItemsets.Clear();
            }
            return rules;
        }

        /// <summary>
        /// Генерация всех правил согласно заданным параметрам
        /// </summary>
        /// <param name="target">Номер подмножества</param>
        /// <param name="needConfidence">Достаточная уверенность</param>
        /// <param name="needQuality">Достаточное качество</param>
        /// <param name="sendingLength">Достаточная длина посылки</param>
        /// <param name="encryptedData">Объект, хранящий информацию о зашифрованном файле</param>
        public static List<RuleClass> GenerateAllRules(int target, double needConfidence, double needQuality, int sendingLength, DataEncryptor encryptedData)
        {
            List<RuleClass> rules = new List<RuleClass>();

            ExcelFile excelFile1 = encryptedData._metaFile;

            HashSet<int> itemset = new HashSet<int>();

            List<HashSet<int>> largerItemsets = new List<HashSet<int>>();


            // В случае, когда target = -1 работаем со всеми подмножествами
            if (target == -1)
            {
                for (int j = 0; j < excelFile1.SubsetsCount; ++j)
                {
                    for (int i = excelFile1.SubsetsCount; i < excelFile1.PropertyNames.Count; i++)
                    {
                        itemset.Add(i);
                        RuleClass rule = new RuleClass(new HashSet<int>(itemset), j, encryptedData);
                        if (rule.confidence >= needConfidence & rule.quality >= needQuality & itemset.Count > 0)
                        {
                            rules.Add(rule);
                        }
                        itemset.Remove(i);
                    }
                }
                for (int length = 2; length <= sendingLength; length++)
                {

                    for (int j = 0; j < excelFile1.SubsetsCount; j++)
                    {
                        // Вычисляем необходимую корреляцию для каждого подмножества относительно введенной достоверной уверенности
                        double lift = CalculateLift(needConfidence, encryptedData._subsets[j], encryptedData._transactions);
                        double needCorrelation = CalculateCorrelation(lift, encryptedData._subsets[j].Count, encryptedData._transactions.Count);

                        largerItemsets.AddRange(GenerateLargerItemsets(excelFile1, length, encryptedData._transactions, needCorrelation, target, encryptedData));
                        foreach (HashSet<int> largerItemset in largerItemsets)
                        {
                            RuleClass rule = new RuleClass(new HashSet<int>(largerItemset), j, encryptedData);
                            if (rule.confidence >= needConfidence & rule.quality >= needQuality)
                            {
                                rules.Add(rule);
                            }
                        }
                    }
                    largerItemsets.Clear();
                }
            }
            else
            {
                // Вычисляем необходимую корреляцию для определенного подмножества относительно введенной достоверной уверенности
                double lift = CalculateLift(needConfidence, encryptedData._subsets[target], encryptedData._transactions);
                double needCorrelation = CalculateCorrelation(lift, encryptedData._subsets[target].Count, encryptedData._transactions.Count);

                for (int i = excelFile1.SubsetsCount; i < excelFile1.PropertyNames.Count; i++)
                {
                    RuleClass rule = new RuleClass(new HashSet<int>(itemset), target, encryptedData);
                    if (rule.confidence >= needConfidence & rule.quality >= needQuality & rule.itemset.Count <= sendingLength & itemset.Count > 0)
                    {
                        rules.Add(rule);
                    }
                }
                for (int i = 1; i <= sendingLength; i++)
                {
                    largerItemsets.AddRange(GenerateLargerItemsets(excelFile1, i, encryptedData._transactions, needCorrelation, target, encryptedData));
                }
                foreach (HashSet<int> largerItemset in largerItemsets)
                {
                    RuleClass rule = new RuleClass(new HashSet<int>(largerItemset), target, encryptedData);
                    if (rule.confidence >= needConfidence & rule.quality >= needQuality)
                    {
                        rules.Add(rule);
                    }
                }
            }
            return rules;
        }

        /// <summary>
        /// Функция создающая все возможные наборы бинарных свойств данной длины
        /// </summary>
        /// <param name="excelFile1">Данный файл</param>
        /// <param name="length">Данная длина</param>
        /// <param name="transactions">Список щашифрованных записей</param>
        /// <param name="correlationThreshold">Минимально возможная корреляция посылки</param>
        /// <param name="target">Номер подмножества</param>
        /// <param name="de">Объект типа DataEncryptor, содержащий зашифрованные данные</param>
        /// <returns>Все возможные наборы бинарных свойств данной длины</returns>
        public static List<HashSet<int>> GenerateLargerItemsets(ExcelFile excelFile1, int length, List<BigInteger> transactions, double correlationThreshold, int target, DataEncryptor de)
        {
            List<HashSet<int>> largerItemsets = new List<HashSet<int>>();
            HashSet<int> itemset = new HashSet<int>();
            GenerateLargerItemset(excelFile1.SubsetsCount, excelFile1.PropertyNames.Count, length, itemset, largerItemsets, excelFile1, correlationThreshold, transactions, target, de);
            return largerItemsets;
        }

        /// <summary>
        /// Генерирует все возможные наборы бинарных свойств от данного номера бинарного свойства и до последнего
        /// </summary>
        /// <param name="initialNumberProperties"> Номер начального свойства</param>
        /// <param name="propertiesCount"> Количество бинарных свойств</param>
        /// <param name="length"> Длина набора</param>
        /// <param name="itemset"> Набор</param>
        /// <param name="largerItemsets"> Список наборов</param>
        /// <param name="excelFile1">Файл с метаданными</param>
        /// <param name="correlationThreshold">Минимальная корреляция</param>
        /// <param name="transactions">Зашифрованные записи</param>
        /// <param name="target">Номер подмножества</param>
        /// <param name="de">Объект типа DataEncryptor, содержащий зашифрованные данные</param>
        public static void GenerateLargerItemset(int initialNumberProperties, int propertiesCount, int length, HashSet<int> itemset, List<HashSet<int>> largerItemsets, ExcelFile excelFile1, double correlationThreshold, List<BigInteger> transactions, int target, DataEncryptor de)
        {
            if (length == 0)
            {
                // Проходим по всем подмножествам
                if (target == -1)
                {
                    for (int i = 0; i < excelFile1.SubsetsCount; ++i)
                    {
                        // Высчитываем корреляцию данной посылки в каждом подмножестве
                        double confidence = CalculateConfidence(itemset, de._subsets[i], transactions, excelFile1.PropertyNames.Count);
                        double lift = CalculateLift(confidence, de._subsets[i], transactions);
                        double currentCorrelation = CalculateCorrelation(lift, de._subsets[i].Count, transactions.Count);

                        // Добавляем набор только если он проходит по корреляции
                        if (currentCorrelation >= correlationThreshold)
                        {
                            largerItemsets.Add(new HashSet<int>(itemset));
                            break;
                        }
                    }
                }
                // Проходим по выбранному подмножеству
                else
                {
                    // Высчитываем корреляцию данной посылки в выбранном подмножестве
                    double confidence = CalculateConfidence(itemset, de._subsets[target], transactions, excelFile1.PropertyNames.Count);
                    double lift = CalculateLift(confidence, de._subsets[target], transactions);
                    double currentCorrelation = CalculateCorrelation(lift, de._subsets[target].Count, transactions.Count);

                    // Добавляем набор только если он проходит по корреляции
                    if (currentCorrelation >= correlationThreshold)
                    {
                        largerItemsets.Add(new HashSet<int>(itemset));
                    }
                }
                return;
            }

            for (int i = initialNumberProperties; i < propertiesCount; i++)
            {
                if (itemset.Count == 0 || i > itemset.Last())
                {
                    itemset.Add(i);
                    GenerateLargerItemset(i + 1, propertiesCount, length - 1, itemset, largerItemsets, excelFile1, correlationThreshold, transactions, target, de);
                    itemset.Remove(itemset.Last());
                }
            }
        }


        /// <summary>
        /// Переводит зашифрованную запись в двоичный вид
        /// </summary>
        /// <param name="bigInteger">Зашифрованная запись</param>
        /// <param name="propertyNamesCount">Количество бинарных свойств</param>
        /// <returns>Зашифрованную запись в двочичном виде</returns>
        public static string ToBinaryString(BigInteger bigInteger, int propertyNamesCount)
        {
            return Convert.ToString((long)bigInteger, 2).PadLeft(propertyNamesCount, '0'); // Дополняем нулями слева
        }
    }
}