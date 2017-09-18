using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.HAL2.IO
{
    public class File
    {
        public static string Open(string fname)
        {
            string str = "";
            foreach (byte number in FileSystem.Root.readFile(fname))
                str += ((char)number).ToString();
            return str;
        }

        private static bool Convert(byte @byte, int number)
        {
            return ((int)@byte & 1 << number - 1) != 0;
        }

        public static bool CanExecute(fsEntry Entry)
        {
            return File.Convert(Entry.User, 1) && CurrentUser.Username == Entry.Owner || File.Convert(Entry.Group, 1) || File.Convert(Entry.Global, 1);
        }

        public static bool isExecutable(string file)
        {
            fsEntry[] longList = FileSystem.Root.getLongList(file.Substring(0, String.Utilites.LastIndexOf(file, '/')));
            for (int index = 0; index < longList.Length; ++index)
            {
                if (longList[index].Name == file.Substring(String.Utilites.LastIndexOf(file, FileSystem.Root.Seperator) + 1) && File.CanExecute(longList[index]))
                    return true;
            }
            return false;
        }

        public static void Delete(string File)
        {
            FileSystem.Root.Delete(File);
        }

        public static void Chmod(string name, string chmod)
        {
            FileSystem.Root.Chmod(name, chmod);
        }

        public static void Save(string name, string text)
        {
            BinaryWriter binaryWriter = new BinaryWriter((IOStream)new FileStream(name, "w"));
            foreach (byte @byte in text)
                binaryWriter.BaseStream.Write(@byte);
            binaryWriter.BaseStream.Close();
        }
    }
}
