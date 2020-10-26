using System;
using System.IO;
using System.Linq;

namespace TP2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Any() && File.Exists(args[0]))
            {
                Assembler asm = new Assembler(File.OpenText(args[0]));

                return;
            }

            Console.WriteLine("Informe o caminho do arquivo.");
        }
    }
}