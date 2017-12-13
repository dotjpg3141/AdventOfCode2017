using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13_PacketScanners
{
	class Program
	{
		static void Main(string[] args)
		{
			var example = ParseInput("0: 3\r\n1: 2\r\n4: 4\r\n6: 4");
			var input = ParseInput("0: 3\r\n1: 2\r\n2: 4\r\n4: 4\r\n6: 5\r\n8: 6\r\n10: 8\r\n12: 8\r\n14: 6\r\n16: 6\r\n18: 9\r\n20: 8\r\n22: 6\r\n24: 10\r\n26: 12\r\n28: 8\r\n30: 8\r\n32: 14\r\n34: 12\r\n36: 8\r\n38: 12\r\n40: 12\r\n42: 12\r\n44: 12\r\n46: 12\r\n48: 14\r\n50: 12\r\n52: 12\r\n54: 10\r\n56: 14\r\n58: 12\r\n60: 14\r\n62: 14\r\n64: 14\r\n66: 14\r\n68: 14\r\n70: 14\r\n72: 14\r\n74: 20\r\n78: 14\r\n80: 14\r\n90: 17\r\n96: 18\r\n");

			Debug.Assert(GetFirewallSeverity(example) == 24);
			Console.WriteLine(GetFirewallSeverity(input));

			Debug.Assert(DelayToPassThroughFirewallUncaught(example) == 10);
			Console.WriteLine(DelayToPassThroughFirewallUncaught(input));

			Console.ReadKey();
			Console.WriteLine();
		}

		public static int GetFirewallSeverity(List<(int layerDepth, int scannerRange)> input)
		{
			return input
				.Where(layer => HitScanner(layer.scannerRange, layer.layerDepth))
				.Select(layer => layer.layerDepth * layer.scannerRange)
				.Sum();
		}

		public static int DelayToPassThroughFirewallUncaught(List<(int layerDepth, int scannerRange)> input)
		{
			int delay = 0;
			while (input.Any(layer => HitScanner(layer.scannerRange, layer.layerDepth + delay)))
			{
				delay++;
			}
			return delay;
		}

		public static bool HitScanner(int scannerRange, int time)
		{
			bool isScannerAtPosition0 = time % (scannerRange * 2 - 2) == 0;
			return isScannerAtPosition0;
		}

		public static List<(int layerDepth, int scannerRange)> ParseInput(string input)
		{
			return input.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Split(':'))
				.Select(cols => (int.Parse(cols[0].Trim()), int.Parse(cols[1].Trim())))
				.ToList();
		}
	}
}
