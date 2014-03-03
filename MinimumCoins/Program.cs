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
            // int[] denoms = { 7, 4, 1 };
            int[] denoms = { 14, 9, 2};

            // int amount = 8;
            int amount = 18;


            Console.WriteLine("Using DP recursive: Minimum number of coins: {0}", FindMinimumCoins1(amount, denoms));

            Console.WriteLine("Using greedy approach: Minimum number of coins: {0}", FindMinimumCoins2(amount, denoms));

            Console.WriteLine("Using DP iterative: Minimum number of coins: {0}", FindMinimumCoins3(amount, denoms));
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
    }
}
