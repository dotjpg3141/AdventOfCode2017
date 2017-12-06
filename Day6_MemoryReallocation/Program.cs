using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6_MemoryReallocation
{
	static class Program
	{
		static void Main(string[] args)
		{
			var result = CountCycles(new int[] { 0, 2, 7, 0 }, debugOutput: true);
			Debug.Assert(result.operationCount == 5);
			Debug.Assert(result.loopCount == 4);

			int[] input = { 4, 1, 15, 12, 0, 9, 9, 5, 5, 8, 7, 3, 14, 5, 12, 3 };
			Console.WriteLine(CountCycles(input));

			Console.WriteLine("Done");
			Console.ReadKey();
		}

		private static (int operationCount, int loopCount) CountCycles(int[] v, bool debugOutput = false)
		{
			var list = new List<int[]>();

			while (true)
			{
				if (debugOutput)
				{
					Console.WriteLine($" > [ {string.Join(", ", v)} ]");
				}

				v = SimmulateCycle(v);

				int index = list.IndexOf(item => item.SequenceEqual(v));
				list.Add(v);

				if (index != -1)
				{
					int operationCount = list.Count;
					int loopCount = list.Count - index - 1;
					return (operationCount, loopCount);
				}
			}
		}

		private static int[] SimmulateCycle(int[] v)
		{
			v = v.ToArray();

			// max index
			int maxValue = v.Max();
			int index = v.IndexOf(t => t == maxValue);

			// spread out value over the memory banks
			int value = v[index];
			v[index] = 0;
			while (value > 0)
			{
				index = (index + 1) % v.Length;

				v[index]++;
				value--;
			}

			return v;
		}

		public static int IndexOf<T>(this IEnumerable<T> source, Predicate<T> matches)
		{
			int index = 0;
			foreach (var item in source)
			{
				if (matches(item))
				{
					return index;
				}
				index++;
			}
			return -1;
		}
	}
}
