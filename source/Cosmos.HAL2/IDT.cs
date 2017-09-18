using System;
using System.Collections.Generic;
using System.Text;
namespace Cosmos.HAL2
{
    public class IDT
    {
        public delegate void ISR();
        public static ISR[] idt = new ISR[0xFF];
        public static void Remap()
        {
            
            Core.Memory.PortIO.outb(0x20, 0x11);
            Core.Memory.PortIO.outb(0xA0, 0x11);
            Core.Memory.PortIO.outb(0x21, 0x20);
            Core.Memory.PortIO.outb(0xA1, 0x28);
            Core.Memory.PortIO.outb(0x21, 0x04);
            Core.Memory.PortIO.outb(0xA1, 0x02);
            Core.Memory.PortIO.outb(0x21, 0x01);
            Core.Memory.PortIO.outb(0xA1, 0x01);
            Core.Memory.PortIO.outb(0x21, 0x0);
            Core.Memory.PortIO.outb(0xA1, 0x0);
        }

        public static void SetGate(byte int_num, ISR handler)
        {
            idt[int_num] = handler;
        }

        private void idt_handler()
        {
            int num = 0;
            if (idt[num] != null)
            {
                idt[num]();
            }
        }
    }
}
