using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TP2
{
    /// <summary>
    /// Eduardo C. Andrade - 17111012-5
    /// Michael L. S. Rosa - 17204042-0
    /// Org. Arq. I - 2020/2 - TP2
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int.TryParse("-10", out int n);
            Console.WriteLine(n);
            return;
            if (args.Length >= 2 && File.Exists(args[0]))
            {
                IEnumerable<string> hex = Assembler.Assemble(File.ReadAllLines(args[0]));
                File.WriteAllLines(args[1], hex);

                Console.WriteLine("Sucesso!");
                return;
            }

            Console.WriteLine("Informe pela linha de comando os caminhos de entrada e saída, respectivamente.");
        }
    }
}