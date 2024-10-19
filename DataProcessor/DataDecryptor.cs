using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;


namespace DataProcessor
{
    public class DataDecryptor
    {
        List<BigInteger> _encryptedRecords;

        List<string> _properyNames;

        public DataDecryptor(List<BigInteger> EncryptedRecords, List<string> PropertyNames) 
        {
            _encryptedRecords = EncryptedRecords;
            _properyNames = PropertyNames;
        }

        /// <summary>
        /// Метод для расшифровки зашифрованных записей
        /// </summary>
        /// <returns>
        /// Список списков строк, где каждый из которых содержит имена бинарных свойств, соответствующих одной расшифрованной записи</returns>
        public List<List<string>> GetDecryptedRecords()
        {
            List<List<string>> decryptedRecords = new List<List<string>>();

            foreach (var encryptedRecord in _encryptedRecords)
            {
                string binaryRepresentation = ToBinaryString(encryptedRecord);
                List<string> classNames = new List<string>();

                for (int i = binaryRepresentation.Length - 1; i >= 0; --i)
                {
                    if (binaryRepresentation[i] == '1')
                    {
                        classNames.Add(_properyNames[binaryRepresentation.Length - 1 - i]);
                    }
                }
                decryptedRecords.Add(classNames);
            }


            return decryptedRecords;
        }

        /// <summary>
        /// Метод для перевода числа типа <BigInteger> в двоичную систему счисления
        /// </summary>
        /// <param name="bigInteger">Число для перевода в двоичную запись</param>
        /// <returns>Двоичная запись переданного числа</returns>
        public static string ToBinaryString(BigInteger bigInteger)
        {
            return Convert.ToString((long)bigInteger, 2);
        }
        
    }
}
