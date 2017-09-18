using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.HAL2
{
    public class fsEntry
    {
        public byte User = 6;
        public byte Group = 4;
        public byte Global = 4;
        public string Name;
        public byte Attributes;
        public int Pointer;
        public int Length;
        public string Owner;
        public string Time;
        public int Checksum;
    }
}
