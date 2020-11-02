using System;
using System.Collections.Generic;
using System.IO;

namespace TP2
{
    /// <summary>
    ///     Eduardo C. Andrade - 17111012-5
    ///     Michael L. S. Rosa - 17204042-0
    ///     Org. Arq. I - 2020/2 - TP2
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length >= 3 && File.Exists(args[1]))
            {
                var result = new List<string>();
                string[] input = File.ReadAllLines(args[1]);
                result = args[0] switch
                {
                    "-a" => Assembler.Assemble(input),
                    "-d" => Assembler.Disassemble(input),
                    _ => result
                };

                File.WriteAllLines(args[2], result);

                Console.WriteLine("Sucesso!");
                return;
            }

            Console.WriteLine(
                "Informe pela linha de comando a operação desejada e os caminhos de entrada e saída. (eg.: Program -a example.asm example.txt)");
        }
    }
}