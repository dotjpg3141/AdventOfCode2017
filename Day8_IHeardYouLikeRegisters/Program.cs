﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day8_IHeardYouLikeRegisters
{
	class Program
	{

		private static readonly Dictionary<string, Func<int, int, int>> Operations = new Dictionary<string, Func<int, int, int>>
		{
			["inc"] = (a, b) => a + b,
			["dec"] = (a, b) => a - b,
		};

		private static readonly Dictionary<string, Func<int, int, bool>> Comparisons = new Dictionary<string, Func<int, int, bool>>
		{
			["<"] = (a, b) => a < b,
			[">"] = (a, b) => a > b,
			["<="] = (a, b) => a <= b,
			[">="] = (a, b) => a >= b,
			["=="] = (a, b) => a == b,
			["!="] = (a, b) => a != b,
		};

		static void Main(string[] args)
		{
			var exampleInput = new string[]
			{
				"b inc 5 if a > 1	 ",
				"a inc 1 if b < 5	 ",
				"c dec -10 if a >= 1 ",
				"c inc -20 if c == 10"
			};
			var example = ToInstructions(exampleInput);
			var exampleState = new Dictionary<string, int>();

			Simmulate(example[0], exampleState);
			Debug.Assert(exampleState.Count == 0);

			Simmulate(example[1], exampleState);
			Debug.Assert(exampleState["a"] == 1);

			Simmulate(example[2], exampleState);
			Debug.Assert(exampleState["c"] == 10);

			Simmulate(example[3], exampleState);
			Debug.Assert(exampleState["c"] == -10);

			(int lastMax, int allTimeMax) = Simmulate(exampleInput);
			Debug.Assert(lastMax == 1);
			Debug.Assert(allTimeMax == 10);

			Console.WriteLine(Simmulate(input));

			Console.WriteLine("Done");
			Console.ReadKey();
		}

		static (int lastMax, int allTimeMax) Simmulate(string[] input)
		{
			var instructions = ToInstructions(input);
			var registerState = new Dictionary<string, int>();
			int allTimeMax = 0;

			foreach (var instruction in instructions)
			{
				Simmulate(instruction, registerState);
				if (registerState.Count != 0)
				{
					allTimeMax = Math.Max(allTimeMax, registerState.Values.Max());
				}
			}

			return (lastMax: registerState.Values.Max(), allTimeMax);
		}

		static void Simmulate((string var, string op, int opArg, string cmpLhs, string cmpOp, int cmpRhs) i, Dictionary<string, int> registerState)
		{
			int GetValue(string register)
				=> registerState.TryGetValue(register, out int value) ? value : 0;

			var cmp = Comparisons[i.cmpOp];
			int cmpLhs = GetValue(i.cmpLhs);
			int cmpRhs = i.cmpRhs;

			if (cmp(cmpLhs, cmpRhs))
			{
				var op = Operations[i.op];
				int opLhs = GetValue(i.var);
				int opRhs = i.opArg;

				registerState[i.var] = op(opLhs, opRhs);
			}
		}

		private static List<(string var, string op, int opArg, string cmpLhs, string cmpOp, int cmpRhs)> ToInstructions(string[] input)
		{
			return input.Select(line =>
			{
				var parts = line.Split(' ', '\t');
				Debug.Assert(parts[3] == "if");
				int opArg = int.Parse(parts[2]);
				int cmpRhs = int.Parse(parts[6]);
				return (var: parts[0], op: parts[1], opArg, cmpLhs: parts[4], cmpOp: parts[5], cmpRhs);
			}).ToList();
		}

		private static string[] input =
		{
			"n dec 271 if az < 3", "f inc -978 if nm <= 9", "g inc -336 if ga < 2", "egk dec 437 if y < 5",
			"z dec -550 if g == -336", "mx dec 12 if bqr == 0", "mx dec 433 if ns == 0", "ic inc 506 if g <= -327",
			"ga dec -560 if ic != 506", "bqr dec 570 if az >= -9", "g dec 372 if egk != -429", "f dec -863 if b >= -9",
			"gyc inc 844 if av >= 7", "cr inc -781 if mx >= -453", "ga dec 346 if cr == -781", "b inc -162 if z < 554",
			"nno inc 504 if g <= -700", "mx dec -32 if vkg <= 6", "mx dec -961 if nno < 509", "az dec 320 if ic < 510",
			"nm dec 594 if ga != -354", "cr inc -784 if az <= -311", "bqr inc -321 if mx > 547", "y inc -951 if egk > -438",
			"y inc 703 if y <= -950", "vkg inc -852 if g > -716", "n dec -596 if az < -316", "n dec 410 if e <= 4",
			"f inc -693 if ic <= 510", "egk dec 498 if zqo != 7", "rn inc 626 if u == 0", "bqr dec -770 if bqr > -898",
			"n dec 193 if ic == 506", "egk inc -263 if e == 0", "cr dec -473 if mx <= 554", "vkg inc 667 if e != 10",
			"ga dec 793 if v <= 5", "f dec -673 if v != 0", "g inc 797 if g >= -708", "n inc 361 if v == 0",
			"y inc 399 if zqo == 0", "av dec -894 if zqo == 0", "az dec 785 if z <= 559", "y inc -121 if nm < -593",
			"v inc 873 if vkg > -189", "ns inc -205 if n >= 80", "cr dec 452 if ns != -202", "nm dec -35 if f < -799",
			"ic inc 790 if zqo > 3", "gyc dec 456 if nm <= -565", "rn inc 198 if y != 32", "y dec -917 if f == -806",
			"e dec -694 if zqo != 7", "nno inc 676 if rn < 825", "u inc 414 if ns >= -213", "f dec -405 if bqr <= -113",
			"g dec 456 if ga > -1145", "by inc 613 if nno != 1185", "n dec 414 if ga >= -1131", "ga dec 844 if u <= 418",
			"av dec -404 if ga >= -1974", "g dec -171 if ns > -208", "av dec -302 if f == -403", "ic inc -935 if y >= 30",
			"u inc -807 if bqr > -126", "az inc -536 if v >= 865", "vkg dec -77 if vkg == -185", "n dec 209 if n == 83",
			"u inc -92 if y >= 26", "ic dec 43 if u != -491", "ic inc 701 if z == 550", "bqr inc -473 if e != 699",
			"av dec -44 if egk != -1198", "e dec 134 if ga == -1974", "v dec 479 if mx <= 544", "n inc -748 if y >= 21",
			"bqr inc 726 if u >= -492", "z dec -368 if ns == -205", "y inc -296 if e >= 694", "f inc 271 if nno >= 1177",
			"y dec -881 if cr < -1535", "nno inc -493 if az > -1649", "z dec -434 if v != 882", "bqr inc 783 if e <= 694",
			"gyc inc 930 if gyc != -1", "nm dec -712 if e < 696", "mx inc -342 if nno > 685", "av inc -292 if gyc > 927",
			"rn inc -269 if rn <= 831", "vkg dec -799 if nno == 687", "v inc 31 if vkg < 694", "egk dec -227 if v != 913",
			"ga inc -248 if u == -485", "ga dec -655 if ic >= 228", "nm inc 335 if by != 613", "av inc 251 if u >= -477",
			"bqr inc 834 if ns >= -209", "gyc dec -580 if rn > 549", "u dec 390 if e < 701", "cr inc -936 if b > -170",
			"nm dec -60 if g < -188", "ic inc -226 if by < 615", "y dec -698 if zqo != 0", "vkg inc -938 if av == 904",
			"vkg inc -131 if zqo == 0", "y inc -792 if zqo < -3", "gyc dec -43 if ns < -211", "ga dec 783 if az <= -1637",
			"az inc -359 if cr <= -2479", "e inc -117 if v != 908", "rn inc 231 if av < 898", "nno inc -594 if z != 1354",
			"egk inc -119 if b >= -170", "mx dec 866 if mx != 214", "g inc -688 if mx < -655", "ga inc 451 if z <= 1352",
			"bqr inc 315 if g != -876", "vkg dec -636 if egk != -1090", "bqr inc -395 if by < 610", "mx inc 914 if ga < -1908",
			"by dec -623 if egk > -1090", "ns dec 219 if vkg >= -380", "egk dec -976 if av >= 896", "egk dec -681 if ic != -1",
			"vkg inc 971 if nno < 101", "nno dec 490 if bqr > 2060", "u inc 479 if z <= 1358", "z dec -552 if nno < -389",
			"y inc 696 if nno <= -405", "nm dec 876 if nno >= -399", "f dec 197 if nm > -670", "f inc 584 if e <= 575",
			"av dec -548 if y <= 616", "cr dec -975 if mx >= -664", "vkg dec -532 if egk > 567", "cr inc 115 if zqo <= 2",
			"e inc 315 if nm <= -663", "e inc -850 if ns > -430", "vkg inc -72 if z < 1906", "b inc 7 if y != 618",
			"cr dec -270 if by < 613", "v inc 505 if ns > -430", "ga dec 51 if nm != -670", "z dec -657 if zqo <= 1",
			"u inc -777 if nm == -663", "y dec -37 if nno < -396", "gyc inc 62 if f < -322", "z inc -565 if az > -1996",
			"e dec -223 if vkg <= 516", "nno inc 481 if e == 34", "rn inc 316 if nno >= -406", "e dec 531 if ns <= -423",
			"vkg inc -940 if v >= 1408", "by inc 935 if b <= -152", "rn inc 254 if ns < -416", "gyc dec 822 if zqo > -6",
			"n inc 334 if b < -155", "by inc 987 if nno == -397", "gyc dec -701 if ic >= -6", "nno inc -764 if z <= 2568",
			"nno inc -541 if mx == -660", "ns dec -327 if ns != -424", "z inc -328 if bqr == 2064", "cr inc 550 if cr != -1385",
			"ga dec -441 if rn <= 1123", "gyc inc -461 if zqo == 0", "egk dec 554 if ic < -6", "av inc 300 if zqo == 0",
			"nno dec 324 if av <= 1754", "y dec 235 if b > -165", "rn inc -1 if v < 1415", "b dec 230 if y > 413",
			"mx inc -836 if b <= -384", "av dec 464 if zqo != -7", "rn inc 208 if egk >= 563", "e dec 385 if v >= 1413",
			"g dec -443 if z < 2240", "v dec 296 if nno < -2027", "nm dec 394 if az != -1992", "u dec 609 if nno == -2026",
			"nno dec -178 if ns > -428", "f dec -340 if z >= 2231", "e dec 124 if u > -1783", "bqr dec -221 if bqr == 2064",
			"rn dec -427 if by != 2539", "n dec -242 if cr == -840", "az dec 267 if y < 418", "by inc 737 if cr <= -833",
			"rn inc -938 if ic >= 1", "az inc 181 if zqo >= -6", "f inc -874 if z >= 2233", "cr inc -867 if vkg <= -414",
			"mx inc 424 if ic >= -6", "f inc -125 if g <= -435", "az inc 30 if n < -625", "av inc -575 if y < 418",
			"ga dec -726 if ns <= -418", "u dec 1000 if b >= -393", "ns inc -88 if av < 720", "ns inc -80 if f < -983",
			"z dec 73 if e != -613", "bqr dec 350 if nm > -1065", "az inc 247 if gyc > 980", "v dec -156 if g != -435",
			"mx inc 32 if f > -996", "b dec 349 if mx < -1032", "mx dec -934 if nm != -1067", "ga inc -502 if egk < 573",
			"gyc inc -106 if nm == -1057", "ga inc 669 if mx != -102", "nno dec -335 if g >= -438", "g dec -757 if v != 1566",
			"cr dec -887 if z == 2233", "rn dec -768 if by < 3280", "bqr dec -289 if gyc < 890", "u inc 817 if by > 3265",
			"nm dec -136 if by <= 3270", "b dec 192 if y == 417", "f inc -787 if b <= -929", "f dec 188 if f == -988",
			"cr inc -488 if nno != -1846", "nno inc -116 if az > -1807", "cr inc 899 if by < 3278", "nno dec -454 if b >= -928",
			"f dec 237 if cr != -409", "e inc -910 if b != -926", "zqo dec -710 if egk > 564", "v dec 651 if cr != -407",
			"z inc 45 if ic < 10", "rn inc -16 if n > -633", "u dec -433 if n != -636", "z inc -55 if vkg >= -425",
			"y inc 536 if v <= 916", "nm dec 269 if cr == -409", "vkg inc -811 if gyc > 875", "y inc -269 if rn <= 1576",
			"rn inc -445 if av >= 709", "av inc -131 if ic != 1", "egk dec -35 if y <= 687", "mx dec -947 if nm == -1326",
			"bqr inc -296 if nno >= -1399", "vkg inc -176 if az == -1809", "z inc -7 if ga <= -1058", "v dec -105 if v != 908",
			"by dec -935 if g > 315", "y dec -674 if rn <= 1132", "ga dec -482 if v <= 1026", "nm dec 500 if f >= -1176",
			"by dec 653 if f < -1177", "cr dec 156 if mx == 841", "ns inc 446 if u > -1535", "e inc 256 if mx < 840",
			"av dec -883 if nm == -1826", "gyc inc 204 if by < 4198", "g dec 158 if av > 1456", "z dec -314 if av <= 1466",
			"egk inc -367 if cr > -568", "by dec -847 if f <= -1171", "f dec -361 if z < 2532", "z dec 647 if ns == -146",
			"z inc -416 if z <= 1892", "e dec 138 if cr <= -571", "ic inc -393 if gyc < 891", "zqo dec 338 if e < -605",
			"zqo dec -644 if ic == -390", "ns inc -790 if b != -917", "az dec 136 if av == 1465", "nm inc 971 if gyc != 884",
			"av inc 171 if ic != -390", "u inc -715 if zqo <= 1024", "av inc 27 if zqo != 1021", "n inc 845 if zqo < 1023",
			"cr inc -576 if v == 1019", "y inc 456 if ga >= -583", "mx dec 843 if f > -823", "v dec -589 if ns != -926",
			"e dec -797 if nno > -1395", "g dec 828 if bqr >= 1925", "vkg inc 176 if az > -1949", "nno inc 261 if zqo < 1017",
			"f inc -429 if b == -926", "f dec 658 if mx == -2", "e dec -221 if gyc < 890", "y dec 362 if v < 1605",
			"rn inc 849 if v != 1610", "b inc -650 if y <= 1363", "ic inc 39 if g != -668", "by dec -365 if u > -2248",
			"nm dec 779 if u == -2247", "nm inc -482 if cr <= -1137", "ns dec -873 if nm == -3087", "e dec -993 if rn <= 1979",
			"vkg dec 291 if n <= 220", "nno inc 937 if nno > -1139", "bqr inc -321 if b <= -1570", "av inc 86 if v < 1612",
			"by inc 872 if gyc > 878", "egk inc 858 if bqr > 1603", "e inc -199 if ns <= -62", "az dec 600 if v >= 1615",
			"nno inc -115 if g < -660", "ga inc 614 if f > -1910", "g inc 950 if zqo > 1009", "f dec -319 if cr == -1141",
			"gyc dec 498 if nm <= -3078", "u inc 61 if z != 1464", "av inc 127 if zqo > 1012", "ic inc -78 if f < -1573",
			"b dec -601 if nm <= -3082", "av inc -475 if gyc < 396", "av dec 613 if gyc < 383", "vkg inc 425 if rn <= 1981",
			"nno inc 543 if az >= -1946", "rn dec 280 if vkg >= -1096", "ns dec -366 if gyc <= 388",
			"v dec -558 if egk == 1093", "vkg inc -446 if ns <= 301", "y dec 617 if b <= -970", "n dec -890 if az != -1947",
			"vkg dec -102 if nm != -3092", "e inc -668 if ga < 33", "zqo inc -305 if e >= 529", "vkg inc -97 if vkg != -997",
			"rn inc 188 if nm < -3087", "cr dec 613 if rn < 1700", "v dec 647 if g < 284", "az dec -205 if e == 531",
			"av inc -359 if vkg == -1091", "y dec 252 if ns >= 301", "n inc 251 if f != -1593", "mx inc 438 if gyc < 385",
			"by dec -984 if nm != -3087", "mx dec 354 if ic < -425", "by dec 395 if ns > 298", "egk inc 119 if b == -966",
			"gyc inc -887 if vkg == -1091", "rn inc 898 if ga != 30", "mx dec 35 if y >= 485", "e inc 889 if ns != 300",
			"ga dec 992 if b > -968", "g dec 432 if cr > -1755", "nno dec -669 if gyc > -504", "bqr inc -442 if rn == 1697",
			"u inc -44 if b > -976", "u inc 5 if gyc <= -497", "gyc inc 36 if ns <= 304", "nm inc -528 if b >= -973",
			"y inc -617 if mx <= -400", "g dec 47 if e >= 1418", "b inc 292 if egk != 1083", "vkg inc 734 if ga <= 30",
			"n inc -678 if b != -691", "ic dec 581 if vkg >= -365", "u dec 562 if b == -683", "nno dec 751 if cr > -1757",
			"egk inc 709 if gyc != -461", "n inc -347 if rn == 1697", "gyc inc 960 if cr <= -1751", "z dec 627 if ns < 310",
			"gyc dec -190 if z < 842", "v inc 868 if mx == -398", "n inc -280 if b != -683", "n inc -302 if e == 1420",
			"av dec -389 if g != -209", "nm inc 241 if ic == -1018", "n inc -493 if nno <= 157", "f dec -932 if n == -466",
			"u inc -880 if y >= 480", "y dec -697 if egk < 1807", "az dec 332 if nno != 149", "g dec 971 if z <= 848",
			"by inc -510 if z >= 849", "u inc 103 if bqr < 1161", "av inc -830 if gyc > 678", "rn inc 110 if ns < 306",
			"b dec 616 if ga < 40", "egk inc -749 if y <= 1176", "z inc 669 if ns != 301", "n dec 365 if nno >= 152",
			"ns inc -464 if egk > 1801", "e inc -516 if by > 5894", "bqr inc 934 if g < -1165", "nno inc 224 if cr < -1744",
			"f inc -480 if n >= -456", "gyc inc 101 if g > -1168", "cr inc 501 if e == 905", "z dec -836 if f < -643",
			"ic dec 750 if rn != 1808", "u dec -318 if gyc <= 687", "ga inc -631 if av >= 430", "nno inc -261 if ga > -606",
			"b dec 723 if by >= 5889", "ns inc -109 if bqr != 2095", "cr dec 731 if e >= 903", "nno dec -251 if b >= -2024",
			"mx dec -206 if rn > 1810", "ns dec 249 if rn > 1799", "u inc 201 if egk < 1809", "vkg dec 283 if z > 2338",
			"rn dec -759 if u >= -3141", "f dec 540 if y != 1186", "by dec -523 if z == 2345", "by inc -894 if ga > -608",
			"rn inc -418 if cr >= -2489", "bqr inc -837 if nno > 361", "cr inc -485 if zqo == 711", "y dec -672 if nno == 364",
			"g inc 159 if nm == -3087", "vkg inc 873 if az >= -2074", "b inc 83 if z != 2347", "gyc inc 321 if e < 904",
			"e dec 283 if gyc < 689", "f dec 257 if b >= -1948", "by inc -526 if az < -2063", "cr dec 119 if b == -1939",
			"e inc -534 if g <= -1016", "bqr dec 923 if av == 430", "gyc dec 513 if bqr > 334", "u inc -756 if z > 2339",
			"nm dec 343 if gyc >= 165", "bqr inc -114 if ga <= -601", "gyc inc 355 if cr >= -3091",
			"gyc dec -458 if mx >= -398", "by dec 24 if v != 1519", "cr inc -155 if v <= 1526", "v dec 512 if av >= 427",
			"f dec 297 if f > -911", "nno dec 783 if ns <= -524", "az inc 985 if ga > -592", "cr inc 300 if egk >= 1800",
			"vkg dec 542 if mx == -391", "zqo dec -844 if bqr == 218", "av inc 860 if gyc == 985", "nno dec -253 if nno != 366",
			"v dec 542 if b >= -1941", "z dec -76 if ns > -526", "y inc -336 if az == -2073", "nm inc 995 if egk > 1793",
			"f inc -95 if e < 622", "zqo inc -46 if rn > 1379", "b dec 470 if av <= 1299", "gyc inc 839 if f >= -1304",
			"bqr inc -766 if bqr < 227", "n inc 980 if v < 460", "bqr dec -757 if ns >= -528", "g dec -507 if f > -1308",
			"mx inc 249 if ns > -522", "av dec 524 if g < -507", "e dec -962 if rn > 1397", "ns inc 139 if zqo < 666",
			"av inc -443 if zqo <= 656", "z dec -753 if rn != 1391", "g inc 616 if v <= 474", "ga dec 196 if nno < 618",
			"av dec -317 if gyc > 1826", "f dec -673 if cr < -2939", "b inc -670 if zqo > 670", "cr inc -948 if ga <= -795",
			"av dec -977 if zqo == 665", "f dec 521 if ic != -1764", "ga inc 342 if nno != 618", "av dec -989 if gyc >= 1823",
			"rn inc -118 if mx < -137", "ic inc 874 if nm < -2429", "cr dec 441 if by > 4996", "az dec 920 if cr == -4333",
			"by inc 486 if z >= 3168", "zqo dec -614 if u <= -3897", "zqo inc -359 if g > 105", "cr inc 725 if av <= 3257",
			"nm dec 816 if mx >= -133", "f inc -174 if az < -2996", "n dec -832 if egk <= 1803", "z dec -351 if av > 3257",
			"rn inc 814 if egk == 1802", "rn dec 516 if bqr > 206", "ic inc -181 if ns >= -383", "cr dec 476 if av < 3261",
			"rn dec -446 if mx > -141", "rn dec 966 if bqr > 213", "u dec 426 if b != -2408", "ic inc 841 if ic >= -1071",
			"u inc -990 if n < 371", "gyc inc -436 if az != -2991", "egk inc 673 if by == 5485", "g dec -510 if egk >= 2474",
			"v inc -104 if ic != -225", "b dec 862 if ga != -452", "n dec -733 if mx != -132", "b dec -726 if nno > 618",
			"f dec -259 if rn == 597", "e inc 51 if nm == -2435", "y inc -251 if ns >= -388", "bqr inc -684 if u >= -5325",
			"bqr inc -427 if b > -3278", "n inc 526 if mx >= -149", "f inc -727 if egk != 2475", "ga inc 345 if g >= 622",
			"ic dec -398 if ga > -113", "rn dec 581 if egk < 2478", "rn inc 926 if g < 614", "ns inc 469 if nno != 615",
			"n inc -870 if vkg > -311", "mx inc 124 if ga <= -103", "z dec -983 if ns != 96", "vkg dec 530 if mx < -18",
			"rn dec -899 if g < 625", "gyc dec 98 if g < 627", "av inc -497 if n < 760", "nno dec -115 if egk >= 2470",
			"z inc 610 if z > 4156", "mx inc -184 if ns > 91", "cr dec 724 if nm != -2438", "ns dec 527 if nno > 727",
			"ns inc -177 if cr >= -4811", "v dec 423 if rn != 921", "vkg dec 658 if av >= 2755", "y inc -464 if bqr > -890",
			"nm dec 440 if e < 673", "ic dec -963 if mx == -18", "n dec -613 if by < 5488", "v inc -412 if egk < 2482",
			"vkg inc -278 if y < 1609", "by inc 200 if e == 672", "ns inc -627 if v == -51", "n dec -176 if e == 672",
			"n inc -511 if gyc <= 1289", "zqo inc 589 if ga == -108", "v dec 533 if egk < 2485", "zqo dec -93 if n > 1536",
			"e dec -618 if nno < 742", "gyc inc 258 if f != -1139", "ga inc 562 if vkg >= -1254", "n inc -492 if f > -1158",
			"rn dec -267 if nno < 738", "v dec -263 if y < 1603", "vkg inc 206 if mx == -18", "av dec -610 if n <= 1060",
			"by dec -352 if mx <= -24", "zqo dec 74 if vkg >= -1046", "ns inc -251 if ga > 457", "cr dec 531 if n != 1047",
			"ga inc 892 if by < 5683", "vkg inc -687 if b >= -3273", "av inc -208 if v <= -580", "g dec 550 if az != -2985",
			"ga dec -595 if gyc >= 1548", "egk dec -240 if zqo >= 930", "b dec 262 if az == -2992", "egk dec 831 if ga < 1055",
			"zqo inc 928 if ga > 1044", "nno inc -90 if gyc >= 1542", "e inc -47 if f < -1144", "u dec 131 if zqo > 1871",
			"g dec 268 if f != -1148", "b dec -964 if n <= 1045", "gyc dec -952 if az > -3001", "egk inc 324 if vkg != -1735",
			"by inc -460 if av == 3167", "bqr inc 494 if nm >= -2878", "by inc -616 if rn < 1191", "ga dec -150 if u != -5312",
			"ns dec -535 if av == 3161", "rn inc -720 if ic > 1135", "n inc -821 if gyc < 2508", "n inc 670 if ns < -705",
			"f inc 419 if ga == 1197", "mx dec -96 if nm >= -2876", "vkg inc -270 if n < 909", "z dec -907 if ns < -700",
			"ns inc 160 if gyc >= 2491", "av dec -250 if e > 1240", "f inc 768 if ns >= -542", "ga inc 443 if gyc > 2498",
			"rn dec 107 if g == 72", "ga inc -420 if mx != 74", "mx dec -763 if ic == 1140", "b inc 586 if gyc <= 2505",
			"mx dec -948 if gyc > 2509", "zqo dec -384 if vkg < -1991", "z dec 234 if rn > 1072", "gyc dec -821 if v > -590",
			"z inc 83 if v == -584", "egk dec -835 if b <= -2941", "cr dec -495 if v > -590", "by inc 259 if cr <= -4852",
			"cr inc 530 if mx != 77", "vkg inc 381 if g <= 80", "by dec -71 if az == -2992", "mx inc -453 if g > 64",
			"ic inc 410 if vkg > -1618", "f inc 945 if n == 901", "y dec 409 if nm >= -2884", "z inc 870 if vkg >= -1606",
			"av inc 187 if z >= 5521", "nno inc -852 if ga > 1212", "cr inc 788 if av < 3608", "e dec -182 if by != 5143",
			"nno inc -706 if ga >= 1228", "vkg inc 782 if z < 5532", "mx dec 839 if nno > -219", "v dec 267 if b > -2952",
			"v inc 901 if gyc == 3321", "g dec -820 if rn != 1079", "ic dec -221 if zqo <= 2259", "ga inc -784 if e == 1425",
			"ga inc -569 if bqr >= -404", "u inc -206 if mx <= -1212", "gyc inc -685 if nm == -2881",
			"e dec 269 if mx <= -1208", "av dec -207 if by >= 5137", "nno inc -109 if ga != -131", "ga dec 3 if ic != 1761",
			"zqo inc 181 if nm < -2866", "y inc -15 if by >= 5140", "rn dec -617 if g != 884", "g dec 574 if mx < -1213",
			"ga dec -775 if bqr >= -404", "ic dec 186 if v < 55", "gyc dec 497 if ns >= -552", "z dec 619 if nno >= -319",
			"b dec -753 if av == 3809", "by inc -890 if n != 901", "b inc -714 if z == 4902", "n inc 927 if v <= 43",
			"b inc -107 if b <= -2947", "nno dec 707 if y == 1183", "vkg dec 199 if az <= -2996", "gyc inc -684 if cr <= -3523",
			"z dec -518 if zqo == 2429", "ga inc -220 if cr >= -3527", "zqo inc -922 if y <= 1183", "rn dec 852 if gyc != 2138",
			"nno inc 419 if ic == 1577", "rn dec -951 if by <= 5130", "av inc -557 if z == 4904", "mx dec 599 if g > 327",
			"az dec -594 if u != -5525", "gyc dec 187 if y > 1173", "v dec 2 if e >= 1153", "gyc inc -814 if ns < -540",
			"zqo dec -401 if av != 3249", "ic dec -511 if g == 318", "g dec -447 if g == 318", "zqo dec 965 if by != 5146",
			"gyc inc 658 if vkg != -824", "nm inc -663 if e > 1156", "n inc -974 if zqo <= 954", "n dec 363 if az <= -2389",
			"ga dec 300 if v <= 53", "n inc -402 if y > 1186", "b dec 839 if bqr >= -406", "rn inc -188 if v > 57",
			"av inc 952 if rn < 852", "bqr inc 973 if v == 56", "f dec -234 if b > -3899", "nm dec -96 if zqo != 949",
			"e dec 141 if ic != 2084", "ga dec 146 if g > 755", "av inc -333 if ga > -33", "z dec 698 if e == 1015",
			"e dec -493 if z >= 4200", "v dec 234 if v >= 48", "az dec 572 if vkg < -831", "vkg dec 164 if u >= -5527",
			"ga dec -933 if rn != 846", "gyc inc -180 if ic > 2081", "vkg dec -305 if z >= 4198", "ic inc 50 if cr == -3526",
			"egk inc -956 if zqo <= 939", "n inc 265 if z >= 4215", "az inc -574 if nm <= -2776", "bqr dec -19 if ic == 2141",
			"ga inc -682 if cr != -3525", "g dec 864 if g > 764", "ns inc -746 if f < 452", "g inc -969 if f >= 450",
			"ga inc -127 if e < 1518", "v inc -903 if egk == 3043", "cr dec 714 if nm < -2775", "f inc 33 if ga != -835",
			"bqr inc 919 if bqr != -382", "nm dec -489 if z < 4210", "vkg dec 962 if gyc != 1611", "z inc -698 if ga >= -844",
			"z dec 645 if e <= 1510", "ns dec -501 if y != 1174", "az inc -314 if av < 3868", "mx inc -207 if ns == -792",
			"z inc -604 if zqo < 952", "g inc 466 if g > -1067", "b inc -704 if nm >= -2295", "nno dec 284 if e != 1500",
			"cr dec 58 if z < 2263", "rn dec 494 if ic > 2148", "nm dec 211 if by < 5141", "by inc 150 if av >= 3862",
			"gyc inc -716 if b <= -4596", "u dec 466 if ns > -798", "mx inc -935 if cr == -4288", "bqr inc 275 if b < -4587",
			"v dec 771 if egk == 3043", "vkg inc -902 if u != -6000", "b dec -683 if nno == -1311", "egk dec 525 if v == -1860",
			"az inc 708 if by > 5284", "y inc -820 if ic >= 2137", "az inc -720 if zqo <= 940", "u inc -299 if zqo < 955",
			"bqr inc 248 if rn == 846", "g inc 148 if n > -441", "nm dec 213 if egk >= 2509", "ic inc -840 if g < -928",
			"f inc 98 if by >= 5287", "nm inc 909 if v != -1863", "vkg dec -856 if y >= 360", "v inc -156 if zqo == 946",
			"bqr dec 908 if gyc >= 900", "zqo inc 836 if f == 581", "ns inc -157 if ns == -792", "egk inc -899 if z == 2259",
			"ga dec 658 if ic < 2142", "nno inc -278 if by < 5291", "egk inc 502 if f > 580", "z dec 807 if y >= 363",
			"nm dec 612 if nno == -1583", "y inc 914 if ns <= -942", "gyc inc 419 if v != -2013", "g inc 897 if v <= -2015",
			"gyc inc 107 if vkg >= -1709", "bqr inc 224 if z < 1460", "mx dec 809 if ga < -1488", "cr inc -401 if av < 3875",
			"ns inc -521 if by == 5290", "az inc -269 if ga == -1494", "b dec -688 if by != 5294", "n inc -746 if mx < -2225",
			"y inc -452 if e < 1509", "z dec 495 if n <= -1175", "av dec 204 if egk != 2117", "cr inc 30 if ga >= -1498",
			"y dec -821 if gyc < 1437", "av dec -105 if bqr != -543", "by inc -897 if egk < 2130", "az inc 392 if vkg != -1703",
			"egk inc -34 if ga <= -1490", "mx dec -635 if av >= 3658", "gyc inc 307 if zqo < 1787", "v dec 263 if v >= -2018",
			"n dec 574 if f < 582", "f inc -432 if g < -28", "z dec 259 if av == 3663", "zqo inc 472 if b == -3909",
			"e inc -779 if bqr > -545", "zqo inc 187 if v > -2282", "e dec -395 if y != 1652", "y inc 312 if az < -3021",
			"ga dec -324 if egk < 2082", "cr dec -409 if az < -3023", "nm inc -790 if bqr != -533", "ga dec 252 if ga <= -1493",
			"ic inc -932 if u <= -6292", "e inc -547 if vkg >= -1698", "nno inc -171 if b != -3909",
			"g dec -891 if vkg < -1690", "ga inc 441 if egk < 2089", "z dec -708 if ic != 2147", "nm inc -402 if b == -3919",
			"f inc -374 if u != -6291", "ic inc -48 if ga >= -1312", "egk dec -19 if b > -3911", "av dec 305 if ga >= -1313",
			"g inc -475 if mx < -1588", "vkg inc -269 if vkg == -1700", "gyc inc -535 if f > 572",
			"nno dec -382 if ns >= -1475", "nno dec 650 if y <= 1959", "y inc 148 if zqo <= 2442", "by dec -223 if u != -6286",
			"y inc -319 if ga > -1313", "nno dec 13 if av == 3358", "ga inc -402 if gyc > 1198", "rn inc 37 if f == 581",
			"gyc inc -884 if rn > 882", "ic dec 676 if zqo <= 2438", "u inc 417 if ic != 2098", "n inc 930 if zqo <= 2449",
			"av inc 509 if ns >= -1477", "y dec -981 if y < 1779", "az inc -780 if ns < -1472", "f dec -834 if by != 4622",
			"ic inc 721 if av == 3867", "e dec 916 if g <= 400", "zqo inc 169 if cr > -4254", "nno inc -685 if z == 1406",
			"v dec -920 if zqo > 2440", "nno inc 91 if gyc < 317", "n inc -395 if nno != -2459", "egk inc -746 if v == -1359",
			"av inc 66 if cr == -4260", "gyc inc 40 if ga > -1711", "egk inc 914 if y < 1788", "g dec 176 if u != -5869",
			"b dec -550 if vkg >= -1974", "g inc -545 if ic < 2824", "nm inc -645 if cr <= -4256", "b inc -103 if ns >= -1477",
			"g dec 166 if zqo >= 2436", "nno dec 665 if by <= 4618", "rn dec 622 if nno > -3130", "nno inc -591 if b < -3461",
			"az inc -807 if egk >= 2272", "g dec -417 if f >= 1425", "by dec 547 if by != 4606", "cr inc -579 if e != 212",
			"nm dec 650 if e == 208", "b dec -65 if by > 4064", "b inc -263 if ns <= -1473", "az inc -784 if g >= -494",
			"b dec -193 if z > 1415", "bqr dec 143 if zqo >= 2443", "nno dec 385 if g >= -496", "v dec 352 if ic != 2814",
			"nno dec -521 if by != 4065", "ga dec 198 if az <= -4612", "by inc 926 if b > -3404", "bqr inc -697 if zqo == 2441",
			"vkg inc 124 if mx < -1588", "u dec -564 if bqr < -1237", "v inc -763 if ic >= 2812", "egk inc 339 if az > -4611",
			"z dec -617 if mx <= -1588", "av inc -876 if cr >= -4837", "n dec -600 if mx == -1595", "v inc -354 if vkg > -1850",
			"v inc 194 if vkg <= -1841", "bqr dec -206 if av <= 3934", "rn dec 547 if z != 2033", "b inc 217 if u <= -5305",
			"v dec -230 if egk != 2264", "n inc -447 if b != -3180", "az inc 497 if g >= -499", "e inc -527 if v < -2051",
			"u inc -889 if by == 4995", "b inc 74 if egk < 2282", "ga dec -65 if v == -2052", "nno inc 156 if av != 3933",
			"rn inc -880 if av != 3934", "cr dec 825 if ga == -1840", "y inc 499 if e == -319", "b inc 525 if mx > -1604",
			"f inc -731 if zqo >= 2437", "cr inc 398 if gyc < 360", "f inc 286 if vkg > -1846", "zqo inc 761 if ic > 2816",
			"y dec -877 if rn != -1166", "vkg inc -405 if ga != -1838", "vkg dec -522 if v <= -2060",
			"mx inc 281 if nno > -3586", "vkg dec -203 if bqr >= -1043", "vkg inc 278 if az >= -4125",
			"rn dec 544 if b < -2573", "av dec 730 if mx > -1318", "ic inc 49 if n < -620", "f dec -599 if nno != -3583",
			"e inc -871 if gyc == 355", "e dec -735 if mx != -1307", "n dec -73 if rn <= -1707", "ns inc 896 if g >= -502",
			"z dec -480 if z <= 2023", "zqo inc 595 if v < -2043", "mx dec 647 if vkg == -1769", "n inc 743 if nno > -3589",
			"gyc inc 129 if cr > -5267", "vkg inc 703 if mx > -1966", "bqr inc 378 if g <= -487", "ga dec 852 if zqo > 3035",
			"zqo dec -43 if y > 2280", "ga dec 849 if cr != -5259", "gyc inc -363 if av >= 3203", "vkg dec 481 if nm <= -3884",
			"g dec 609 if f < 964", "av inc -951 if n != 187", "ns inc -676 if zqo == 3079", "nno dec -998 if av == 2252",
			"n dec -624 if vkg > -1544", "y inc -262 if ns == -1250", "av dec 248 if ns < -1250", "f dec 351 if z == 2503",
			"bqr inc 535 if ic > 2858", "f inc -725 if nno > -2589", "ic inc 860 if v == -2052", "nno inc -211 if v >= -2055",
			"ic dec -324 if bqr <= -126", "az dec 746 if f <= -99", "g dec 985 if e <= -454", "f dec 434 if z < 2495",
			"egk dec 842 if bqr == -121", "gyc inc 715 if y >= 2022", "ns inc -573 if nm != -3888", "b dec 415 if mx >= -1951",
			"by inc -719 if ga == -3551", "n dec -783 if cr >= -5268", "nm dec 409 if by > 4988", "u inc -72 if av >= 2246",
			"ns inc 338 if v <= -2050", "e inc 12 if rn < -1706", "ns dec -354 if nm == -4299", "egk inc -589 if zqo >= 3084",
			"egk dec 856 if vkg != -1555", "ns inc -439 if zqo >= 3076", "by inc -942 if cr != -5272",
			"z inc 119 if b >= -2588", "cr inc -864 if ga != -3537", "nno inc -333 if gyc > 828", "z inc -530 if e > -451",
			"nno inc -113 if u > -6271", "g inc -213 if mx <= -1953", "y inc 107 if ga < -3536", "y dec -971 if az < -4859",
			"n dec -424 if av < 2258", "ic dec 632 if rn <= -1705", "az dec -701 if ns > -1576", "by dec -45 if egk > 581",
			"z inc 111 if vkg == -1547", "ic dec -737 if u <= -6266", "v dec 13 if nm != -4289", "b inc -616 if ic < 3838",
			"av inc 266 if b <= -3190", "nno dec -699 if gyc >= 834", "y dec 400 if gyc >= 835", "y dec -119 if ns <= -1567",
			"n dec 452 if rn < -1703", "egk inc -254 if vkg == -1556", "az inc 568 if ns <= -1580", "f dec -399 if bqr < -115",
			"zqo dec 948 if nno > -2440", "rn inc 154 if zqo == 2131", "f dec -21 if b != -3190", "nno inc -204 if g > -1683",
			"v dec 190 if az >= -4168", "bqr inc 236 if v > -2263", "cr dec 554 if nm >= -4303", "gyc inc -657 if e > -449",
			"e inc 65 if nm != -4306", "v inc 340 if az > -4171", "mx dec 527 if nm <= -4296", "g dec -426 if ic > 3819",
			"b dec -766 if z < 2205", "ic inc 822 if cr < -6677", "cr inc -733 if n != 958", "rn inc -759 if vkg == -1547",
			"nm dec 588 if rn == -2315", "n dec 310 if ic < 4641", "n dec -24 if g >= -1274", "n dec 341 if nm == -4887",
			"cr dec 131 if av > 2513", "zqo inc -78 if gyc < 181", "mx inc 368 if vkg < -1537", "gyc inc 157 if zqo >= 2053",
			"rn inc -871 if vkg <= -1544", "bqr dec 299 if egk == 576", "cr dec 27 if b <= -2423", "rn dec 848 if az < -4170",
			"av dec 836 if egk <= 577", "v dec -334 if cr < -7576", "f dec -820 if u == -6271", "az dec -367 if egk == 576",
			"by inc -267 if bqr != -192", "bqr dec 95 if cr == -7575", "ga dec -641 if bqr == -279", "ns inc -776 if y != 2821",
			"by dec 177 if cr != -7575", "nno dec 656 if g >= -1261", "z inc -641 if az >= -3789", "rn dec 93 if v != -1911",
			"az dec -664 if f > 1139", "ic dec -565 if e >= -380", "by inc -600 if ga == -2894", "zqo dec 580 if g <= -1263",
			"zqo dec -324 if by != 3784", "gyc dec -987 if ns != -1570", "gyc dec -921 if mx >= -2111",
			"by inc -529 if ic < 5211", "av dec 272 if ic == 5210", "nno inc 745 if e < -374", "ga inc 893 if by >= 3782",
			"egk dec -936 if bqr >= -284", "vkg dec -146 if nno <= -1679", "y inc -686 if ga != -2000",
			"b dec -659 if u < -6265", "b dec 418 if n < 629", "az inc 613 if ns == -1563", "vkg inc 385 if ga != -2017",
			"egk dec 54 if az == -3802", "vkg inc 330 if by == 3786", "f inc -243 if u != -6271", "gyc dec 751 if ns != -1574",
			"nno dec -285 if ga <= -2006", "vkg inc -257 if v >= -1921"
		};
	}
}
