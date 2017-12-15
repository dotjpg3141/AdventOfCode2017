using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15_DuelingGenerators
{
	class Program
	{
		static void Main(string[] args)
		{

			var rawInput = "Generator A starts with 699\r\nGenerator B starts with 124\r\n";
			var rawExample = "65\r\n8921";

			Part1(rawExample, rawInput);
			Part2(rawExample, rawInput);

			Console.WriteLine("Done");
			Console.ReadKey();

		}

		private static void Part1(string rawExample, string rawInput)
		{
			var numberOfRounds1 = 40_000_000;

			var debugExample = ParseInput(rawExample);
			var input = ParseInput(rawInput);
			var example = ParseInput(rawExample);

			CountMachtes(debugExample.a, debugExample.b, 5, false, debugOutput: true);
			Debug.Assert(CountMachtes(example.a, example.b, numberOfRounds1, false) == 588);
			Console.WriteLine(CountMachtes(input.a, input.b, numberOfRounds1, false));
		}

		private static void Part2(string rawExample, string rawInput)
		{
			var numberOfRounds2 = 5_000_000;

			var input = ParseInput(rawInput);
			var debugExample = ParseInput(rawExample);
			var example = ParseInput(rawExample);

			CountMachtes(debugExample.a, debugExample.b, 5, true, debugOutput: true);
			Debug.Assert(CountMachtes(example.a, example.b, numberOfRounds2, true) == 309);
			Console.WriteLine(CountMachtes(input.a, input.b, numberOfRounds2, true));
		}

		static int CountMachtes(Generator a, Generator b, int numberOfRounds, bool requireMultiple, bool debugOutput = false)
		{
			int machtCount = 0;
			for (int i = 0; i < numberOfRounds; i++)
			{
				a.Next(requireMultiple);
				b.Next(requireMultiple);

				if (debugOutput)
				{
					Console.WriteLine();
					Console.WriteLine(a);
					Console.WriteLine(b);
					Console.WriteLine();
				}

				if (a.Low16 == b.Low16)
				{
					machtCount++;
				}
			}
			return machtCount;
		}

		static (Generator a, Generator b) ParseInput(string input)
		{
			var lines = input.Split('\n');
			var valueA = int.Parse(lines[0].Split(' ').Last());
			var valueB = int.Parse(lines[1].Split(' ').Last());
			var a = new Generator(valueA, 16807, 4);
			var b = new Generator(valueB, 48271, 8);
			return (a, b);
		}

		class Generator
		{
			public int Value { get; private set; }
			public readonly int Factor;
			public readonly int MultipleRequirement;

			public Generator(int value, int factor, int multipleRequirement)
			{
				Value = value;
				Factor = factor;
				MultipleRequirement = multipleRequirement;
			}

			public ushort Low16
				=> (ushort)Value;

			public void Next(bool requireMultiple)
			{
				do
				{
					long tempValue = Value;
					tempValue *= Factor;
					tempValue %= 2147483647;
					Value = (int)tempValue;
				} while (requireMultiple && Value % MultipleRequirement != 0);
			}

			public override string ToString()
				=> $"Value: {ToBinaryString(Value)}; Low: {ToBinaryString(Low16)}; Value: {Value}";
		}

		private static string ToBinaryString(int intValue)
			=> Convert.ToString(intValue, 2).PadLeft(32, '0');

		private static string ToBinaryString(ushort ushortValue)
			=> Convert.ToString(ushortValue, 2).PadLeft(16, '0');
	}
}
