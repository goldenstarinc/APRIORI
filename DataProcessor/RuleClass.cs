using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AA = DataProcessor.AprioriAlgorithm;

namespace DataProcessor
{
    public class RuleClass
    {
        public double f_g { get; private set; }
        public double f_all { get; private set; }
        public double confidence { get; private set; }
        public double lift { get; private set; }
        public double quality { get; private set; }
        public int target { get; private set; }
        public HashSet<int> itemset { get; private set; }
        public List<string> propertyNames { get; private set; }
        public List<RuleClass> rules { get; private set; }

        List<List<BigInteger>> subsets = DB_Data._subsets;
        int propertiesCount = DB_Data._propertyNames.Count;
        List<BigInteger> transactions = DB_Data._transactions;


        public RuleClass(HashSet<int> Itemset, int subsetNumber)
        {
            this.itemset = Itemset;
            this.target = subsetNumber;
            this.propertyNames = DB_Data._propertyNames;

            int support = AA.CountSupport(itemset, subsets[target], propertiesCount);

            this.f_g = AA.GetFrequency(support, subsets[target].Count);
            this.f_all = AA.GetFrequency(support, transactions.Count);
            this.confidence = AA.CalculateConfidence(itemset, subsets[target], transactions, propertiesCount);
            this.lift = AA.CalculateLift(confidence, subsets[target], transactions);
            this.quality = AA.CalculateQuality(f_g, f_all, confidence, lift);
        }

        public override string ToString()
        {
            string result = "==================================================\n";

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
                      $"Lift: {Math.Round(lift, 3)}; " +
                      $"Quality: {Math.Round(quality, 3)}";

            return result;
        }
    }
}
