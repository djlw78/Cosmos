using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmos.HAL
{

    namespace Extention
    {
        public static class PIT
        {
            public static void Mode0(uint frequency)
            {
                HAL2.IDT.Remap();
                uint divisor = 1193182 / frequency;
                Core.Memory.PortIO.outb(0x43, 0x30);
                Core.Memory.PortIO.outb(0x40, (byte)(divisor & 0xFF));
                Core.Memory.PortIO.outb(0x40, (byte)((divisor >> 8) & 0xFF));
                HAL2.IRQ.ClearMask(0);
                HAL2.IRQ.ClearMask(15);
            }
            public static void Mode2(uint frequency)
            {
                HAL2.IDT.Remap();
                uint divisor = 1193182 / frequency;
                Core.Memory.PortIO.outb(0x43, 0x36);
                Core.Memory.PortIO.outb(0x40, (byte)(divisor & 0xFF));
                Core.Memory.PortIO.outb(0x40, (byte)((divisor >> 8) & 0xFF));
                HAL2.IRQ.ClearMask(0);
                HAL2.IRQ.ClearMask(15);
            }
            public static void Beep(uint frequency)
            {
                uint divisor = 1193182 / frequency;
                Core.Memory.PortIO.outb(0x43, 0xB6);
                Core.Memory.PortIO.outb(0x42, (byte)(divisor & 0xFF));
                Core.Memory.PortIO.outb(0x42, (byte)((divisor >> 8) & 0xFF));
            }
            internal static bool called = false;
            public static void SleepSeconds(uint seconds)
            {
                SleepMilliseconds(seconds * 1000);
            }
            public static void SleepMilliseconds(uint milliseconds)
            {
                if (milliseconds <= 50)
                {
                    called = false;
                    Mode0(milliseconds.MsToHz());
                    while (!called) { }
                    called = false;
                }
                else
                {
                    uint mod = milliseconds % 100;
                    uint ms = milliseconds - mod;
                    for (int i = 0; i < ms; i += 50)
                    {
                        called = false;
                        Mode0(20);
                        while (!called) { }
                    }
                    called = false;
                    ms = mod % 2;
                    for (int i = 0; i < ms; i += 2)
                    {
                        called = false;
                        Mode0(500);
                        while (!called) { }
                    }
                    called = false;
                }
            }
            public static uint MsToHz(this int ms)
            {
                return (uint)(1000 / ms);
            }
            public static uint MsToHz(this uint ms)
            {
                return (uint)(1000 / ms);
            }
        }

    }


    /// <summary>
    /// Programmable Interval Timer
    /// with 1,193181818... MHz
    /// </summary>
    public class PIT : Device
    {
        public class PITTimer : IDisposable
        {
            internal int NSRemaining;
            public int NanosecondsTimeout;
            public bool Recuring;
            internal int ID = -1;

            public int TimerID
            {
                get
                {
                    return ID;
                }
            }

            public delegate void dOnTrigger();
            public dOnTrigger HandleTrigger;

            public PITTimer(dOnTrigger HandleOnTrigger, int NanosecondsTimeout, bool Recuring)
            {
                this.HandleTrigger = HandleOnTrigger;
                this.NanosecondsTimeout = NanosecondsTimeout;
                this.NSRemaining = this.NanosecondsTimeout;
                this.Recuring = Recuring;
            }
            public PITTimer(dOnTrigger HandleOnTrigger, int NanosecondsTimeout, int NanosecondsLeft)
            {
                this.HandleTrigger = HandleOnTrigger;
                this.NanosecondsTimeout = NanosecondsTimeout;
                this.NSRemaining = NanosecondsLeft;
                this.Recuring = true;
            }
            ~PITTimer()
            {
                Dispose();
            }

            public void Dispose()
            {
                if (ID != -1)
                {
                    //Global.PIT.UnregisterTimer(ID);
                }
            }
        }

        protected Core.IOGroup.PIT IO = Core.Global.BaseIOGroups.PIT;
        private List<PITTimer> ActiveHandlers = new List<PITTimer>();
        private ushort _T0Countdown = 65535;
        private ushort _T2Countdown = 65535;
        private int TimerCounter = 0;
        private bool WaitSignaled = false;
        public const uint PITFrequency = 1193180;
        public const uint PITDelayNS = 838;
        public bool T0RateGen = false;

        public ushort T0Countdown
        {
            get
            {
                return _T0Countdown;
            }
            set
            {
                _T0Countdown = value;

                IO.Command.Byte = (byte)(T0RateGen ? 0x34 : 0x30);
                IO.Data0.Byte = (byte)(value & 0xFF);
                IO.Data0.Byte = (byte)(value >> 8);
            }
        }
        public uint T0Frequency
        {
            get
            {
                return (PITFrequency / ((uint)_T0Countdown));
            }
            set
            {
                if (value < 19 || value > 1193180)
                {
                    throw new ArgumentException("Frequency must be between 19 and 1193180!");
                }

                T0Countdown = (ushort)(PITFrequency / value);
            }
        }
        public uint T0DelyNS
        {
            get
            {
                return (PITDelayNS * _T0Countdown);
            }
            set
            {
                if (value > 54918330)
                    throw new ArgumentException("Delay must be no greater that 54918330");

                T0Countdown = (ushort)(value / PITDelayNS);
            }
        }

        public ushort T2Countdown
        {
            get
            {
                return _T2Countdown;
            }
            set
            {
                _T2Countdown = value;

                IO.Command.Byte = 0xB6;
                IO.Data0.Byte = (byte)(value & 0xFF);
                IO.Data0.Byte = (byte)(value >> 8);
            }
        }
        public uint T2Frequency
        {
            get
            {
                return (PITFrequency / ((uint)_T2Countdown));
            }
            set
            {
                if (value < 19 || value > 1193180)
                {
                    throw new ArgumentException("Frequency must be between 19 and 1193180!");
                }

                T2Countdown = (ushort)(PITFrequency / value);
            }
        }
        public uint T2DelyNS
        {
            get
            {
                return (PITDelayNS * _T2Countdown);
            }
            set
            {
                if (value > 54918330)
                    throw new ArgumentException("Delay must be no greater than 54918330");

                T2Countdown = (ushort)(value / PITDelayNS);
            }
        }

        //TODO: Why is sound in PIT? Is it a function of the PIT?
        public void EnableSound()
        {
            //IO.Port61.Byte = (byte)(IO.Port61.Byte | 0x03);
        }
        public void DisableSound()
        {
            //IO.Port61.Byte = (byte)(IO.Port61.Byte | 0xFC);
        }
        public void PlaySound(int aFreq)
        {
            EnableSound();
            T2Frequency = (uint)aFreq;
        }
        public void MuteSound()
        {
            DisableSound();
        }

        private void SignalWait()
        {
            WaitSignaled = true;
        }

        public void Wait(uint TimeoutMS)
        {
            WaitSignaled = false;

            RegisterTimer(new PITTimer(SignalWait, (int)(TimeoutMS * 1000000), false));

            while (!WaitSignaled)
            {
                Core.Global.CPU.Halt();
            }
        }
        public void WaitNS(int TimeoutNS)
        {
            WaitSignaled = false;

            RegisterTimer(new PITTimer(SignalWait, TimeoutNS, false));

            while (!WaitSignaled)
            {
                Core.Global.CPU.Halt();
            }
        }

        public void HandleInterrupt()
        {
            int T0Delay = (int)T0DelyNS;
            PITTimer hndlr = null;
            for (int i = ActiveHandlers.Count - 1; i >= 0; i--)
            {
                hndlr = ActiveHandlers[i];

                hndlr.NSRemaining -= T0Delay;

                if (hndlr.NSRemaining < 1)
                {
                    if (hndlr.Recuring)
                    {
                        hndlr.NSRemaining = hndlr.NanosecondsTimeout;
                    }
                    else
                    {
                        hndlr.ID = -1;
                        ActiveHandlers.RemoveAt(i);
                    }
                    hndlr.HandleTrigger();
                }
            }
        }

        public int RegisterTimer(PITTimer timer)
        {
            if (timer.ID != -1)
                throw new InvalidOperationException("Timer has already been registered!");

            timer.ID = (TimerCounter++);
            ActiveHandlers.Add(timer);

            return timer.ID;
        }
        public void UnregisterTimer(int timerid)
        {
            for (int i = 0; i < ActiveHandlers.Count; i++)
            {
                if (ActiveHandlers[i].ID == timerid)
                {
                    ActiveHandlers[i].ID = -1;
                    ActiveHandlers.RemoveAt(i);
                    return;
                }
            }
        }

    }
}
