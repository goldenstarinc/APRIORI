using DataProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_Preprocessor_WPF
{
    public class SharedData
    {
        private static SharedData instance;

        public ExcelFile MetaFile { get; set; }
        public string FilePath { get; set; }
        public DataEncryptor EncryptedData { get; set; }

        private SharedData() { }

        public static SharedData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SharedData();
                }
                return instance;
            }
        }
    }
}
