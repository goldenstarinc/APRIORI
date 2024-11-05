using System;
using System.Collections.Generic;
using System.Numerics;
using AA = DataProcessor.AprioriAlgorithm;

namespace DataProcessor
{
    public class RuleClass
    {
        public double f_g { get; private set; }
        public double f_all { get; private set; }
        public double confidence { get; private set; }
        public double lift { get; private set; }
        public double correlation { get; private set; }
        public double quality { get; private set; }
        public int target { get; private set; }
        public HashSet<int> itemset { get; private set; }
        public List<string> propertyNames { get; private set; }


        public RuleClass(HashSet<int> Itemset, int subsetNumber, DataEncryptor encryptedData)
        {
            this.itemset = Itemset;
            this.target = subsetNumber;
            this.propertyNames = encryptedData._propertyNames;

            List<List<string>> _subsets = encryptedData._subsets;
            List<string> transactions = encryptedData._transactions;

            int support = AA.CountSupport(itemset, _subsets[target], encryptedData._propertiesCount);

            this.f_g = AA.GetFrequency(support, _subsets[target].Count);
            this.f_all = AA.GetFrequency(support, transactions.Count);
            this.confidence = AA.CalculateConfidence(itemset, _subsets[target], transactions, encryptedData._propertiesCount);
            this.lift = AA.CalculateLift(confidence, _subsets[target].Count, transactions.Count);
            this.correlation = AA.CalculateCorrelation(lift, _subsets[target].Count, transactions.Count);
            this.quality = AA.CalculateQuality(f_g, f_all, confidence, correlation);    
        }

        /// <summary>
        ///  Переопределение метода ToString
        /// </summary>
        public override string ToString()
        {
            string result = "";

            int count = 0;
            foreach (var set in itemset)
            {
                result += propertyNames[set];
                count++;

                if (count != itemset.Count)
                    result += ", ";
                else
                    result += " ";
            }

            result += $"=> Goal{target}:\n" +
                      $"F_g: {Math.Round(f_g, 3)}; " +
                      $"F_all: {Math.Round(f_all, 3)}; " +
                      $"Confidence: {Math.Round(confidence, 3)}; " +
                      $"Correlation: {Math.Round(correlation, 3)}; " +
                      $"Quality: {Math.Round(quality, 3)}\n";

            return result;
        }
    }
}
