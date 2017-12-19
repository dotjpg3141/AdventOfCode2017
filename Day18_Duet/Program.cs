using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Day18_Duet
{
	class Program
	{
		static void Main(string[] args)
		{
			var input = ParseInput("set i 31\r\nset a 1\r\nmul p 17\r\njgz p p\r\nmul a 2\r\nadd i -1\r\njgz i -2\r\nadd a -1\r\nset i 127\r\nset p 618\r\nmul p 8505\r\nmod p a\r\nmul p 129749\r\nadd p 12345\r\nmod p a\r\nset b p\r\nmod b 10000\r\nsnd b\r\nadd i -1\r\njgz i -9\r\njgz a 3\r\nrcv b\r\njgz b -1\r\nset f 0\r\nset i 126\r\nrcv a\r\nrcv b\r\nset p a\r\nmul p -1\r\nadd p b\r\njgz p 4\r\nsnd a\r\nset a b\r\njgz 1 3\r\nsnd b\r\nset f 1\r\nadd i -1\r\njgz i -11\r\nsnd a\r\njgz f -16\r\njgz a -19\r\n");
			var example1 = ParseInput("set a 1\r\nadd a 2\r\nmul a a\r\nmod a 5\r\nsnd a\r\nset a 0\r\nrcv a\r\njgz a -1\r\nset a 1\r\njgz a -2");
			var example2 = ParseInput("snd 1\r\nsnd 2\r\nsnd p\r\nrcv a\r\nrcv b\r\nrcv c\r\nrcv d");

			Debug.Assert(Duet1(example1) == 4);
			Console.WriteLine(Duet1(input));

			Debug.Assert(Duet2(example2) == 3);
			Console.WriteLine(Duet2(input));

			Console.WriteLine("Done");
			Console.ReadKey();
		}

		public static BigInteger Duet1(List<Instruction> list)
		{
			var mem = new MemoryContext() { Instructions = list };
			while (!mem.Terminated)
			{
				ExecuteInstruction(mem, soundMode: true);
				if (mem.HitRcv)
				{
					break;
				}
			}

			return mem.LastPlayedFrequency;
		}

		public static BigInteger Duet2(List<Instruction> list)
		{
			var mem0 = new MemoryContext() { Instructions = list, Registers = { ["p"] = 0 } };
			var mem1 = new MemoryContext() { Instructions = list, Registers = { ["p"] = 1 } };

			mem0.InputQueue = mem1.OutputQueue = new Queue<BigInteger>();
			mem1.InputQueue = mem0.OutputQueue = new Queue<BigInteger>();

			while (true)
			{
				if (!mem0.Terminated)
				{
					ExecuteInstruction(mem0, soundMode: false);
				}
				if (!mem1.Terminated)
				{
					ExecuteInstruction(mem1, soundMode: false);
				}

				bool terminated = mem0.Terminated && mem1.Terminated;
				bool deadLock = mem0.WaitingForInput && mem1.WaitingForInput;
				if (terminated || deadLock)
				{
					break;
				}
			}

			return mem1.SendCount;
		}

		private static void ExecuteInstruction(MemoryContext mem, bool soundMode)
		{
			var insn = mem.Instructions[mem.InstructionIndex];
			var instructionIndexOffset = 1;
			switch (insn.Type)
			{
				case InstructionType.Snd when soundMode:
					mem.LastPlayedFrequency = mem.Read(insn.Value1);
					break;

				case InstructionType.Rcv when soundMode:
					if (mem.Read(insn.Value1) != 0)
					{
						mem.HitRcv = true;
					}
					break;

				case InstructionType.Snd /* when !soundMode */:
					mem.OutputQueue.Enqueue(mem.Read(insn.Value1));
					mem.SendCount++;
					break;
				case InstructionType.Rcv /* when !soundMode */:
					if (mem.InputQueue.Count == 0)
					{
						instructionIndexOffset = 0; // wait for input
					}
					else
					{
						mem.Store(insn.Value1, mem.InputQueue.Dequeue());
					}
					break;

				case InstructionType.Set:
					mem.Store(insn.Value1, mem.Read(insn.Value2));
					break;

				case InstructionType.Add:
					mem.Store(insn.Value1, mem.Read(insn.Value1) + mem.Read(insn.Value2));
					break;

				case InstructionType.Mul:
					mem.Store(insn.Value1, mem.Read(insn.Value1) * mem.Read(insn.Value2));
					break;

				case InstructionType.Mod:
					mem.Store(insn.Value1, mem.Read(insn.Value1) % mem.Read(insn.Value2));
					break;

				case InstructionType.Jgz:
					if (mem.Read(insn.Value1) > 0)
					{
						instructionIndexOffset = (int)mem.Read(insn.Value2);
					}
					break;

				default:
					throw new InvalidOperationException();
			}

			mem.InstructionIndex += instructionIndexOffset;
		}

		public static List<Instruction> ParseInput(string input)
			=> input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Split(' '))
				.Select(args => new Instruction()
				{
					Type = (InstructionType)Enum.Parse(typeof(InstructionType), args[0], ignoreCase: true),
					Value1 = InsnValue.Parse(args[1]),
					Value2 = args.Length < 3 ? null : InsnValue.Parse(args[2]),
				})
				.ToList();

		public class MemoryContext
		{
			public List<Instruction> Instructions { get; set; }
			public Dictionary<string, BigInteger> Registers { get; set; } = new Dictionary<string, BigInteger>();
			public int InstructionIndex { get; set; }
			public bool Terminated
				=> InstructionIndex < 0
				|| InstructionIndex >= Instructions.Count;

			// NOTE(jpg): sound mode specific
			public bool HitRcv { get; set; }
			public BigInteger LastPlayedFrequency { get; set; }

			// NOTE(jpg): "multi-threading" mode specific
			public int SendCount { get; set; }
			public Queue<BigInteger> InputQueue { get; set; }
			public Queue<BigInteger> OutputQueue { get; set; }
			public bool WaitingForInput
				=> !Terminated
				&& Instructions[InstructionIndex].Type == InstructionType.Rcv
				&& InputQueue.Count == 0;

			public BigInteger Read(InsnValue insnValue)
				=> insnValue.RegisterName == null
					? insnValue.Value
					: Registers.TryGetValue(insnValue.RegisterName, out var val)
						? val
						: 0;

			public void Store(InsnValue register, BigInteger value)
			{
				Debug.Assert(register.RegisterName != null);
				Registers[register.RegisterName] = value;
			}
		}

		public struct Instruction
		{
			public InstructionType Type { get; set; }
			public InsnValue Value1 { get; set; }
			public InsnValue Value2 { get; set; }
		}

		public class InsnValue
		{
			private object _internalValue;

			public string RegisterName
			{
				get => _internalValue as string;
				set => _internalValue = value;
			}

			public BigInteger Value
			{
				get => _internalValue is BigInteger i ? i : throw new InvalidOperationException();
				set => _internalValue = value;
			}

			public static InsnValue Parse(string s)
				=> BigInteger.TryParse(s, out var val)
					? new InsnValue() { Value = val }
					: new InsnValue() { RegisterName = s };
		}

		public enum InstructionType
		{
			Snd,
			Set,
			Add,
			Mul,
			Mod,
			Rcv,
			Jgz,
		}
	}
}
