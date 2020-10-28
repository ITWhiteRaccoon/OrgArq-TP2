using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TP2
{
    class Program
    {
        static void Main(string[] args)
        {
            string b = "00001100000100000000000000001000";
            Console.WriteLine($"0x{Convert.ToInt32(b, 2).ToString("x8")}");
            return;
            if (args.Length >= 2 && File.Exists(args[0]))
            {
                IEnumerable<string> bin = Assembler.Assemble(File.ReadAllLines(args[0]));
                File.WriteAllLines(args[1], bin);

                Console.WriteLine("Sucesso!");
                return;
            }

            Console.WriteLine("Informe pela linha de comando os caminhos de entrada e saída, respectivamente.");
        }
    }
}