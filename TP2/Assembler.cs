using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TP2
{
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

        public static IEnumerable<string> Assemble(string[] input)
        {
            var result = new List<string>();
            var labels = new Dictionary<string, int>();

            SearchLabels(input, labels); //Procura por labels primeiro para poder completar em caso jal label

            input = input.Where(x => !string.IsNullOrWhiteSpace(x) && !x.Equals(".text")).ToArray();

            int currentLine = 0;
            foreach (string line in input)
            {
                string bin = "";

                string[] content = line.Split(' ', ',');
                content = content.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                string instr = content[0];
                if (_nameToOp.ContainsKey(instr))
                {
                    bin = Convert.ToString(_nameToOp[instr].opcode, 2)[^6..];

                    switch (instr)
                    {
                        case "jal":
                            bin += Convert.ToString(labels[content[1]], 2).PadLeft(32, '0')[4..^2];
                            break;
                        case "jr":
                            break;
                        case "sll":
                        case "div":
                        case "nor":
                            break;
                        case "slti":
                        case "lw":
                        case "beq":
                        case "blez":
                        case "ori":
                            break;
                    }
                    result.Add(Convert.ToInt32(bin,2).ToString("x8"));
                }
            }

            return result;
        }

        private static void SearchLabels(string[] input, Dictionary<string, int> labels)
        {
            int currentLine = 0;

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
                    labels[label] = StartAddress + (4 * currentLine);
                    input[i] = input[i].Substring(labelSearch.Index + labelSearch.Length).Trim();
                    if (input[i].Length <= 0)
                    {
                        lastOpenLabel = label;
                        openLabel = true;
                        continue;
                    }
                }

                currentLine++;
            }
        }
    }
}