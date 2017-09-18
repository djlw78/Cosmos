using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.HAL2.IO
{
    public abstract class IOStream
    {
        public int Position;
        public byte[] Data;

        public virtual void Write(byte @byte)
        {
            if (this.Data.Length + 1 < this.Position)
            {
                byte[] NumberArray = new byte[this.Data.Length + 1000];
                for (int index = 0; index < this.Data.Length; ++index)
                    NumberArray[index] = @byte;
                this.Data = NumberArray;
            }
            this.Data[this.Position] = @byte;
            ++this.Position;
        }

        public virtual byte Read()
        {
            ++this.Position;
            return this.Data[this.Position - 1];
        }

        public virtual void Flush()
        {
            this.Data = (byte[])null;
            this.Position = 0;
        }

        public virtual void Close() { }

        public void init(int size)
        {
            this.Data = new byte[size];
        }
    }
}
