using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Given a set of denominations and an amount
// find the minimum number of coins needed to 
// generate that amount
namespace MinimumCoins
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] denoms = { 14, 9, 2};
            int amount = 18;


            Console.WriteLine("Using DP recursive: Minimum number of coins: {0}", FindMinimumCoins1(amount, denoms));

            Console.WriteLine("Using greedy approach: Minimum number of coins: {0}", FindMinimumCoins2(amount, denoms));

            Console.WriteLine("Using DP iterative: Minimum number of coins: {0}", FindMinimumCoins3(amount, denoms));

            var result = FindMinimumCoins4(amount, denoms);
            Console.WriteLine("Using DP recursive: Minimum coins to make amount {0}", amount);

            foreach (int value in result)
            {
                Console.Write(value + " ");
            }

            Console.WriteLine();

            TestAllCombinationsForAmount(18, new int[]{14, 9, 2});
            TestAllCombinationsForAmount(6, new int[] {25, 10, 5, 1});

        }

        static void TestAllCombinationsForAmount(int amount, int[] denoms)
        {
            Console.WriteLine("All possible ways to generate {0} is", amount);
            
            var outputList = ChangeCombinations(amount, denoms);

            foreach (var list in outputList)
            {
                foreach (int value in list)
                {
                    Console.Write(value + " ");
                }

                Console.WriteLine();
            }
        }

        static Dictionary<int, int> cache = new Dictionary<int, int>();

        // Approach 1: Using brute force then adding caching a.k.a DP
        // assumes denominations is already sorted in descending order
        // First write the brute force version
        // Now add caching. The way to think about caching is the key
        // should be the amount and the value should be the minimum 
        // number of coins to make that amount
        static int FindMinimumCoins1(int amount, int[] denominations)
        {
            if (amount == 0)
            {
                return 0;
            }

            if (cache.ContainsKey(amount))
            {
                return cache[amount];
            }

            List<int> result = new List<int>();

            int newAmount = 0;

            for (int i = 0; i < denominations.Length; i++)
            {
                newAmount = amount - denominations[i];

                if (newAmount >= 0)
                {
                    result.Add(1 + FindMinimumCoins1(newAmount, denominations));
                }
            }

            int minimumCoins = Minimum(result);

            cache.Add(amount, minimumCoins);

            return minimumCoins;
        }

        static int Minimum(List<int> input)
        {
            if (input.Count == 0)
            {
                return 0;
            }

            int minimum = input[0];

            for (int i = 1; i < input.Count; i++)
            {
                if (input[i] < minimum)
                {
                    minimum = input[i];
                }
            }

            return minimum;
        }

        // Greedy approach
        // The greedy approach works by 
        static int FindMinimumCoins2(int amount, int[] denoms)
        {
            if (amount == 0)
            {
                return 0;
            }

            int result = 0;

            int newAmount = amount;

            for (int i = 0; i < denoms.Length; i++)
            {
                if ((newAmount - denoms[i]) >= 0)
                {
                    newAmount = newAmount - denoms[i];
                    result++;
                }
            }

            if (newAmount > 0)
            {
                return result + FindMinimumCoins2(newAmount, denoms);
            }

            return result;
        }

        // DP using iterative
        static int FindMinimumCoins3(int amount, int[] denoms)
        {
            int[] entries = new int[amount + 1];

            // Fill in the array by figuring out how 
            // to make arr[i] using the given denominations

            entries[0] = 0;

            for (int i = 1; i < entries.Length; i++)
            {
                List<int> result = new List<int>();

                for (int j = 0; j < denoms.Length; j++)
                {
                    int newAmount = (i - denoms[j]);
                    if (newAmount >= 0)
                    {
                        int numOfCoins = 1 + entries[newAmount];
                        result.Add(numOfCoins);
                    }
                }

                entries[i] = Minimum(result);
            }

            return entries[entries.Length - 1];
        }

        static Dictionary<int, List<int>> cache2 = new Dictionary<int, List<int>>();

        // assume denoms is sorted in descending order
        // Returns the minimum set of coins to make an amount
        static List<int> FindMinimumCoins4(int amount, int[] denoms)
        {
            if (amount == 0)
            {
                return new List<int>();
            }

            if (cache2.ContainsKey(amount))
            {
                return cache2[amount];
            }

            List<List<int>> allLists = new List<List<int>>();

            for (int i = 0; i < denoms.Length; i++)
            {
                if (amount - denoms[i] >= 0)
                {
                    List<int> subset = FindMinimumCoins4(amount - denoms[i], denoms);

                    if (subset != null)
                    {
                        subset.Add(denoms[i]);
                        allLists.Add(subset);
                    }
                }
            }

            List<int> smallestList = Minimum(allLists);

            cache2.Add(amount, smallestList);

            return smallestList;
        }

        static List<int> Minimum(List<List<int>> inputLists)
        {
            List<int> smallestList = null;

            foreach (var list in inputLists)
            {
                if (smallestList == null || list.Count < smallestList.Count)
                {
                    smallestList = list;
                }
            }

            return smallestList;
        }

        static List<List<int>> ChangeCombinations(int amount, int[] denoms)
        {
            List<List<int>> result = new List<List<int>>();

            if (amount == 0)
            {
                result.Add(new List<int>());
            }
            else
            {

                for (int i = 0; i < denoms.Length; i++)
                {
                    int newAmount = amount - denoms[i];

                    if (newAmount >= 0)
                    {
                        int[] newDenoms = GetNewDenoms(denoms, i);

                        var subset = ChangeCombinations(newAmount, newDenoms);

                        foreach (var list in subset)
                        {
                            list.Add(denoms[i]);

                            result.Add(list);
                        }
                    }
                }
            }

            return result;
        }

        static int[] GetNewDenoms(int[] originalDenoms, int index)
        {
            int[] newDenoms = new int[originalDenoms.Length - index];

            for (int i = index, j = 0; i < originalDenoms.Length; i++, j++)
            {
                newDenoms[j] = originalDenoms[i];
            }

            return newDenoms;
        }
    }
}