using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor
{
    public class DB_Data
    {
        public static List<BigInteger> _transactions { get; set; }  // Транзакции в виде двоичных чисел
        public static List<string> _propertyNames { get; set; }     // Имена свойств для элементов
        public static List<List<BigInteger>> _subsets { get; set; } // Подмножества

        public DB_Data(List<BigInteger> Transactions, List<string> PropertyNames, int subsetsCount) 
        {
            List<BigInteger> _tempTransactions = new List<BigInteger>();
            _tempTransactions.AddRange(Transactions);
            _subsets = new List<List<BigInteger>>();

            _transactions = Transactions;
            _propertyNames = PropertyNames;

            // Формируем подмножества на основе принадлежности записи к определенному бинарному свойству целевого столбца
            for (int i = 0; i < subsetsCount; i++)
            {
                _subsets.Add(FormSubsets(i, _tempTransactions));
            }
        }

        /// <summary>
        /// Формирует подмножества на основе принадлежности записи к определенному бинарному свойству целевого столбца
        /// </summary>
        /// <param name="index"> Индекс бинарного свойства целевого столбца</param>
        /// <returns> Подмножество</returns>
        public List<BigInteger> FormSubsets(int index, List<BigInteger> _tempTransactions)
        {
            // Хранение записи для добавления подмножества
            List<BigInteger> temp = new List<BigInteger>();

            // Проходимся по каждой записи
            foreach (var transaction in _tempTransactions)
            {
                string item = DataDecryptor.ToBinaryString(transaction);
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
        /// Переводит число в двоичный вид
        /// </summary>
        /// <param name="bigInteger"> Число</param>
        /// <returns> Двоичный вид числа</returns>
        public static string ToBinaryString(BigInteger bigInteger, int propertyNamesCount)
        {
            return Convert.ToString((long)bigInteger, 2).PadLeft(propertyNamesCount, '0'); // Дополняем нулями слева
        }
    }
}
