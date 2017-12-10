using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;

namespace Day10_KnotHash
{
	static class Program
	{
		static void Main(string[] args)
		{
			var rawInput = "97,167,54,178,2,11,209,174,119,248,254,0,255,1,64,190";

			Debug.Assert(KnotHash1(new[] { 3, 4, 1, 5 }, listCount: 5, debugOutput: true) == 12);
			Console.WriteLine(KnotHash1(ParseInput(rawInput)));

			Debug.Assert(KnotHash2("") == "a2582a3a0e66e6e86e3812dcb672a272");
			Debug.Assert(KnotHash2("AoC 2017") == "33efeb34ea91902bb2f59c9920caa6cd");
			Debug.Assert(KnotHash2("1,2,3") == "3efbe78a8d82f29979031a4aa0b16a9d");
			Debug.Assert(KnotHash2("1,2,4") == "63960835bcdc130f0b66d7ff4f6a5a8e");
			Console.WriteLine(KnotHash2(rawInput));

			Console.WriteLine("Done");
			Console.ReadKey();
		}

		static List<int> ParseInput(string input)
			=> input.Split(',').Select(int.Parse).ToList();

		static int KnotHash1(IEnumerable<int> lengths, int listCount = 256, bool debugOutput = false)
		{
			var list = Enumerable.Range(0, listCount).ToList();
			var currentPosition = 0;
			var skipSize = 0;

			KnotHash(lengths, list, ref currentPosition, ref skipSize, debugOutput);
			return list[0] * list[1];
		}

		static string KnotHash2(string input, bool debugOutput = false)
		{
			var lengths = input
				.Select(c => (int)c)
				.Concat(new[] { 17, 31, 73, 47, 23 })
				.ToList();

			var sparseHash = Enumerable.Range(0, 256).ToList();
			var currentPosition = 0;
			var skipSize = 0;

			for (int i = 0; i < 64; i++)
			{
				KnotHash(lengths, sparseHash, ref currentPosition, ref skipSize, debugOutput);
			}

			var denseHash = new List<int>();

			const int chunkSize = 16;
			for (int i = 0; i < sparseHash.Count; i += chunkSize)
			{
				denseHash.Add(sparseHash
					.Slice(i, chunkSize)
					.Aggregate((a, b) => a ^ b));
			}

			return string.Join("", denseHash.Select(value => value.ToString("x2")));
		}

		static void KnotHash(IEnumerable<int> lengths, List<int> list, ref int currentPosition, ref int skipSize, bool debugOutput)
		{
			foreach (var length in lengths)
			{
				list.ReverseRangeCircular(currentPosition, length);
				currentPosition += length + skipSize;
				skipSize++;

				if (debugOutput)
				{
					Console.WriteLine($"> [ { string.Join(", ", list) } ], currentPosition={currentPosition}, skipSize={skipSize}");
				}
			}
		}

		static void ReverseRangeCircular<T>(this List<T> source, int start, int count)
		{
			for (int i = 0; i < count / 2; i++)
			{
				int a = start + i;
				int b = start + count - 1 - i;
				source.Swap(a % source.Count, b % source.Count);
			}
		}

		static void Swap<T>(this List<T> source, int index1, int index2)
		{
			var temp = source[index1];
			source[index1] = source[index2];
			source[index2] = temp;
		}
	}
}
