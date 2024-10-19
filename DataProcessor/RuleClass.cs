using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor
{
    public class RuleClass
    {
        public int f_g { get; private set; }
        public int f_all { get; private set; }
        public int confidence { get; private set; }
        public int lift { get; private set; }
        public int quality { get; private set; }

        private List<List<BigInteger>> _subsets = AprioriAlgorithm.GetSubsets();

        public RuleClass(HashSet<int> itemset, int subsetNumber)
        {
            _subsets = AprioriAlgorithm.GetSubsets();

            int support = AprioriAlgorithm.CountSupport(itemset, AprioriAlgorithm._subsets[2]);
            Console.WriteLine(GetFrequency(CountSupport(itemset, _subsets[2]), _subsets[2].Count));

            Console.WriteLine(GetFrequency(CountSupport(itemset, _subsets[2]), _transactions.Count));

            Console.WriteLine(CalculateConfidence(itemset, _subsets[2]));

            Console.WriteLine(CalculateLift(CalculateConfidence(itemset, _subsets[2]), _subsets[2]));
        }
        public RuleClass(double quality, double confidence, int subsetNumber)
        {

        }
        public RuleClass(double quality, double confidence, int length, int subsetNumber)
        {

        }
        public override string ToString()
        {

        }
    }
}
