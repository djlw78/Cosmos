using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.HAL2.IO
{
    public class TextReader
    {
        public int Position = 0;
        private char[] Character;
        public int Length;

        public TextReader(string str)
        {
            this.Character = str.ToCharArray();
            this.Length = this.Character.Length;
        }

        public char Read()
        {
            ++this.Position;
            return this.Character[this.Position - 1];
        }

        public char Peek()
        {
            if (this.Position < this.Character.Length)
                return this.Character[this.Position];
            return 'ÿ';
        }

        public char Peek(int depth)
        {
            if (this.Position < this.Character.Length)
                return this.Character[this.Position + depth];
            return 'ÿ';
        }
    }
}
