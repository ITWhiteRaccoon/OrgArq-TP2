namespace TP2
{
    public struct Instruction
    {
        public int opcode;
        public int funct;

        public Instruction(int opcode, int funct)
        {
            this.opcode = opcode;
            this.funct = funct;
        }
    }
}