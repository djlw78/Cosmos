using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.HAL2
{
    public class DeviceFS : StorageDevice
    {
        public char Seperator = '/';

        public override void Chmod(string f, string perms)
        {
            throw new NotImplementedException();
        }

        public override void Chown(string f, string perms)
        {
            throw new NotImplementedException();
        }

        public override void Delete(string Path)
        {
            throw new NotImplementedException();
        }

        public override fsEntry[] getLongList(string dir)
        {
            string[] strArray = this.ListFiles(dir);
            List<fsEntry> fsEntryList = new List<fsEntry>();
            for (int index = 0; index < strArray.Length; ++index)
                fsEntryList.Add(new fsEntry()
                {
                    Name = strArray[index],
                    Owner = "sys",
                    //Time = b.b(),
                    Group = (byte)6,
                    User = (byte)6,
                    Attributes = (byte)3,
                    Global = (byte)0
                });
            return fsEntryList.ToArray();
        }

        public override string[] ListDirectories(string dir)
        {
            return this.ListJustFiles(dir);
        }

        public override string[] ListJustFiles(string dir)
        {
            return this.ListFiles(dir);
        }

        public override void makeDir(string name, string owner)
        {
            throw new NotImplementedException();
        }

        public override byte[] readFile(string name)
        {
            Devices.device device = new Devices.device();
            for (int DeviceCount = 0; DeviceCount < Devices.devices.Count; ++DeviceCount)
            {
                int Number = String.Utilites.LastIndexOf(Devices.devices[DeviceCount].name, this.Seperator);
                if (Devices.devices[DeviceCount].name.Substring(Number + 1) == String.Utilites.cleanName(name))
                {
                    byte[] NumberArray = new byte[checked((ulong)unchecked((long)Devices.devices[DeviceCount].blockDevice.BlockCount * (long)Devices.devices[DeviceCount].blockDevice.BlockCount))];
                    byte[] ArrayData = new byte[Devices.devices[DeviceCount].blockDevice.BlockCount];
                    for (int BlockDevicesCount = 0; BlockDevicesCount < (int)(uint)Devices.devices[DeviceCount].blockDevice.BlockCount; ++BlockDevicesCount)
                    {
                        Devices.devices[DeviceCount].blockDevice.ReadBlock((ulong)BlockDevicesCount, 1U, ArrayData);
                        for (int BlockSize = 0; BlockSize < (int)(uint)Devices.devices[DeviceCount].blockDevice.BlockSize; ++BlockSize)
                            NumberArray[BlockDevicesCount * (int)(uint)Devices.devices[DeviceCount].blockDevice.BlockSize + BlockSize] = ArrayData[BlockSize];
                    }
                    return NumberArray;
                }
            }
            throw new Exception("File not found!");
        }

        public override void saveFile(byte[] data, string name, string owner)
        {
            throw new NotImplementedException();
        }

        public override void Move(string s1, string s2)
        {
            throw new NotImplementedException();
        }

        public override string[] ListFiles(string Directory)
        {
            List<string> StringList = new List<string>();
            for (int index = 0; index < Devices.devices.Count; ++index)
            {
                int Number = String.Utilites.LastIndexOf(Devices.devices[index].name, this.Seperator);
                StringList.Add(Devices.devices[index].name.Substring(Number + 1));
            }
            return StringList.ToArray();
        }
    }
}
