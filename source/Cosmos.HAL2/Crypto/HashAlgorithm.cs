using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.HAL2.Crypto
{
    /// <summary>
    /// Sketchy-Ass-Abstract-Class, Please use wisely.
    /// </summary>
    public abstract class HashAlgorithm
    {
        public byte[] HashValue;

        public string getString(byte[] data)
        {
            string @string = "";
            foreach (byte number in this.Calculate(data))
                @string = Conversions.ByteToHex((int)number);
            return @string;
        }

        public byte[] Calculate(string @string)
        {
            byte[] NumberArray = new byte[@string.Length];
            int index = 0;
            foreach (byte number in @string)
            {
                NumberArray[index] = number;
                ++index;
            }
            return NumberArray;
        }

        public virtual byte[] Calculate(byte[] data)
        {
            return (byte[])null;
        }

        public virtual void Calculate(byte[] data, ref uint val)
        {

        }
    }
}
