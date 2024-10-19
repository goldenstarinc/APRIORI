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
        /// <summary>
        /// Считает количество вхождений определенного набора бинарных свойств в подмножестве
        /// </summary>
        /// <param name="itemset"> Определенный набор бинарных свойств</param>
        /// <param name="subset"> Подмножество</param>
        /// <returns> Количество вхождений определенного набора бинарных свойств в подмножестве</returns>
        public static int CountSupport(HashSet<int> itemset, List<BigInteger> subset, int propertiesCount)
        {
            int count = 0;

            foreach(var set in subset)
            {
                bool isAdequate = true;
                string binarySet = DB_Data.ToBinaryString(set, propertiesCount);
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
        /// Вычисляет корреляцию между посылкой правила и его значением
        /// </summary>
        /// <param name="confidence"> Достоверность правила</param>
        /// <param name="subset"> Подмножество</param>
        /// <returns> Корреляция между посылкой правила и его значением</returns>
        public static double CalculateLift(double confidence, List<BigInteger> subset, List<BigInteger> transactions)
        {
            return confidence * (transactions.Count / subset.Count);
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