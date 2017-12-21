using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;

namespace Day22_FractalArt
{
	class Program
	{
		static void Main(string[] args)
		{
			var input = ParseInput("../.. => ##./##./.##\r\n#./.. => .../.#./##.\r\n##/.. => .../.##/#.#\r\n.#/#. => ##./#../#..\r\n##/#. => .##/#.#/#..\r\n##/## => ..#/.#./.##\r\n.../.../... => #.../.##./...#/#...\r\n#../.../... => ...#/..../..#./..##\r\n.#./.../... => ..../.##./###./....\r\n##./.../... => ###./#.##/..#./..#.\r\n#.#/.../... => #.../.#../#..#/..#.\r\n###/.../... => ..##/.##./#.../....\r\n.#./#../... => #.##/..../..../#.##\r\n##./#../... => .#.#/.#.#/##../.#..\r\n..#/#../... => .###/####/.###/##..\r\n#.#/#../... => ..../.#.#/..../####\r\n.##/#../... => .##./##.#/.###/#..#\r\n###/#../... => ####/...#/###./.###\r\n.../.#./... => ..##/#..#/###./###.\r\n#../.#./... => ###./..##/.#.#/.#.#\r\n.#./.#./... => ..#./..#./##.#/##..\r\n##./.#./... => #..#/###./..#./#.#.\r\n#.#/.#./... => .###/#.../.#.#/.##.\r\n###/.#./... => #.##/##../#.#./...#\r\n.#./##./... => #.##/#.##/#.##/.###\r\n##./##./... => ..##/#..#/.###/....\r\n..#/##./... => #..#/.##./##../####\r\n#.#/##./... => ###./###./..##/..##\r\n.##/##./... => ###./##.#/.##./###.\r\n###/##./... => ##../#..#/##../....\r\n.../#.#/... => ##.#/..#./..##/##..\r\n#../#.#/... => #..#/.###/.#../#.#.\r\n.#./#.#/... => ####/#.##/.###/###.\r\n##./#.#/... => #.../####/...#/.#.#\r\n#.#/#.#/... => ...#/.#.#/#..#/#.##\r\n###/#.#/... => ###./#.##/##.#/..##\r\n.../###/... => ..../##.#/.#../..##\r\n#../###/... => ####/..##/.##./.###\r\n.#./###/... => #.#./#.#./#.../#..#\r\n##./###/... => #..#/..##/#.##/#.#.\r\n#.#/###/... => .##./##.#/.#../####\r\n###/###/... => ####/##.#/.#../#.#.\r\n..#/.../#.. => #..#/#.##/.###/.###\r\n#.#/.../#.. => .##./#.../.#.#/....\r\n.##/.../#.. => .#.#/.#.#/##../####\r\n###/.../#.. => .#.#/.##./####/##.#\r\n.##/#../#.. => .###/.###/.###/#...\r\n###/#../#.. => ..##/#.../#.#./..#.\r\n..#/.#./#.. => #.#./##../##../####\r\n#.#/.#./#.. => ..../..##/#..#/..#.\r\n.##/.#./#.. => #.##/#..#/##.#/.##.\r\n###/.#./#.. => ...#/#.../#.#./.#..\r\n.##/##./#.. => .##./#..#/.##./...#\r\n###/##./#.. => ##.#/##.#/.##./...#\r\n#../..#/#.. => ##../..#./..#./#.#.\r\n.#./..#/#.. => #.#./##../#..#/#.##\r\n##./..#/#.. => #.##/###./###./.#.#\r\n#.#/..#/#.. => ..../...#/...#/#..#\r\n.##/..#/#.. => #..#/#.#./..##/.##.\r\n###/..#/#.. => ##../.#.#/.#../#.#.\r\n#../#.#/#.. => ####/.##./.##./.##.\r\n.#./#.#/#.. => ...#/.##./..#./.##.\r\n##./#.#/#.. => .#.#/.##./..#./.#.#\r\n..#/#.#/#.. => .#../##.#/##../#...\r\n#.#/#.#/#.. => .#.#/..#./#.../##..\r\n.##/#.#/#.. => ..#./#.#./###./#...\r\n###/#.#/#.. => ..../#.#./..##/##.#\r\n#../.##/#.. => .##./##../.#../..##\r\n.#./.##/#.. => ##../#.#./#.../####\r\n##./.##/#.. => ###./###./#.#./..##\r\n#.#/.##/#.. => ...#/#..#/..#./###.\r\n.##/.##/#.. => ..##/####/..../#.##\r\n###/.##/#.. => .#.#/#.../.##./#...\r\n#../###/#.. => ..#./.#.#/#..#/.##.\r\n.#./###/#.. => ####/..../####/#.##\r\n##./###/#.. => .###/..../#.#./####\r\n..#/###/#.. => ###./#.#./.#.#/#...\r\n#.#/###/#.. => #.#./#.#./..##/.##.\r\n.##/###/#.. => #.##/.###/.##./#.##\r\n###/###/#.. => #..#/.#../.#../.##.\r\n.#./#.#/.#. => .#../.##./##../..##\r\n##./#.#/.#. => .##./#.##/...#/#.#.\r\n#.#/#.#/.#. => ##.#/###./#.#./..#.\r\n###/#.#/.#. => ..../##../.###/###.\r\n.#./###/.#. => .#.#/.###/..../#..#\r\n##./###/.#. => #.../..#./#..#/.#..\r\n#.#/###/.#. => .#../##.#/##.#/.###\r\n###/###/.#. => #..#/.#.#/#.#./..#.\r\n#.#/..#/##. => .#../.###/...#/#.##\r\n###/..#/##. => ...#/...#/..##/...#\r\n.##/#.#/##. => #.#./###./.##./####\r\n###/#.#/##. => #.#./...#/...#/....\r\n#.#/.##/##. => ###./#.../##.#/..#.\r\n###/.##/##. => .#../#.../.###/.#..\r\n.##/###/##. => #.../..#./..#./.###\r\n###/###/##. => .#../.#../####/###.\r\n#.#/.../#.# => ##.#/##../...#/##.#\r\n###/.../#.# => ###./###./#..#/###.\r\n###/#../#.# => .###/..#./.#../#...\r\n#.#/.#./#.# => ##.#/.##./.#.#/##.#\r\n###/.#./#.# => ...#/...#/#.##/.##.\r\n###/##./#.# => #.../##../#.../....\r\n#.#/#.#/#.# => ####/.#../..##/..##\r\n###/#.#/#.# => ##../####/#.##/..##\r\n#.#/###/#.# => ##../..../..../####\r\n###/###/#.# => .#../.#.#/.###/.#.#\r\n###/#.#/### => ##../####/###./...#\r\n###/###/### => ###./#..#/##../.##.\r\n");
			var example = ParseInput("../.# => ##./#../...\r\n.#./..#/### => #..#/..../..../#..#");

			{
				var grid = Grid.Parse(".#./..#/###");
				Debug.Assert(grid.Flip().Flip().Equals(grid));
				Debug.Assert(grid.Rotate().Rotate().Rotate().Rotate().Equals(grid));
			}

			Debug.Assert(FractalArt1(example, iterCount: 2, debugOutput: true) == 12);
			Console.WriteLine(FractalArt1(input));
			Console.WriteLine(FractalArt1(input, iterCount: 18));

			Console.WriteLine("Done");
			Console.ReadKey();
		}

		static int FractalArt1(Dictionary<Grid, Grid> rules, int iterCount = 5, bool debugOutput = false)
		{
			var grid = Grid.Parse(".#./..#/###");

			for (int _ = 0; _ < iterCount; _++)
			{

				int splitSize = grid.Size % 2 == 0 ? 2
							  : grid.Size % 3 == 0 ? 3
							  : throw new InvalidOperationException();

				int smallGridCount = grid.Size / splitSize;

				var newGrid = new Grid(smallGridCount * (splitSize + 1));

				for (int i = 0; i < smallGridCount; i++)
				{
					for (int j = 0; j < smallGridCount; j++)
					{
						var smallGridIn = grid.Subgrid(i, j, splitSize);
						var smallGridOut = rules[smallGridIn];
						newGrid.SetSubgrid(i, j, smallGridOut);
					}
				}

				grid = newGrid;

				if (debugOutput)
				{
					Console.WriteLine(grid.ToString());
				}
			}

			return grid.CountEnabledPixels();
		}

		static Dictionary<Grid, Grid> ParseInput(string input)
		{
			return input.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
				.SelectMany(line =>
				{
					var rule = line.Split(new[] { ' ', '=', '>' }, StringSplitOptions.RemoveEmptyEntries)
						.Select(Grid.Parse)
						.ToList();

					var transformed = new List<Grid>() { rule[0] };

					for (int i = 1; i < 4; i++)
					{
						transformed.Add(transformed[i - 1].Rotate());
					}

					for (int i = 0; i < 4; i++)
					{
						transformed.Add(transformed[i].Flip());
					}

					return transformed.Select(t => (input: t, output: rule[1]));
				})
				.DistinctBy(rule => rule.input)
				.ToDictionary(
					rule => rule.input,
					rule => rule.output
				);
		}

		class Grid
		{
			private int hashCode = 0;
			public int Size => Data.GetLength(0);
			public bool[,] Data { get; }

			public Grid(int size)
			{
				this.Data = new bool[size, size];
			}

			public override bool Equals(object obj)
			{
				if (obj is Grid grid
					&& Size == grid.Size
					&& (hashCode == 0 || grid.hashCode == 0 || hashCode == grid.hashCode))
				{
					for (int i = 0; i < Size; i++)
					{
						for (int j = 0; j < Size; j++)
						{
							if (grid.Data[i, j] != this.Data[i, j])
							{
								return false;
							}
						}
					}
					return true;
				}
				return false;
			}

			public override int GetHashCode()
			{
				if (hashCode == 0)
				{
					hashCode = -566003807;
					for (int i = 0; i < Size; i++)
					{
						for (int j = 0; j < Size; j++)
						{
							hashCode = hashCode * 31 + Data[i, j].GetHashCode();
						}
					}
				}
				return hashCode;
			}

			public static Grid Parse(string s)
			{
				var lines = s.Split('/');
				var grid = new Grid(lines.Length);
				for (int i = 0; i < lines.Length; i++)
				{
					for (int j = 0; j < lines.Length; j++)
					{
						grid.Data[i, j] = lines[i][j] == '#';
					}
				}
				return grid;
			}

			public override string ToString()
			{
				var sb = new StringBuilder(Size * (Size + 2));
				for (int row = 0; row < Size; row++)
				{
					for (int col = 0; col < Size; col++)
					{
						sb.Append(this.Data[col, row] ? '#' : '.');
					}
					sb.AppendLine();
				}
				return sb.ToString();
			}

			public Grid Subgrid(int i, int j, int subgridSize)
			{
				var subgrid = new Grid(subgridSize);
				for (int ii = 0; ii < subgridSize; ii++)
				{
					for (int jj = 0; jj < subgridSize; jj++)
					{
						subgrid.Data[ii, jj] = this.Data[i * subgridSize + ii, j * subgridSize + jj];
					}
				}
				return subgrid;
			}

			public void SetSubgrid(int i, int j, Grid subgrid)
			{
				for (int ii = 0; ii < subgrid.Size; ii++)
				{
					for (int jj = 0; jj < subgrid.Size; jj++)
					{
						this.Data[i * subgrid.Size + ii, j * subgrid.Size + jj] = subgrid.Data[ii, jj];
					}
				}
			}

			public Grid Rotate()
			{
				var grid = new Grid(Size);
				for (int i = 0; i < Size; i++)
				{
					for (int j = 0; j < Size; j++)
					{
						grid.Data[i, j] = this.Data[Size - 1 - j, i];
					}
				}
				return grid;
			}

			public Grid Flip()
			{
				var grid = new Grid(Size);
				for (int i = 0; i < Size; i++)
				{
					for (int j = 0; j < Size; j++)
					{
						grid.Data[i, j] = this.Data[Size - 1 - i, j];
					}
				}
				return grid;
			}

			public int CountEnabledPixels()
			{
				int pixelOn = 0;
				for (int i = 0; i < Size; i++)
				{
					for (int j = 0; j < Size; j++)
					{
						pixelOn += Data[i, j] ? 1 : 0;
					}
				}
				return pixelOn;
			}
		}
	}
}
