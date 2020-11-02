namespace TP2
{
    /// <summary>
    ///     Eduardo C. Andrade - 17111012-5
    ///     Michael L. S. Rosa - 17204042-0
    ///     Org. Arq. I - 2020/2 - TP2
    /// </summary>
    public readonly struct Instruction
    {
        public readonly int OpCode;
        public readonly int Funct;

        public Instruction(int opCode, int funct)
        {
            OpCode = opCode;
            Funct = funct;
        }
    }
}