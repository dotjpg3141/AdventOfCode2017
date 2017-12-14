using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnotHash = Day10_KnotHash.Program;

namespace Day14_DiskDefragmentation
{
	class Program
	{
		static void Main(string[] args)
		{
			var input = CreateDiskGrid("nbysizxe");
			var example = CreateDiskGrid("flqrgnkx");

			Debug.Assert(CountUsedSquares(example) == 8108);
			Console.WriteLine(CountUsedSquares(input));

			Debug.Assert(CountRegions(example) == 1242);
			Console.WriteLine(CountRegions(input));

			Console.WriteLine("Done");
			Console.ReadKey();
		}

		public static int CountUsedSquares(DiskState[,] grid)
		{
			int count = 0;
			for (int i = 0; i < 128; i++)
			{
				for (int j = 0; j < 128; j++)
				{
					if (grid[i, j] == DiskState.UsedUnvisited)
					{
						count++;
					}
				}
			}
			return count;
		}

		public static int CountRegions(DiskState[,] grid)
		{
			int count = 0;
			for (int i = 0; i < 128; i++)
			{
				for (int j = 0; j < 128; j++)
				{
					if (grid[i, j] == DiskState.UsedUnvisited)
					{
						FillRegion(grid, i, j);
						count++;
					}
				}
			}
			return count;
		}

		private static void FillRegion(DiskState[,] grid, int i, int j)
		{
			if (0 <= i && i < 128 &&
				0 <= j && j < 128 &&
				grid[i, j] == DiskState.UsedUnvisited)
			{
				grid[i, j] = DiskState.UsedVisited;
				FillRegion(grid, i - 1, j);
				FillRegion(grid, i + 1, j);
				FillRegion(grid, i, j - 1);
				FillRegion(grid, i, j + 1);
			}
		}

		static DiskState[,] CreateDiskGrid(string input)
		{
			var grid = new DiskState[128, 128];
			for (int i = 0; i < 128; i++)
			{
				var key = input + "-" + i;
				var hash = KnotHash.KnotHash(key);

				var rowString = string.Join("", hash.SelectMany(ToBinaryString));
				for (int j = 0; j < 128; j++)
				{
					grid[i, j] = rowString[j] == '1' ? DiskState.UsedUnvisited : DiskState.Free;
				}
			}
			return grid;
		}

		private static string ToBinaryString(byte byteValue) 
			=> Convert.ToString(byteValue, 2).PadLeft(8, '0');

		public enum DiskState
		{
			Free,
			UsedUnvisited,
			UsedVisited,
		}

	}
}
