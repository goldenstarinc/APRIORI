using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Interface_Preprocessor_WPF
{
    /// <summary>
    /// Interaction logic for DecryptedDataWindow.xaml
    /// </summary>
    public partial class DecryptedDataWindow : Window
    {
        public DecryptedDataWindow(List<List<string>> decryptedRecords)
        {
            InitializeComponent();
            DisplayDecryptedRecords(decryptedRecords);
        }

        private void DisplayDecryptedRecords(List<List<string>> decryptedRecords)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int recordCount = 1;

            foreach (var decryptedRecordList in decryptedRecords)
            {
                string recordHeader = $"Record_#{recordCount}:";
                stringBuilder.AppendLine(recordHeader);

                foreach (var record in decryptedRecordList)
                {
                    stringBuilder.Append($" {record}");
                }
                stringBuilder.AppendLine();
                recordCount++;
            }

            textBox.Text = stringBuilder.ToString();
        }
    }
}
