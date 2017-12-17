using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17_Spinlock
{
	class Program
	{
		static void Main(string[] args)
		{
			int input = 386;
			int example = 3;

			Debug.Assert(Spinlock1(example, 9, true) == 5);
			Console.WriteLine(Spinlock1(input));

			Spinlock2(example, 9, true);
			Console.WriteLine(Spinlock2(input));

			Console.WriteLine("Done");
			Console.ReadKey();
		}

		static int Spinlock1(int offset, int loopCount = 2017, bool debugOutput = false)
		{
			var buffer = new List<int>() { 0 };
			int position = 0;

			for (int i = 1; i <= loopCount; i++)
			{
				position = (position + offset) % buffer.Count + 1;
				if (position == buffer.Count)
				{
					buffer.Add(i);
				}
				else
				{
					buffer.Insert(position, i);
				}

				if (debugOutput)
				{
					Console.WriteLine($" > i: {i}, position: {position}, buffer: [ {string.Join(" ", buffer)} ]");
				}
			}

			position = (position + 1) % buffer.Count;
			return buffer[position];
		}

		static int Spinlock2(int offset, int loopCount = 50000000, bool debugOutput = false)
		{
			int position = 0;
			var bufferSize = 1;
			int lastValue = -1;

			for (int i = 1; i <= loopCount; i++)
			{
				position = (position + offset) % bufferSize + 1;
				bufferSize++;

				if (position == 1)
				{
					lastValue = i;

					if (debugOutput)
					{
						Console.WriteLine($" > i: {i}, position: {position}, buffer[1]: {lastValue}");
					}
				}
			}

			return lastValue;
		}
	}
}
