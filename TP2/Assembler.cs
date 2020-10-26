using System;
using System.Collections.Generic;
using System.IO;

namespace TP2
{
    public class Assembler
    {
        private StreamReader _input;
        private Dictionary<string, Instruction> _nameToOp;
        private Dictionary<string, int> _regToNum;

        public Assembler(StreamReader input)
        {
            _input = input;

            _nameToOp = new Dictionary<string, Instruction>
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

            _regToNum = new Dictionary<string, int>
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
        }
    }
}