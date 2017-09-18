using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.HAL2
{
    public class Devices
    {
        public static List<Devices.device> devices = new List<Devices.device>();

        public static HAL.BlockDevice.BlockDevice getDevice(string name)
        {
            for (int index = 0; index < Devices.devices.Count; ++index)
            {
                if (Devices.devices[index].name == name)
                    return Devices.devices[index].blockDevice;
            }
            throw new Exception("Device not found!");
        }

        public class device
        {
            public string name;
            public HAL.BlockDevice.BlockDevice blockDevice;
        }
    }
}
