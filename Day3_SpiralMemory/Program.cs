using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3_SpiralMemory
{
	class Program
	{
		private static int[] NeighbourAxis = { -1, 0, +1 };

		static void Main(string[] args)
		{
			int input = 325489;

			Debug.Assert(SpiralDistance(1) == 0);
			Debug.Assert(SpiralDistance(12) == 3);
			Debug.Assert(SpiralDistance(1024) == 31);
			Console.WriteLine(SpiralDistance(input));

			Debug.Assert(SpiralNeighbourSum().ElementAt(0).sum == 1);
			Debug.Assert(SpiralNeighbourSum().ElementAt(1).sum == 1);
			Debug.Assert(SpiralNeighbourSum().ElementAt(2).sum == 2);
			Debug.Assert(SpiralNeighbourSum().ElementAt(3).sum == 4);
			Debug.Assert(SpiralNeighbourSum().ElementAt(4).sum == 5);
			Console.WriteLine(SpiralNeighbourSum(input));

			Console.WriteLine("Done");
			Console.ReadKey();
		}

		private static int SpiralDistance(int target)
		{
			var (x, y, _) = Spiral().First(item => item.index == target);
			return Math.Abs(x) + Math.Abs(y);
		}

		private static int SpiralNeighbourSum(int exlusiveMinSum)
		{
			return SpiralNeighbourSum().First(item => item.sum > exlusiveMinSum).sum;
		}

		private static IEnumerable<(int x, int y, int index)> Spiral()
		{
			int x = 0;
			int y = 0;
			int len = 0;
			int dx = +1;
			int dy = 0;
			int index = 1;

			while (true)
			{
				if (dx != 0)
				{
					len++;
				}

				for (int i = 0; i < len; i++)
				{
					yield return (x, y, index++);
					x += dx;
					y += dy;
				}

				// turn right
				int oldX = dx;
				dx = -dy;
				dy = oldX;
			}
		}
		
		private static IEnumerable<(int sum, int index)> SpiralNeighbourSum()
		{
			var spiral = new Dictionary<(int x, int y), int>
			{
				[(0, 0)] = 1
			};

			foreach (var (x, y, index) in Spiral())
			{

				var sum = (from nx in NeighbourAxis
						   from ny in NeighbourAxis
						   select (x + nx, y + ny) into position
						   where spiral.ContainsKey(position)
						   select spiral[position]).Sum();

				spiral[(x, y)] = sum;
				yield return (sum, index);
			}

			throw new InvalidOperationException();
		}
	}
}
