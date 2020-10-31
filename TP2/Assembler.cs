using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TP2
{
    /// <summary>
    /// Eduardo C. Andrade - 17111012-5
    /// Michael L. S. Rosa - 17204042-0
    /// Org. Arq. I - 2020/2 - TP2
    /// Provides a method that converts MIPS Assembly to hex code
    /// </summary>
    public class Assembler
    {
        private const int StartAddress = 0x00400000;

        private static Dictionary<string, Instruction> _nameToOp = new Dictionary<string, Instruction>
        {
            {"sll", new Instruction(0, 0)},
            {"div", new Instruction(0, 0x1a)},
            {"nor", new Instruction(0, 0x27)},

            {"jr", new Instruction(0, 8)},

            {"beq", new Instruction(0x04, 0)},
            {"blez", new Instruction(0x06, 0)},
            {"slti", new Instruction(0x0a, 0)},
            {"ori", new Instruction(0x0d, 0)},
            {"lw", new Instruction(0x23, 0)},

            {"jal", new Instruction(0x03, 0)},
        };

        private static Dictionary<string, int> _regToNum = new Dictionary<string, int>
        {
            {"$zero", 0},
            {"$at", 1},
            {"$v0", 2},
            {"$v1", 3},
            {"$a0", 4},
            {"$a1", 5},
            {"$a2", 6},
            {"$a3", 7},
            {"$t0", 8},
            {"$t1", 9},
            {"$t2", 10},
            {"$t3", 11},
            {"$t4", 12},
            {"$t5", 13},
            {"$t6", 14},
            {"$t7", 15},
            {"$s0", 16},
            {"$s1", 17},
            {"$s2", 18},
            {"$s3", 19},
            {"$s4", 20},
            {"$s5", 21},
            {"$s6", 22},
            {"$s7", 23},
            {"$t8", 24},
            {"$t9", 25},
            {"$k0", 26},
            {"$k1", 27},
            {"$gp", 28},
            {"$sp", 29},
            {"$fp", 30},
            {"$ra", 31},
        };

        /// <summary>
        /// Checks if the provided array contains the number of arguments needed.
        /// </summary>
        /// <param name="args">Array with the arguments</param>
        /// <param name="nArgs">Number of arguments</param>
        /// <param name="code">Assembly code for exception message</param>
        /// <exception cref="ArgumentException">Thrown if there are less arguments than expected in array, the exception message contains the code string.</exception>
        private static void CheckArgs<T>(T[] args, int nArgs, string code)
        {
            if (args.Length < nArgs)
            {
                throw new ArgumentException($"Faltam argumentos na linha '{code}'.");
            }
        }

        /// <summary>
        /// Checks if the provided dictionary contains all the keys from the array.
        /// </summary>
        /// <param name="dict">Dictionary for search</param>
        /// <param name="keys">Keys to lookup in dictionary</param>
        /// <param name="keyType">Type of key to show in exception message</param>
        /// <param name="code">Assembly code for exception message</param>
        /// <exception cref="ArgumentException">Thrown if any of key from the array is not found in the dictionary, the exception message contains the code string.</exception>
        private static void CheckKeys<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey[] keys, string keyType,
            string code)
        {
            foreach (TKey key in keys)
            {
                if (!dict.ContainsKey(key))
                {
                    throw new ArgumentException($"O {keyType} '{key}' na linha '{code}' não existe.");
                }
            }
        }

        /// <summary>
        /// Tries to parse the string to a positive/negative hex/decimal integer
        /// </summary>
        /// <param name="number">The string to convert</param>
        /// <returns>Int containing the number represented in the string</returns>
        private static int GetInt(string number)
        {
            //if (Regex.IsMatch(number,))
            return 0;
        }

        //
        public static IEnumerable<string> Assemble(string[] input)
        {
            //Procura por labels primeiro para poder completar (ex.: jal label)
            input = SearchLabels(input, out Dictionary<string, int> labels);

            //Procura pelos comandos assembly para converter
            return SearchInstructions(input, labels);
        }

        private static List<string> SearchInstructions(string[] input, Dictionary<string, int> labels)
        {
            var result = new List<string>();
            foreach (string line in input)
            {
                StringBuilder hex = new StringBuilder();

                string[] content = line.Split(' ', ',');
                content = content.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                CheckKeys(_nameToOp, new[] {content[0]}, "comando", line);

                Instruction instr = _nameToOp[content[0]];
                hex.Append(Convert.ToString(instr.opcode, 2).PadLeft(6, '0')[^6..]);

                if (instr.opcode == 3)
                {
                    CheckArgs(content, 2, line);
                    hex.Append(Convert.ToString(labels[content[1]], 2).PadLeft(32, '0')[4..^2]);
                }
                else if (instr.opcode == 0)
                {
                    int rs = 0, rt = 0, rd = 0, shamt = 0;
                    switch (instr.funct)
                    {
                        case 0: //sll r,r,i
                            CheckArgs(content, 4, line);
                            CheckKeys(_regToNum, content[1..3], "registrador", line);
                            rt = _regToNum[content[2]];
                            rd = _regToNum[content[1]];
                            shamt = Convert.ToInt32(content[3], 10);
                            break;
                        case 8: //jr r
                            CheckArgs(content, 2, line);
                            CheckKeys(_regToNum, content[1..2], "registrador", line);
                            rs = _regToNum[content[1]];
                            break;
                        case 0x1a: //div r,r
                            CheckArgs(content, 3, line);
                            CheckKeys(_regToNum, content[1..3], "registrador", line);
                            rs = _regToNum[content[1]];
                            rt = _regToNum[content[2]];
                            break;
                        case 0x27: //nor r,r,r
                            CheckArgs(content, 4, line);
                            CheckKeys(_regToNum, content[1..4], "registrador", line);
                            rs = _regToNum[content[2]];
                            rt = _regToNum[content[3]];
                            rd = _regToNum[content[1]];
                            break;
                    }

                    hex.Append(Convert.ToString(rs, 2).PadLeft(5, '0'))
                        .Append(Convert.ToString(rt, 2).PadLeft(5, '0'))
                        .Append(Convert.ToString(rd, 2).PadLeft(5, '0'))
                        .Append(Convert.ToString(shamt, 2).PadLeft(5, '0'))
                        .Append(Convert.ToString(instr.funct, 2).PadLeft(6, '0'));
                }
                else
                {
                    int rs = 0, rt = 0, imm = 0;
                    switch (instr.opcode)
                    {
                        case 4: //beq r,r,l
                            CheckArgs(content, 4, line);
                            CheckKeys(_regToNum, content[1..3], "registrador", line);
                            break;
                        case 6: //blez r,l
                            CheckArgs(content, 3, line);
                            CheckKeys(_regToNum, content[1..2], "registrador", line);
                            break;
                        case 0xa: //slti r,r,i
                            CheckArgs(content, 4, line);
                            CheckKeys(_regToNum, content[1..3], "registrador", line);
                            break;
                        case 0xd: //ori r,r,i
                            CheckArgs(content, 4, line);
                            CheckKeys(_regToNum, content[1..3], "registrador", line);
                            break;
                        case 0x23: //lw r,o(r)
                        {
                            CheckArgs(content, 3, line);
                            Match searchOffReg = Regex.Match(content[2], @"([^()]+)\(([^()]+)\)");
                            if (!searchOffReg.Success) { continue; }

                            string off = searchOffReg.Groups[1].Value, reg = searchOffReg.Groups[2].Value;
                            //if (!Regex.IsMatch())
                            CheckKeys(_regToNum, new[] {content[1], reg}, "registrador", line);
                            rs = _regToNum[reg];
                            rt = _regToNum[content[1]];
                            //imm = off;
                            break;
                        }
                    }

                    hex.Append(Convert.ToString(rs, 2).PadLeft(5, '0'))
                        .Append(Convert.ToString(rt, 2).PadLeft(5, '0'))
                        .Append(Convert.ToString(imm, 2).PadLeft(5, '0'));
                }

                result.Add($"0x{Convert.ToInt32(hex.ToString(), 2).ToString("x8")}");
            }

            return result;
        }

        private static string[] SearchLabels(string[] input, out Dictionary<string, int> labels)
        {
            labels = new Dictionary<string, int>();
            int currentAddress = 0;

            bool openLabel = false;
            string lastOpenLabel = "";
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = input[i].Trim();

                if (input[i].Contains(".text") || input[i].Length <= 0) { continue; }

                if (openLabel)
                {
                    labels[lastOpenLabel] = labels[lastOpenLabel] + 4;
                    openLabel = false;
                    continue;
                }

                Match labelSearch = Regex.Match(input[i], @"^[\w|\d]+:");
                if (labelSearch.Success)
                {
                    string label = labelSearch.Value.Substring(0, labelSearch.Value.Length - 1);
                    labels[label] = StartAddress + (4 * currentAddress);
                    input[i] = input[i].Substring(labelSearch.Index + labelSearch.Length).Trim();
                    if (input[i].Length <= 0)
                    {
                        lastOpenLabel = label;
                        openLabel = true;
                        continue;
                    }
                }

                currentAddress++;
            }

            input = input.Where(x => !string.IsNullOrWhiteSpace(x) && !x.Equals(".text")).ToArray();

            return input;
        }
    }
}