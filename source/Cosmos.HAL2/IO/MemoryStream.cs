using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.HAL2.IO
{
    public class MemoryStream : IOStream
    {
        private unsafe byte* ByteStream = (byte*)null;

        public unsafe MemoryStream(int size)
        {
            this.ByteStream = (byte*)null;
            this.init(size);
        }

        public unsafe MemoryStream(byte[] data)
        {
            this.ByteStream = (byte*)null;
            this.Data = data;
        }

        public unsafe MemoryStream(byte* ptr)
        {
            this.ByteStream = ptr;
        }

        public override void Close() { }

        public override unsafe void Write(byte i)
        {
            if ((IntPtr)this.ByteStream == IntPtr.Zero)
            {
                base.Write(i);
            }
            else
            {
                this.ByteStream[this.Position] = i;
                ++this.Position;
            }
        }

        public override unsafe byte Read()
        {
            if ((IntPtr)this.ByteStream == IntPtr.Zero)
                return base.Read();
            ++this.Position;
            return this.ByteStream[this.Position - 1];
        }
    }
}
