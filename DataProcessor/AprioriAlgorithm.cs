using Aspose.Cells.Pivot;
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
        public static List<BigInteger> _transactions { get; set; }  // Транзакции в виде двоичных чисел
        public static List<string> _propertyNames { get; set; }     // Имена свойств для элементов
        public static List<List<BigInteger>> _subsets { get; set; } // Подмножества


        /// <summary>
        /// Формирует подмножества на основе принадлежности записи к определенному бинарному свойству целевого столбца
        /// </summary>
        /// <param name="index"> Индекс бинарного свойства целевого столбца</param>
        /// <returns> Подмножество</returns>
        public static List<BigInteger> FormSubsets(int index, List<BigInteger> _tempTransactions)
        {
            // Хранение записи для добавления подмножества
            List<BigInteger> temp = new List<BigInteger>();

            // Проходимся по каждой записи
            foreach (var transaction in _tempTransactions)
            {
                string item = AprioriAlgorithm.ToBinaryString(transaction);
                if (item[item.Length - 1 - index] == '1')
                {
                    temp.Add(transaction);
                }
            }

            // Удаляем записанные записи из основного списка
            foreach (var delete in temp)
            {
                _tempTransactions.Remove(delete);
            }

            return temp;
        }

        /// <summary>
        /// Считает количество вхождений определенного набора бинарных свойств в подмножестве
        /// </summary>
        /// <param name="itemset"> Определенный набор бинарных свойств</param>
        /// <param name="subset"> Подмножество</param>
        /// <returns> Количество вхождений определенного набора бинарных свойств в подмножестве</returns>
        public static int CountSupport(HashSet<int> itemset, List<BigInteger> subset)
        {
            int count = 0;

            foreach(var set in subset)
            {
                bool isAdequate = true;
                string binarySet = ToBinaryString(set);
                foreach(int item in itemset)
                {

                    if (binarySet[binarySet.Length - 1 - item] != '1')
                    {
                        isAdequate = false;
                        break;
                    }
                }

                if(isAdequate) count++;
            }

            return count;
        }

        /// <summary>
        /// Переводит число в двоичный вид
        /// </summary>
        /// <param name="bigInteger"> Число</param>
        /// <returns> Двоичный вид числа</returns>
        public static string ToBinaryString(BigInteger bigInteger)
        {
            return Convert.ToString((long)bigInteger, 2).PadLeft(_propertyNames.Count, '0'); // Дополняем нулями слева
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
        public static double CalculateConfidence(HashSet<int> itemset, List<BigInteger> subset)
        {
            int subsetSupport = CountSupport(itemset, subset);
            int setSupport = CountSupport(itemset, _transactions);
            return (double)subsetSupport / setSupport;
        }

        /// <summary>
        /// Вычисляет корреляцию между посылкой правила и его значением
        /// </summary>
        /// <param name="confidence"> Достоверность правила</param>
        /// <param name="subset"> Подмножество</param>
        /// <returns> Корреляция между посылкой правила и его значением</returns>
        public static double CalculateLift(double confidence, List<BigInteger> subset)
        {
            return confidence * (_transactions.Count / subset.Count);
        }

        /// <summary>
        /// Вычисляет качество правило
        /// </summary>
        /// <param name="f_g"> Вычисляет частоту вхождения определенного набора бинарных свойств в подмножестве</param>
        /// <param name="f_all"> Вычисляет частоту вхождения определенного набора бинарных свойств в множестве</param>
        /// <param name="confidence"> Достоверность правила</param>
        /// <param name="lift"> Корреляция между посылкой правила и его значением</param>
        /// <param name="p1"> Вес 1</param>
        /// <param name="p2"> Вес 2</param>
        /// <param name="p3"> Вес 3</param>
        /// <param name="p4"> Вес 4</param>
        /// <returns> Качество правила</returns>
        public static double CalculateQuality(double f_g , double f_all , double confidence , double lift, double p1 = 1, double p2 = 1, double p3 = 1, double p4 = 1)
        {
            return f_all * p1 + f_g * p2 + confidence * p3 + lift * p4;
        }
    }
}