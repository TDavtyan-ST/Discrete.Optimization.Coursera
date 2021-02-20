using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Discrete.Optimization.Knapsack
{
    public class ItemWithMetadata
    {
        public int Index { get; set; }
        public int Value { get; set; }
        public int Weight { get; set; }
        public double ValuePerWeight { get; set; }

        public ItemWithMetadata(int index, int value, int weight, double valuePerWeight)
        {
            Index = index;
            Value = value;
            Weight = weight;
            ValuePerWeight = valuePerWeight;
        }
    }

    class Program
    {
        static int n;
        static int capacity;
        private static int[] values;
        private static int[] weights;
        private static int[] taken;
        static List<ItemWithMetadata> items = new();
        private static int[,] dynamicTable;
        private static int optimized = 0;

        static void Main(string[] args)
        {
            if (TakeInput(args)) return;

            int optimalValue;
            if (n < 100)
            {
                optimalValue = oracle(capacity, n);
            }
            else
            {
                optimalValue = CalculateUsingGreedy(capacity, n);
                optimized = 1;
            }


            Console.WriteLine($"{optimalValue} {optimized}");
            Console.WriteLine(String.Join(' ', taken));
        }

        private static int CalculateUsingGreedy(int cap, int i)
        {
            items = items.OrderByDescending(item => item.ValuePerWeight).ToList();

            int weight = 0, value = 0;

            foreach (var item in items)
            {
                if (weight + item.Weight < cap)
                {
                    weight += item.Weight;
                    value += item.Value;
                    taken[item.Index] = 1;
                }
            }

            return value;
        }


        private static bool TakeInput(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("no file specified");
                return true;
            }

            string filePath = args[0];
            // Console.WriteLine(filePath);
            // Console.WriteLine(File.ReadAllText(filePath));
            string[] fileContent = File.ReadAllText(filePath).Split("\n");
            string[] firstLine = fileContent[0].Split(' ');

            // Console.WriteLine(firstLine[0]);
            n = Convert.ToInt32(firstLine[0]);
            capacity = Convert.ToInt32(firstLine[1]);

            values = new int[n];
            weights = new int[n];
            taken = new int[n];

            for (int i = 1; i <= n; i++)
            {
                string[] currentLine = fileContent[i].Split(' ');

                int value = Convert.ToInt32(currentLine[0]);
                int weight = Convert.ToInt32(currentLine[1]);

                values[i - 1] = value;
                weights[i - 1] = weight;
                items.Add(new ItemWithMetadata(i-1, value, weight, (double) value / (double) weight));
            }

            return false;
        }

        private static void PrintDynamicTable()
        {
            for (int capacityIndex = 0; capacityIndex <= capacity; capacityIndex++)
            {
                for (int i = 0; i <= n; i++)

                {
                    Console.Write($"{dynamicTable[capacityIndex, i]}, ");
                }

                Console.WriteLine();
            }
        }

        // Returns the maximum value that can
        // be put in includedValue knapsack of cap cap
        static int oracle(int cap, int j)
        {
            dynamicTable = new int[capacity + 1, n + 1];

            CalculateDynamicTableValues(cap, j);

            PrintDynamicTable();

            TraceBackDynamicTable();

            return dynamicTable[cap, j];
        }

        private static void TraceBackDynamicTable()
        {
            int capacityIndex = capacity;
            int i = n;

            while (i != 0 && n != 0)
            {
                if (dynamicTable[capacityIndex, i] == dynamicTable[capacityIndex, i - 1])
                {
                    taken[i - 1] = 0;
                }
                else
                {
                    taken[i - 1] = 1;
                    capacityIndex -= weights[i - 1];
                }

                i--;
            }
        }

        private static void CalculateDynamicTableValues(int cap, int size)
        {
            for (int i = 0; i <= size; i++)
            {
                for (int capacityIndex = 0; capacityIndex <= cap; capacityIndex++)
                {
                    if (capacityIndex == 0 || i == 0)
                    {
                        dynamicTable[capacityIndex, i] = 0;
                    }
                    else if (weights[i - 1] > capacityIndex)
                    {
                        dynamicTable[capacityIndex, i] = dynamicTable[capacityIndex, i - 1];
                    }
                    else
                    {
                        var includedValue = values[i - 1] + dynamicTable[capacityIndex - weights[i - 1], i - 1];
                        var excludedValue = dynamicTable[capacityIndex, i - 1];

                        dynamicTable[capacityIndex, i] = Math.Max(
                            excludedValue,
                            includedValue);
                    }
                }
            }
        }
    }
}