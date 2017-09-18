using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.HAL2
{
    public class RootFilesystem : StorageDevice
    {
        public List<MountPoint> Mountpoints = new List<MountPoint>();
        public char Seperator = '/';

        public void Mount(string Directory, StorageDevice sd)
        {
            MountPoint mountPoint = new MountPoint();
            Directory = String.Utilites.cleanName(Directory);
            mountPoint.Path = Directory;
            mountPoint.device = sd;
            this.Mountpoints.Add(mountPoint);
        }

        public bool isMountPoint(string Directory)
        {
            for (int index = 0; index < this.Mountpoints.Count; ++index)
            {
                if (String.Utilites.cleanName(this.Mountpoints[index].Path) == String.Utilites.cleanName(Directory))
                    return true;
            }
            return false;
        }

        public void Unmount(string device)
        {
            List<MountPoint> mountPointList = new List<MountPoint>();
            for (int index = 0; index < this.Mountpoints.Count; ++index)
            {
                if (String.Utilites.cleanName(this.Mountpoints[index].Path) != String.Utilites.cleanName(device))
                    mountPointList.Add(this.Mountpoints[index]);
            }
            this.Mountpoints = mountPointList;
        }

        public override void Chmod(string Directory, string perms)
        {
            Directory = String.Utilites.cleanName(Directory);
            for (int index = 0; index < this.Mountpoints.Count; ++index)
            {
                if (this.Mountpoints[index].Path.Length <= Directory.Length && this.Mountpoints[index].Path != "" && (Directory.Substring(0, this.Mountpoints[index].Path.Length) == this.Mountpoints[index].Path && this.Mountpoints[index].Path != ""))
                {
                    this.Mountpoints[index].device.Chmod(Directory.Substring(this.Mountpoints[index].Path.Length), perms);
                    return;
                }
            }
            this.Mountpoints[0].device.Chmod(Directory, perms);
        }

        public override void Chown(string Directory, string perms)
        {
            Directory = String.Utilites.cleanName(Directory);
            for (int index = 0; index < this.Mountpoints.Count; ++index)
            {
                if (this.Mountpoints[index].Path.Length <= Directory.Length && this.Mountpoints[index].Path != "" && (Directory.Substring(0, this.Mountpoints[index].Path.Length) == this.Mountpoints[index].Path && this.Mountpoints[index].Path != ""))
                {
                    this.Mountpoints[index].device.Chown(Directory.Substring(this.Mountpoints[index].Path.Length), perms);
                    return;
                }
            }
            this.Mountpoints[0].device.Chown(Directory, perms);
        }

        public override string[] ListJustFiles(string Directory)
        {
            Directory = String.Utilites.cleanName(Directory);
            for (int index = 0; index < this.Mountpoints.Count; ++index)
            {
                if (this.Mountpoints[index].Path.Length <= Directory.Length && this.Mountpoints[index].Path != "" && Directory.Substring(0, this.Mountpoints[index].Path.Length) == this.Mountpoints[index].Path)
                    return this.Mountpoints[index].device.ListJustFiles(Directory.Substring(this.Mountpoints[index].Path.Length));
            }
            return this.Mountpoints[0].device.ListJustFiles(Directory);
        }

        public override fsEntry[] getLongList(string Directory)
        {
            Directory = String.Utilites.cleanName(Directory);
            for (int index = 0; index < this.Mountpoints.Count; ++index)
            {
                if (this.Mountpoints[index].Path.Length <= Directory.Length && this.Mountpoints[index].Path != "" && Directory.Substring(0, this.Mountpoints[index].Path.Length) == this.Mountpoints[index].Path)
                    return this.Mountpoints[index].device.getLongList(Directory.Substring(this.Mountpoints[index].Path.Length));
            }
            return this.Mountpoints[0].device.getLongList(Directory);
        }

        public override void Move(string FromDirectoryPath, string ToDirectoryPath)
        {
            FromDirectoryPath = String.Utilites.cleanName(FromDirectoryPath);
            ToDirectoryPath = String.Utilites.cleanName(ToDirectoryPath);
            for (int index = 0; index < this.Mountpoints.Count; ++index)
            {
                if (this.Mountpoints[index].Path.Length <= FromDirectoryPath.Length && this.Mountpoints[index].Path != "" && FromDirectoryPath.Substring(0, this.Mountpoints[index].Path.Length) == this.Mountpoints[index].Path)
                {
                    this.Mountpoints[index].device.Move(FromDirectoryPath.Substring(this.Mountpoints[index].Path.Length), ToDirectoryPath.Substring(this.Mountpoints[index].Path.Length));
                    return;
                }
            }
            this.Mountpoints[0].device.Move(FromDirectoryPath, ToDirectoryPath);
        }

        public override void makeDir(string Directory, string Owner)
        {
            Directory = String.Utilites.cleanName(Directory);
            for (int index = 0; index < this.Mountpoints.Count; ++index)
            {
                if (this.Mountpoints[index].Path.Length <= Directory.Length && this.Mountpoints[index].Path != "" && Directory.Substring(0, this.Mountpoints[index].Path.Length) == this.Mountpoints[index].Path)
                {
                    this.Mountpoints[index].device.makeDir(Directory.Substring(this.Mountpoints[index].Path.Length), Owner);
                    return;
                }
            }
            this.Mountpoints[0].device.makeDir(Directory, Owner);
        }

        public override byte[] readFile(string Directory)
        {
            Directory = String.Utilites.cleanName(Directory);
            for (int index = 0; index < this.Mountpoints.Count; ++index)
            {
                if (this.Mountpoints[index].Path.Length <= Directory.Length && this.Mountpoints[index].Path != "" && Directory.Substring(0, this.Mountpoints[index].Path.Length) == this.Mountpoints[index].Path)
                    return this.Mountpoints[index].device.readFile(Directory.Substring(this.Mountpoints[index].Path.Length));
            }
            return this.Mountpoints[0].device.readFile(Directory);
        }

        public override void saveFile(byte[] data, string Directory, string Owner)
        {
            Directory = String.Utilites.cleanName(Directory);
            for (int index = 0; index < this.Mountpoints.Count; ++index)
            {
                if (this.Mountpoints[index].Path.Length <= Directory.Length && this.Mountpoints[index].Path != "" && Directory.Substring(0, this.Mountpoints[index].Path.Length) == this.Mountpoints[index].Path)
                {
                    this.Mountpoints[index].device.saveFile(data, Directory.Substring(this.Mountpoints[index].Path.Length), Owner);
                    return;
                }
            }
            this.Mountpoints[0].device.saveFile(data, Directory, Owner);
        }

        public override void Delete(string Directory)
        {
            Directory = String.Utilites.cleanName(Directory);
            for (int index = 0; index < this.Mountpoints.Count; ++index)
            {
                if (this.Mountpoints[index].Path.Length <= Directory.Length && this.Mountpoints[index].Path != "" && Directory.Substring(0, this.Mountpoints[index].Path.Length) == this.Mountpoints[index].Path)
                {
                    this.Mountpoints[index].device.Delete(Directory.Substring(this.Mountpoints[index].Path.Length));
                    return;
                }
            }
            this.Mountpoints[0].device.Delete(Directory);
        }

        public override string[] ListDirectories(string Directory)
        {
            Directory = String.Utilites.cleanName(Directory);
            for (int index = 0; index < this.Mountpoints.Count; ++index)
            {
                if (this.Mountpoints[index].Path.Length <= Directory.Length && this.Mountpoints[index].Path != "")
                {
                    if (Directory.Substring(0, this.Mountpoints[index].Path.Length) == this.Mountpoints[index].Path)
                        return this.Mountpoints[index].device.ListDirectories(Directory.Substring(this.Mountpoints[index].Path.Length));
                }
                else if (!(this.Mountpoints[index].Path == ""))
                    ;
            }
            return this.Mountpoints[0].device.ListDirectories(Directory);
        }

        public override string[] ListFiles(string Directory)
        {
            Directory = String.Utilites.cleanName(Directory);
            for (int index = 0; index < this.Mountpoints.Count; ++index)
            {
                if (this.Mountpoints[index].Path.Length <= Directory.Length && this.Mountpoints[index].Path != "")
                {
                    if (Directory.Substring(0, this.Mountpoints[index].Path.Length) == this.Mountpoints[index].Path)
                        return this.Mountpoints[index].device.ListFiles(Directory.Substring(this.Mountpoints[index].Path.Length));
                }
                else if (!(this.Mountpoints[index].Path == "")) ;
            }
            return this.Mountpoints[0].device.ListFiles(Directory);
        }
    }
}
