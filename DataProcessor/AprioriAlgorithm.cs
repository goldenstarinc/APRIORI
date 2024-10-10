using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor
{
    public class AprioriAlgorithm
    {
        private List<BigInteger> _transactions;  // Транзакции в виде двоичных чисел
        private int _minSupport;                 // Минимальная поддержка
        private List<string> _propertyNames;     // Имена свойств для элементов

        public AprioriAlgorithm(List<BigInteger> transactions, int minSupport, List<string> propertyNames)
        {
            _transactions = transactions;
            _minSupport = minSupport;
            _propertyNames = propertyNames;
        }

        // Основная функция для поиска частых наборов
        public List<HashSet<int>> FindFrequentItemsets(double support = 0)
        {
            support = (support / 100) * _transactions.Count;
            List<HashSet<int>> frequentItemsets = new List<HashSet<int>>();

            // Генерируем и фильтруем одноэлементные наборы
            var singleItemsets = GenerateSingleItemsets();
            frequentItemsets.AddRange(FilterFrequentItemsets(singleItemsets, support));

            // Генерируем более крупные наборы
            var largerItemsets = GenerateLargerItemsets(frequentItemsets, support);
            frequentItemsets.AddRange(largerItemsets);

            return frequentItemsets;
        }

        private List<HashSet<int>> GenerateSingleItemsets()
        {
            List<HashSet<int>> singleItemsets = new List<HashSet<int>>();
            for (int i = 0; i < _propertyNames.Count; i++)
            {
                singleItemsets.Add(new HashSet<int> { i });
            }
            return singleItemsets;
        }

        private List<HashSet<int>> FilterFrequentItemsets(List<HashSet<int>> itemsets, double support)
        {
            var frequentItemsets = new List<HashSet<int>>();
            foreach (var itemset in itemsets)
            {
                int count = CountSupport(itemset);
                if (count >= support)
                {
                    frequentItemsets.Add(itemset);
                }
            }
            return frequentItemsets;
        }

        private List<HashSet<int>> GenerateLargerItemsets(List<HashSet<int>> previousItemsets, double support)
        {
            List<HashSet<int>> largerItemsets = new List<HashSet<int>>();
            for (int i = 0; i < previousItemsets.Count; i++)
            {
                for (int j = i + 1; j < previousItemsets.Count; j++)
                {
                    var combined = new HashSet<int>(previousItemsets[i]);
                    combined.UnionWith(previousItemsets[j]);

                    if (combined.Count == previousItemsets[i].Count + 1) // Добавляем на один элемент больше
                    {
                        if (CountSupport(combined) >= support)
                        {
                            largerItemsets.Add(combined);
                        }
                    }
                }
            }

            if (largerItemsets.Count > 0)
            {
                largerItemsets.AddRange(GenerateLargerItemsets(largerItemsets, support));
            }

            return largerItemsets;
        }

        // Подсчет поддержки (support) набора элементов
        private int CountSupport(HashSet<int> itemset)
        {
            int count = 0;
            foreach (var transaction in _transactions)
            {
                string binaryTransaction = ToBinaryString(transaction);
                bool containsAll = true;
                foreach (var item in itemset)
                {
                    if (item >= binaryTransaction.Length || binaryTransaction[binaryTransaction.Length - 1 - item] != '1')
                    {
                        containsAll = false;
                        break;
                    }
                }
                if (containsAll)
                {
                    count++;
                }
            }
            return count;
        }

        // Преобразование BigInteger в двоичную строку
        private string ToBinaryString(BigInteger bigInteger)
        {
            return Convert.ToString((long)bigInteger, 2).PadLeft(_propertyNames.Count, '0'); // Дополняем нулями слева
        }

        // Функция для генерации ассоциативных правил
        public List<Tuple<HashSet<int>, HashSet<int>, double, double>> GenerateAssociationRules(List<HashSet<int>> frequentItemsets)
        {
            var rules = new List<Tuple<HashSet<int>, HashSet<int>, double, double>>();
            foreach (var itemset in frequentItemsets)
            {
                if (itemset.Count > 1)
                {
                    // Создаем все возможные подмножества для генерации правил
                    var subsets = GetSubsets(itemset);

                    foreach (var subset in subsets)
                    {
                        if (subset.Count > 0 && subset.Count < itemset.Count)
                        {
                            var remaining = new HashSet<int>(itemset);
                            remaining.ExceptWith(subset);

                            // Вычисляем конфиденс и лифт
                            double confidence = CalculateConfidence(subset, remaining);
                            double lift = CalculateLift(subset, remaining);

                            rules.Add(new Tuple<HashSet<int>, HashSet<int>, double, double>(subset, remaining, confidence, lift));
                        }
                    }
                }
            }

            return rules;
        }

        // Получение всех подмножеств для генерации ассоциативных правил
        private List<HashSet<int>> GetSubsets(HashSet<int> itemset)
        {
            List<HashSet<int>> subsets = new List<HashSet<int>>();
            int[] array = itemset.ToArray();
            int subsetCount = (int)Math.Pow(2, array.Length);

            for (int i = 1; i < subsetCount - 1; i++) // Пропускаем пустое и полное подмножества
            {
                HashSet<int> subset = new HashSet<int>();
                for (int j = 0; j < array.Length; j++)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        subset.Add(array[j]);
                    }
                }
                subsets.Add(subset);
            }
            return subsets;
        }

        // Вычисление конфиденса
        private double CalculateConfidence(HashSet<int> antecedent, HashSet<int> consequent)
        {
            int antecedentSupport = CountSupport(antecedent);
            int combinedSupport = CountSupport(new HashSet<int>(antecedent.Union(consequent)));
            return (double)combinedSupport / antecedentSupport;
        }

        // Вычисление лифта
        private double CalculateLift(HashSet<int> antecedent, HashSet<int> consequent)
        {
            int consequentSupport = CountSupport(consequent);
            double confidence = CalculateConfidence(antecedent, consequent);
            return confidence / ((double)consequentSupport / _transactions.Count);
        }
    }
}