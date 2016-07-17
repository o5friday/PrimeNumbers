using System;
using System.Collections;
using System.Collections.Generic;

namespace PrimeNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter number of primes to generate");
            string input = Console.ReadLine();

            int numberOfPrimes;
            if (Int32.TryParse(input, out numberOfPrimes))
            {
                if (numberOfPrimes < 1)
                {
                    Console.WriteLine("Incorrect input");
                    return;
                }

                var primes = SieveOfEratosthenes.GeneratePrimesList(numberOfPrimes);
                Console.WriteLine($"List of {numberOfPrimes} elements: {string.Join(", ", primes)}");

                double result = 0;
                for (int i = 0; i < primes.Count; i++)
                {
                    result += (double)primes[i] / (i + 1);
                }

                Console.WriteLine($"Result: {Math.Floor(result)}");
            }
            else
            {
                Console.WriteLine("Incorrect input");
            }
        }
    }

    class SieveOfEratosthenes
    {
        public static List<int> GeneratePrimesList(int numberOfPrimesToReturn)
        {
            var primes = new List<int>();

            var limit = FindApproximateNthPrime(numberOfPrimesToReturn);
            if (limit == 0)
            {
                return primes;
            }
            var bits = BuildSieve(limit);

            for (int i = 0, found = 0; i <= limit && found < numberOfPrimesToReturn; i++)
            {
                if (!bits[i])
                {
                    continue;
                }

                primes.Add(i);
                found++;
            }

            return primes;
        }

        public static int FindApproximateNthPrime(int n)
        {
            double p;
            if (n >= 6) // Rosser's theorem (https://en.wikipedia.org/wiki/Prime_number_theorem)
            {
                p = n * (Math.Log(n) + Math.Log(Math.Log(n)));
            }
            else if (n > 0)
            {
                p = new[] { 2, 3, 5, 7, 11 }[n - 1];
            }
            else
            {
                p = 0;
            }

            return (int)p;
        }

        private static BitArray BuildSieve(int limit)
        {
            // build the sieve for numbers starting from '0' up to 'limit'
            var bits = new BitArray(limit + 1, true);
            bits[0] = false;
            bits[1] = false;
            for (var i = 0; i * i <= limit; i++)
            {
                if (!bits[i])
                {
                    continue;
                }

                for (var j = i * i; j <= limit; j += i)
                {
                    bits[j] = false;
                }
            }

            return bits;
        }
    }
}
